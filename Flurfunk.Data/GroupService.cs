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
    public class GroupService : IGroupService
    {
        private IDatabase db;
        private IUserService userService;

        public GroupService(IDatabase database, IUserService userService)
        {
            db = database;
            this.userService = userService;
        }

        public Group Create(string name)
        {
            if (db.Groups.AsQueryable().Any(x => x.Name == name))
            {
                throw new Exception("Lösung finden.");
            }

            Group newGroup = new Group();
            newGroup.Name = name;
            newGroup.Validate();

            db.Groups.Insert(newGroup);
            return newGroup;
        }

        public void Join(string groupId, string userId)
        {
            var user = userService.Get(userId);
            Group grouptToAdd = Get(groupId);
            user.Groups = user.Groups ?? new Dictionary<string, string>();
            if (!user.Groups.Any(x => x.Key == groupId))
            {
                user.Groups.Add(groupId, grouptToAdd.Name);
                db.Users.Save(user);
            }
        }

        public void Leave(string groupId, string userId)
        {
            var user = userService.Get(userId);
            user.Groups.Remove(groupId);
            db.Users.Save(user);
        }

        public Group Get(string groupId)
        {
            return db.Groups.FindOneById(ObjectId.Parse(groupId));
        }

        public List<User> GetAllUsersThatAreNotInGroup(string groupId)
        {
            //not needed yet
            return db.Users.AsQueryable().Where(x => !x.Groups.ContainsKey(groupId)).ToList();
        }

        public List<Group> Find(string groupName)
        {
            return db.Groups.AsQueryable().Where(x => x.Name.ToLower().Contains(groupName.ToLower())).OrderBy(x => x.Name).ToList();
        }
    }
}
