using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext.interfaces;
using System.Security.Cryptography;
using System.Text.Json;
using System.Configuration;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.DataContext
{
    public class UserDataContext: IUserDataContext
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string? _apiUrl;
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Process> _processCollection;

        public UserDataContext()
        {
            _apiUrl = Settings.WorkerServiceApiUrl;
            _usersCollection = new MongoDataAccess().GetMongoCollection<User>(Constants.PITT_USERS);
            _processCollection = new MongoDataAccess().GetMongoCollection<Process>(Constants.PITT_PROCESS);
        }

        public void SyncUserDataToMongo()
        {
            _log.Info("--> SyncUserDataToMongo");
            string emailIDs = GetUserEmailIDs();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string[] emails = emailIDs.Split(',');

                    foreach (string emailID in emails)
                    {
                        HttpResponseMessage response = client.GetAsync(_apiUrl + "/api/Employee/getUserByEmail?keyword=" + emailID).Result;
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        _log.Info(responseBody);
                        User user = null;

                        if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        {
                            DeleteUser(emailID);
                        }
                        else if (response.IsSuccessStatusCode && response != null)
                        {
                            user = JsonSerializer.Deserialize<User>(responseBody);
                            user = UpdateUserObject(user);

                            //Update the mongoDb collection
                            UpdateUserAsync(user);
                        }    
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            _log.Info("<-- SyncUserDataToMongo");
        }

        private string GetUserEmailIDs()
        {
            string emailIDs = "";
            _usersCollection.Find(_ => true).ToList().ForEach(user => {
                emailIDs = emailIDs + user.email + ",";
            });
            emailIDs = emailIDs.Substring(0, emailIDs.Length - 1);
            Console.WriteLine("User email IDs in MongoDB: " + emailIDs);
            return emailIDs;
        }

        private User UpdateUserObject(User user)
        {
            _log.Info("--> UpdateUserObject");
            // Get the list of roles that the user has.
            var directoryRoles = new HashSet<string>(user.roles ?? new List<string>());

            if (Settings.Roles == null)
                throw new ConfigurationErrorsException("Application roles have not been configured. Define in App Config section of App Settings.");

            // Get the list of application access roles - the roles that are of interest to the app.
            var appRoles = new HashSet<string>();
            var userRoles = Settings.Roles?.GetSection("UserRoles").GetChildren();
            user.isUser = false;
            // Add this to app roles list - the roles that are of interest to the app.
            if (userRoles?.Count() > 0)
            {
                foreach (var role in userRoles)
                    appRoles.Add(role.Value!);

                if (userRoles.Select(x => x.Value!).Intersect(directoryRoles).Count() > 0)
                    user.isUser = true;
            }

            // Get the super user role.
            var superUserRoles = Settings.Roles?.GetSection("SuperUserRoles").GetChildren();
            user.isSuperUser = false;
            // Add this to app roles list - the roles that are of interest to the app.
            if (superUserRoles?.Count() > 0)
            {
                foreach (var role in superUserRoles)
                    appRoles.Add(role.Value!);

                if (superUserRoles.Select(x => x.Value!).Intersect(directoryRoles).Count() > 0)
                    user.isSuperUser = true;
            }

            // Get the owner user role.
            var ownerRoles = Settings.Roles?.GetSection("OwnerRoles").GetChildren();
            user.isOwner = false;
            // Add this to app roles list - the roles that are of interest to the app.
            if (ownerRoles?.Count() > 0)
            {
                foreach (var role in ownerRoles)
                    appRoles.Add(role.Value!);

                if (ownerRoles.Select(x => x.Value!).Intersect(directoryRoles).Count() > 0)
                    user.isOwner = true;
            }

            // Get the system admin user role.
            var systemAdminRoles = Settings.Roles?.GetSection("SystemAdminRoles").GetChildren();
            user.isSystemAdmin = false;
            // Add this to app roles list - the roles that are of interest to the app.
            if (systemAdminRoles?.Count() > 0)
            {
                foreach (var role in systemAdminRoles)
                    appRoles.Add(role.Value!);

                if (systemAdminRoles.Select(x => x.Value!).Intersect(directoryRoles).Count() > 0)
                    user.isSystemAdmin = true;
            }

            // Get the business admin user role.
            var businessAdminRoles = Settings.Roles?.GetSection("BusinessAdminRoles").GetChildren();
            user.isBusinessAdmin = false;
            // Add this to app roles list - the roles that are of interest to the app.
            if (businessAdminRoles?.Count() > 0)
            {
                foreach (var role in businessAdminRoles)
                    appRoles.Add(role.Value!);

                if (businessAdminRoles.Select(x => x.Value!).Intersect(directoryRoles).Count() > 0)
                    user.isBusinessAdmin = true;
            }

            // Get the creator org roles.
            var creatorOrgRoles = Settings.Roles?.GetSection("CreatorOrgRoles").GetChildren();
            // Add this to app roles list - the roles that are of interest to the app.
            if (creatorOrgRoles?.Count() > 0)
            {
                foreach (var role in creatorOrgRoles)
                    appRoles.Add(role.Value!);
            }

            // Replace the user roles in the user object with user-application roles.
            var userAppRoles = appRoles.Intersect(directoryRoles).ToList();
            user.roles = userAppRoles.ToList();

            // Get the process roles.
            var processRoles = Settings.Roles?.GetSection("ProcessRoles").GetChildren();
            if (processRoles == null || processRoles.Count() == 0)
                throw new Exception("Process security roles have not been configured. Define in App Config section of App Settings.");

            var matchingProcessRoles = new HashSet<string>(processRoles.Select(x => x.Value!)).Intersect(directoryRoles).ToList();
            user.processRoles = GetUserProcessRoles(matchingProcessRoles, user.isSystemAdmin.Value);
            _log.Info("<-- UpdateUserObject");
            // Return the user object.
            return user;
        }

        private UserProcessRoles GetUserProcessRoles(List<string>? userRoles, bool isSystemAdmin)
        {
            _log.Info("--> GetUserProcessRoles");
            // Get all current processes.
            Processes processes = GetProcesses();
            UserProcessRoles allowedProcesses = new UserProcessRoles();

            // Compare user's entitlement roles to process list.
            foreach (Process p in processes.Where(x => x.pittEntitlements != null))
            {
                foreach (string e in p.pittEntitlements!)
                {
                    if (isSystemAdmin || (userRoles != null && userRoles.Count > 0 && userRoles.Contains(e)))
                    {
                        allowedProcesses.Add(new UserProcessRole() { process = p.name, role = e });
                        break;
                    }
                }
            }
            _log.Info("<-- GetUserProcessRoles");
            return allowedProcesses;
        }

        private Processes GetProcesses()
        {
            Processes processes = new Processes();
            processes.AddRange(_processCollection.Find(x => x.isActive == true).ToList());
            return processes;
        }

        private User UpdateUserAsync(User user)
        {
            _log.Info("--> UpdateUserAsync");
            var builder = Builders<User>.Filter;
            FilterDefinition<User> filter = builder.Eq(x => x.email, user.email);
            UpdateDefinition<User> update = Builders<User>.Update
                .Set(x => x.wwid, user.wwid)
                .Set(x => x.idsid, user.idsid)
                .Set(x => x.name, user.name)
                .Set(x => x.firstName, user.firstName)
                .Set(x => x.middleName, user.middleName)
                .Set(x => x.lastName, user.lastName)
                .Set(x => x.email, user.email)
                .Set(x => x.location, user.location)
                .Set(x => x.badgeType, user.badgeType)
                .Set(x => x.manager, user.manager)
                .Set(x => x.roles, user.roles)
                .Set(x => x.processRoles, user.processRoles)
                .Set(x => x.isUser, user.isUser)
                .Set(x => x.isSuperUser, user.isSuperUser)
                .Set(x => x.isOwner, user.isOwner)
                .Set(x => x.isSystemAdmin, user.isSystemAdmin)
                .Set(x => x.isBusinessAdmin, user.isBusinessAdmin);
            UpdateOptions options = new UpdateOptions { IsUpsert = true };

            User? existingUser = _usersCollection.Find(x => x.email == user.email).FirstOrDefault();

            UpdateResult result = _usersCollection.UpdateOne(filter, update, options);

            if (existingUser != null)
                user.id = existingUser.id;
            else
                user.id = result.UpsertedId.ToString();
            
            _log.Info("<-- UpdateUserAsync");
            return user;
        }

        private void DeleteUser(string emailID)
        {
            _log.Info("--> DeleteUser");
            var builder = Builders<User>.Filter;
            FilterDefinition<User> filter = builder.Eq(x => x.email, emailID);
            User? existingUser = _usersCollection.Find(x => x.email == emailID).FirstOrDefault();

            DeleteResult delResult= _usersCollection.DeleteOne(filter);
            if(delResult.DeletedCount!=1)
            {
                _log.Error("Failed to delete user info with email ID: "+emailID);
            }

            _log.Info("<-- DeleteUser");
        }
    }
}
