using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.PITT.Business.Models;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;
using Intel.MsoAuto.C3.PITT.Business.Services;
using log4net;
using System.Reflection;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class ProjectService
    {
        private readonly ProjectDataContext projectDataContext;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProjectService()
        {
            projectDataContext = new ProjectDataContext();
        }

        public List<Project> GetAllActiveProjects()
        {
            return projectDataContext.GetAllActiveProjects();
        }

        public List<Project> GetProjectsByFinalxCCBDocNum(string xCCBDocNum)
        {
            return projectDataContext.GetProjectsByFinalxCCBDocNum(xCCBDocNum);
        }

        public BaseProject ConvertProjectToPITTBaseProject(Project project)
        {
            return new BaseProject()
            {
                id = project.id,
                sitesAffected = project.sitesAffected,
                lifeCycleStatus = project.lifeCycleStatus,
                isActive = project.isActive,
                isDraft = project.isDraft,
                externalStatus = project.externalStatus,
                createdBy = project.createdBy,
                createdOn = project.createdOn,
                updatedBy = project.updatedBy,
                updatedOn = project.updatedOn,
            };
        }

        public async Task UpdateProjectState(string projectId, ExternalStatus status)
        {
            await projectDataContext.UpdateProjectState(projectId, status);
        }


    }
}
