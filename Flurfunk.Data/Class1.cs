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

namespace Flurfunk.Data
{
    public class MessageService : IMessageService
    {
        private IDatabase db;

        public MessageService(IDatabase database)
        {
            db = database;
        }

        public Message Create(string text, string creator)
        {
            return Create(text, creator, ObjectId.Empty.ToString());
        }

        public Message Create(string text, string creator, string Group)
        {
            Message newMessage = new Message() { CreatorId = creator, Text = text, Group = Group, Created = DateTime.Now };
            newMessage.Validate();
            db.Messages.Insert(newMessage);
            return newMessage;
        }

        public void Delete(string messageId)
        {
            db.Messages.Remove(Query.EQ("_id", messageId));
        }

        public List<Message> GetNewerThan(DateTime time, string keyword = "")
        {
            IQueryable<Message> data = string.IsNullOrWhiteSpace(keyword) ? db.Messages.AsQueryable() : Filter(keyword).AsQueryable();

            var messages = data.Where(x => x.Created > time).OrderByDescending(x => x.Created);

            return LoadUsers(messages.ToList());
        }
        public List<Message> GetOlderThan(int count, DateTime time, string keyword = "")
        {
            IQueryable<Message> data = string.IsNullOrWhiteSpace(keyword) ? db.Messages.AsQueryable() : Filter(keyword).AsQueryable();
            
            var messages = data.Where(x => x.Created < time).OrderByDescending(x => x.Created).Take(count);

            return LoadUsers(messages.ToList());
        }

        [Obsolete]
        public List<Message> Get(int count, int startIndex)
        {
            return db.Messages.AsQueryable().OrderBy(x => x.Created).Skip(startIndex).Take(count).ToList();
        }

        [Obsolete]
        public List<Message> Get(int count, int startIndex, string keyword)
        {
            var textSearchCommand = new CommandDocument
                {
                    { "text", db.Messages.Name },
                    { "search", keyword }
                };
            var commandResult = db.MongoDatabase.RunCommand(textSearchCommand);
            var response = commandResult.Response["results"];

            List<Message> result = new List<Message>();

            foreach (var item in response.AsBsonArray)
            {
                result.Add(BsonSerializer.Deserialize<Message>(item["obj"].ToBsonDocument()));
            }

            return result.OrderBy(x => x.Created).Skip(startIndex).Take(count).ToList();
        }

        public List<Message> Filter(string keyword)
        {
            var textSearchCommand = new CommandDocument
                {
                    { "text", db.Messages.Name },
                    { "search", keyword }
                };
            var commandResult = db.MongoDatabase.RunCommand(textSearchCommand);
            var response = commandResult.Response["results"];

            List<Message> result = new List<Message>();

            foreach (var item in response.AsBsonArray)
            {
                result.Add(BsonSerializer.Deserialize<Message>(item["obj"].ToBsonDocument()));
            }

            return result;
        }

        private List<Message> LoadUsers(List<Message> messages)
        {
            var ids = messages.Select(x => x.CreatorId).Distinct();

            var users = db.Users.Find(Query<User>.In(x => x._id, ids));

            return messages.Join(users, message => message.CreatorId, user => user._id, (message, user) => { message.Creator = user; return message; }).ToList();
        }
    }

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

    public class GroupService : IGroupService
    {
        private IDatabase db;

        public GroupService(IDatabase database)
        {
            db = database;
        }

        public Group Create(string name)
        {
            Group newGroup = new Group();
            newGroup.Name = name;
            newGroup.Validate();

            return newGroup;
        }

        public void Join(string groupId, User user)
        {
            Group grouptToAdd = Get(groupId);
            user.Groups.Add(groupId, grouptToAdd.Name);
            db.Users.Save(user);
        }

        public void Leave(string groupId, User user)
        {
            user.Groups.Remove(groupId);
            db.Users.Save(user);
        }

        public Group Get(string groupId)
        {
            return db.Groups.FindOneById(groupId);
        }

        public List<User> GetAllUsersThatAreNotInGroup(string groupId)
        {
            return db.Users.AsQueryable().Where(x => !x.Groups.ContainsKey(groupId)).ToList();
        }
    }

    public class Database : Flurfunk.Data.Interface.IDatabase
    {
        private MongoClient mongoConnection;
        private MongoServer mongoServer;
        private MongoDatabase mongoDatabase;

        public MongoDatabase MongoDatabase
        {
            get { return mongoDatabase; }
        }

        public Database(string connectionString, string databaseName)
        {
            mongoConnection = new MongoClient(new MongoUrl(connectionString));
            mongoServer = mongoConnection.GetServer();
            mongoDatabase = mongoServer.GetDatabase(databaseName);

            Messages.EnsureIndex(new IndexKeysDocument("Text", "text"));
            Users.EnsureIndex("ProviderId");
        }

        public MongoCollection<User> Users { get { return mongoDatabase.GetCollection<User>("users"); } }

        public MongoCollection<Message> Messages { get { return mongoDatabase.GetCollection<Message>("messages"); } }

        public MongoCollection<Group> Groups { get { return mongoDatabase.GetCollection<Group>("groups"); } }
    }
}