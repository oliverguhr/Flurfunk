using Flurfunk.Data.Interface;
using Flurfunk.Data.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flurfunk.Data
{
    public class UserService : IUserService
    {
        private IDatabase db;

        public UserService(IDatabase database)
        {
            db = database;
        }

        public User Create(string name, string providerName, string providerId)
        {
            User newUser = new User { Name = name, ProviderId = providerId, ProviderName = providerName };
            newUser.Validate();
            var test = newUser.GetValidationResults();

            db.Users.Insert(newUser);
            return newUser;
        }

        public void Delete(string userId)
        {
            db.Users.Remove(Query.EQ("_id", userId));
        }

        public void AddFilter(string keyword, string userId)
        {
            var user = Get(userId);
            user.Filter = user.Filter ?? new List<string>();
            user.Filter.Add(keyword);
            db.Users.Save(user);
        }

        public void RemoveFilter(string keyword, string userId)
        {
            var user = Get(userId);
            user.Filter.Remove(keyword);
            db.Users.Save(user);
        }

        public User Get(string userid)
        {
            return db.Users.FindOne(Query<User>.EQ(x => x._id, userid));
        }

        public User GetByProviderId(string providerId)
        {
            return db.Users.AsQueryable().SingleOrDefault(x => x.ProviderId == providerId);
        }

        public User GetByName(string userName)
        {
            return db.Users.AsQueryable().Where(user => user.Name == userName).SingleOrDefault();
        }
    }
}
