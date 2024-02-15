using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using MongoDB.Driver;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{
    public class StateModelDataContext
    {
        private IMongoCollection<StateModelAssociationProject> stateModelAssociationProjectsCollection;
        public StateModelDataContext()
        {
            stateModelAssociationProjectsCollection = new MongoDataAccess().GetMongoCollection<StateModelAssociationProject>(Constants.PITT_STATE_MODEL_ASSOCIATION_PROJECTS);
        }

        public List<StateModelAssociationProject> GetStateModelAssociationProjects()
        {
            return stateModelAssociationProjectsCollection.Find(_ => true).ToList();
        }

        public StateModelAssociationProject? GetStateModelAssociationProjectByProjectId(string projectId)
        {
            return stateModelAssociationProjectsCollection.Find(x => x.projectId == projectId).FirstOrDefault();
        }

        public async Task<StateModelAssociationProject> UpdateStateModelAssociationProjectById(StateModelAssociationProject smaProject)
        {
            IMongoCollection<StateModelAssociationProject> stateModelAssociationProjectsCollection
               = new MongoDataAccess().GetMongoCollection<StateModelAssociationProject>(Constants.PITT_STATE_MODEL_ASSOCIATION_PROJECTS);

            await stateModelAssociationProjectsCollection.ReplaceOneAsync(x => x.id == smaProject.id, smaProject);

            return smaProject;
        }

    }
}
