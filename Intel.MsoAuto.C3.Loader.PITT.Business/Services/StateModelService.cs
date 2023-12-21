using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;
using Intel.MsoAuto.Shared.Extensions;
using log4net;
using System.Reflection;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class StateModelService
    {
        private readonly StateModelDataContext stateModelDataContext;
        private readonly ProjectDataContext projectDataContext;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public StateModelService()
        {
            stateModelDataContext = new StateModelDataContext();
            projectDataContext = new ProjectDataContext();
        }

        public State CalculateStateStatusByState(State state)
        {
            float? atRiskThreshold = state!.risk;
            int? duration = state.duration;
            DateTime? startTime = state!.startDate;

            if (atRiskThreshold == null || startTime == null || duration == null)
            {
                // Abort
                return state;
            }

            // Convert threshold to a precentage
            atRiskThreshold /= 100;

            // Get todays date
            DateTime now = DateTime.UtcNow;

            // We have to do an explicit cast because it things startTime is null but its not
            TimeSpan span = (TimeSpan)(now - startTime);
            double totalDaysElapsed = span!.TotalDays;

            float? atRiskDays = duration! * atRiskThreshold!;

            if (now > state.endDate)
            {
                state.externalStatus = ExternalStatus.DELAYED;
            }
            else if (totalDaysElapsed >= atRiskDays)
            {
                state.externalStatus = ExternalStatus.AT_RISK;
            }
            else
            {
                state.externalStatus = ExternalStatus.ON_TRACK;
            }

            state.updatedOn = DateTime.UtcNow;
            state.updatedBy = "system";

            return state;
        }

        public async void UpdateProjectState(Project project)
        {
            DateTime? projectECD = project.updatedProjectECD;
            DateTime now = DateTime.UtcNow;

            if (projectECD == null) { return; }

            if (now >= projectECD && project.externalStatus != ExternalStatus.DELAYED)
            {
                await projectDataContext.UpdateProjectState(project.id!, ExternalStatus.DELAYED);
            }
            else if (project.externalStatus != ExternalStatus.ON_TRACK)
            {
                await projectDataContext.UpdateProjectState(project.id!, ExternalStatus.ON_TRACK);
            }
        }

        public void UpdateProjectStates()
        {
            log.Info("--> UpdateProjectStates" + "(" + Settings.Env + ")");
            List<Project> projects = projectDataContext.GetAllActiveProjects();
            foreach (Project project in projects)
            {
                UpdateProjectState(project);
            }
            log.Info("<-- UpdateProjectStates" + "(" + Settings.Env + ")");
        }

        public async Task UpdateStatusesOnStateModelAssociationProjectAsync()
        {

            log.Info("--> UpdateStatusesOnStateModelAssociationProject" + "(" + Settings.Env + ")");
            List<StateModelAssociationProject> smaProjects = stateModelDataContext.GetStateModelAssociationProjects();

            foreach (StateModelAssociationProject smaProject in smaProjects)
            {
                bool modifiedSMA = false;
                StateModelAssociation sma = smaProject.stateModelAssociation;

                for (int i = 0; i < sma.stateModel.states.Count; i++)
                {
                    State state = sma.stateModel.states[i];
                    ExternalStatus externalStatus = state.externalStatus;

                    if (state.status == StateStatus.ACTIVE)
                    {
                        State updatedState = CalculateStateStatusByState(state);
                        if (updatedState.externalStatus == externalStatus)
                        {
                            // No update -- dont do anything
                            continue;
                        }
                        sma.stateModel.states[i] = updatedState;
                        modifiedSMA = true;
                    }

                }

                if (modifiedSMA)
                {
                    sma.updatedBy = "system";
                    sma.updatedOn = DateTime.UtcNow;
                    smaProject.updatedBy = "system";
                    smaProject.updatedOn = DateTime.UtcNow;
                    smaProject.stateModelAssociation = sma;

                    // We need to update this smaProj
                    string? id = smaProject.id;
                    if (id == null)
                    {
                        // abort
                        return;
                    }
                    try
                    {
                        await stateModelDataContext.UpdateStateModelAssociationProjectById(smaProject);
                    }
                    catch (Exception)
                    {
                        // TODO: Logging?
                        throw;
                    }


                }

            }

            log.Info("<-- UpdateStatusesOnStateModelAssociationProject" + "(" + Settings.Env + ")");
        }

        public async void StartStateModelDailyTask()
        {
            await UpdateStatusesOnStateModelAssociationProjectAsync();
            UpdateProjectStates();
        }
    }
}
