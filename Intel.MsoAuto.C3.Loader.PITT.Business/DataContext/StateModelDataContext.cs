using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using MongoDB.Driver;
//using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Workflows;
using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{
    public class StateModelDataContext
    {
        public StateModelDataContext()
        {

        }

        public List<StateModelAssociationProject> GetStateModelAssociationProjects()
        {
            IMongoCollection<StateModelAssociationProject> stateModelAssociationProjectsCollection
                = new MongoDataAccess().GetMongoCollection<StateModelAssociationProject>(Constants.PITT_STATE_MODEL_ASSOCIATION_PROJECTS);

            return stateModelAssociationProjectsCollection.Find(_ => true).ToList();
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
