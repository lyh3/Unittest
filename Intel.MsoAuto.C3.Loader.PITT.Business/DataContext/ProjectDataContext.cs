using System.Reflection;
using log4net;
using MongoDB.Driver;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;
using Intel.MsoAuto.C3.PITT.Business.Models.Interfaces;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{
    public class ProjectDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IMongoCollection<Project> _projectCollection;

        public ProjectDataContext()
        {
            _projectCollection = new MongoDataAccess().GetMongoCollection<Project>(Constants.PITT_PROJECTS);
        }

        public async Task UpdateProjectState(string id, ExternalStatus status)
        {

            FilterDefinition<Project> filter = Builders<Project>.Filter.Eq(x => x.id, id);
            UpdateDefinition<Project> update = Builders<Project>.Update.Set(x => x.externalStatus, status);

            UpdateResult result = await _projectCollection.UpdateOneAsync(filter, update);
        }

        public List<Project> GetAllActiveProjects()
        {
            IQueryable<Project> docs = _projectCollection.AsQueryable()
                .Where(x => x.isActive && x.lifeCycleStatus != LifeCycleStatus.DROPPED && x.lifeCycleStatus != LifeCycleStatus.COMPLETED)
                .Where(x => x.updatedProjectECD != null);

            List<Project> projects = new List<Project>();
            projects.AddRange(docs.ToList());

            return projects;
        }

        public List<Project> GetProjectsByFinalxCCBDocNum(string xCCBDocNum)
        {
            IQueryable<Project> docs = _projectCollection.AsQueryable()
                .Where(x => x.isActive && x.lifeCycleStatus != LifeCycleStatus.DROPPED)
                .Where(x => x.finalXccbDocumentNum != null && x.finalXccbDocumentNum == xCCBDocNum);

            List<Project> projects = new List<Project>();
            projects.AddRange(docs.ToList());

            return projects;
        }

    }
}
