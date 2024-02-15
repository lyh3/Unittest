using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.Shared.Extensions;
using log4net;
using System.Reflection;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;
using Intel.MsoAuto.C3.PITT.Business.Models;
using Intel.MsoAuto.C3.PITT.Business.Services.Interfaces;
using System.Text;
using System.Text.Json;
using Intel.MsoAuto.C3.PITT.Business.Models.Lists;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class StateModelService
    {
        private readonly StateModelDataContext _stateModelDataContext;
        private readonly ProjectService _projectService;
        private readonly XCCBService _xCCBService;
        private readonly string? _env = null;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public StateModelService(string? env = null)
        {
            _stateModelDataContext = new StateModelDataContext();
            _projectService = new ProjectService();
            _xCCBService = new XCCBService();
            _env = env;
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
                await _projectService.UpdateProjectState(project.id!, ExternalStatus.DELAYED);
            }
            else if (project.externalStatus != ExternalStatus.ON_TRACK)
            {
                await _projectService.UpdateProjectState(project.id!, ExternalStatus.ON_TRACK);
            }
        }

        public void UpdateProjectStates()
        {
            log.Info("--> UpdateProjectStates" + "(" + Core.Settings.Env + ")");
            List<Project> projects = _projectService.GetAllActiveProjects();
            foreach (Project project in projects)
            {
                UpdateProjectState(project);
            }
            log.Info("<-- UpdateProjectStates" + "(" + Core.Settings.Env + ")");
        }

        public async Task UpdateStatusesOnStateModelAssociationProjectAsync()
        {

            log.Info("--> UpdateStatusesOnStateModelAssociationProject" + "(" + Core.Settings.Env + ")");
            List<StateModelAssociationProject> smaProjects = _stateModelDataContext.GetStateModelAssociationProjects();

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
                        await _stateModelDataContext.UpdateStateModelAssociationProjectById(smaProject);
                    }
                    catch (Exception)
                    {
                        // TODO: Logging?
                        throw;
                    }


                }

            }

            log.Info("<-- UpdateStatusesOnStateModelAssociationProject" + "(" + Core.Settings.Env + ")");
        }

        public StateModelAssociationProject? GetStateModelAssociationProjectByProjectId(string projectId)
        {
            return _stateModelDataContext.GetStateModelAssociationProjectByProjectId(projectId);
        }

        public TransitionState GetDoneTransitionState()
        {
            return new TransitionState()
            {
                id = "64c16489155a71a6ace0e196",
                name = "Done",
                duration = 0,
                risk = 100,
                contactGroups = new ContactGroups(),
                isActive = true,
                endDate = null,
                startDate = null,
                nextStateId = null,
                status = null,
                taskDescription = null,
                updatedBy = "System",
                updatedOn = DateTime.UtcNow,
            };
        }

        public async Task<StateModelAssociationProject?> ProgressStateModelAssociationByTransitionState(string projectId, string activeStateId, TransitionState newTransitionState)
        {
            // Setup the azure auth
            AzureAuthentication sso = new AzureAuthentication();
            // Get a token for our desired account
            RequestToken? token = await sso.GetAccessToken(Core.Settings.AAMSettings.genericAccount);

            if (token == null)
            {
                return null;
            }

            string authToken = token.accessToken;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                    string json = JsonSerializer.Serialize(newTransitionState);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    string uri = Core.Settings.PITTApiUrl + "/api/statemodels/transitionstatemodelassociationbyprojectid/" + projectId + "/" + activeStateId;

                    HttpResponseMessage response = await client.PutAsync(uri, httpContent);

                    string jsonString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        StateModelAssociationProject? smaProj = JsonSerializer.Deserialize<StateModelAssociationProject>(jsonString);
                        return smaProj;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                // Do nothing
            }

            return null;
        }

        public async Task UpdateStateModelByxCCBData(XCCB.Business.Entities.XccbDocument doc, BaseProject project, StateModelAssociationProject smaProj)
        {
            StateModelAssociation sma = smaProj.stateModelAssociation;

            StateModel stateModel = sma.stateModel;
            List<State> states = stateModel.states;

            // Find if "FWP in Process" is in states and is active
            State? fwpInProcess = states.Find(x => x.id == Core.Constants.PITT_STATE_FWP_IN_PROCESS && x.status == "active");

            // Abort
            if (fwpInProcess == null) return;

            // Progress the workflow

            string FWP_IN_PROCESS_STATE_ID = Core.Constants.PITT_STATE_FWP_IN_PROCESS;
            TransitionState doneTransitionState = GetDoneTransitionState();
            StateModelAssociationProject? res = await ProgressStateModelAssociationByTransitionState(project.id, FWP_IN_PROCESS_STATE_ID, doneTransitionState);
        }

        public async Task UpdateStateModelsByxCCBData()
        {
            List<XCCB.Business.Entities.XccbDocument> updatedFWPApprovedDocs = _xCCBService.GetUpdatedFWPApprovedxCCBDocs();

            foreach (XCCB.Business.Entities.XccbDocument doc in updatedFWPApprovedDocs)
            {
                // Get the projects by doc.documentId (We have a type mismatch between project and xCCB Docs)
                List<Project> xCCBProjects = _projectService.GetProjectsByFinalxCCBDocNum(doc.documentID.ToStringSafely());
                List<BaseProject> xCCBBaseProjects = xCCBProjects.Select(x => _projectService.ConvertProjectToPITTBaseProject(x)).ToList();
                foreach (BaseProject project in xCCBBaseProjects)
                {
                    StateModelAssociationProject? smaProject = GetStateModelAssociationProjectByProjectId(project.id!);

                    if (smaProject == null) { continue; }

                    await UpdateStateModelByxCCBData(doc, project, smaProject);
                }
            }

        }

        public async Task StartStateModelDailyTask()
        {
            try
            {
                await UpdateStateModelsByxCCBData();
                await UpdateStatusesOnStateModelAssociationProjectAsync();
                UpdateProjectStates();
            }
            catch (Exception ex)
            {
                ex.ExceptionEmailNotification(GetType().Name, new Configurations(environment: _env, sendEmail: true));
            }
        }
    }
}
