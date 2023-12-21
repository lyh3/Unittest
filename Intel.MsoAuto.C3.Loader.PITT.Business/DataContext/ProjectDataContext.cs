using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using System.Text;
using System.Threading.Tasks;
using Intel.MsoAuto.Shared.Extensions;
using MongoDB.Driver;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{
    public class ProjectDataContext
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ProjectDataContext() { }

        public async Task UpdateProjectState(string id, ExternalStatus status)
        {
            IMongoCollection<Project> _projectCollection = new MongoDataAccess().GetMongoCollection<Project>(Constants.PITT_PROJECTS);

            FilterDefinition<Project> filter = Builders<Project>.Filter.Eq(x => x.id, id);
            UpdateDefinition<Project> update = Builders<Project>.Update.Set(x => x.externalStatus, status);

            UpdateResult result = await _projectCollection.UpdateOneAsync(filter, update);
        }

        public List<Project> GetAllActiveProjects()
        {
            IMongoCollection<Project> _projectCollection = new MongoDataAccess().GetMongoCollection<Project>(Constants.PITT_PROJECTS);

            IQueryable<Project> docs = _projectCollection.AsQueryable()
                .Where(x => x.isActive || x.lifeCycleStatus != LifeCycleStatus.DROPPED)
                .Where(x => x.updatedProjectECD != null);

            List<Project> projects = new List<Project>();
            projects.AddRange(docs.ToList());

            return projects;
        }

    }
}
