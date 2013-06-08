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

        public Message Create(string text, ObjectId creator)
        {
            return Create(text, creator, ObjectId.Empty);
        }

        public Message Create(string text, ObjectId creator, ObjectId Group)
        {
            Message newMessage = new Message() { Creator = creator, Text = text, Group = Group, Created = DateTime.Now };
            newMessage.Validate();
            db.Messages.Insert(newMessage);
            return newMessage;
        }

        public void Delete(ObjectId messageId)
        {
            db.Messages.Remove(Query.EQ("_id", messageId));
        }

        public List<Message> Get(int count, int startIndex)
        {
            return db.Messages.AsQueryable().OrderBy(x => x.Created).Skip(startIndex).Take(count).ToList();
        }

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

        public void Delete(ObjectId userId)
        {
            db.Users.Remove(Query.EQ("_id", userId));
        }

        public void AddFilter(string keyword, User user)
        {
            user.Filter.Add(keyword);
            db.Users.Save(user);
        }

        public User Get(ObjectId userid)
        {
            return db.Users.FindOneById(userid);
        }

        public User Get(string userName)
        {            
            return db.Users.AsQueryable().Where(user => user.Name == userName).Single();
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

        public void Join(ObjectId groupId, User user)
        {
            Group grouptToAdd = Get(groupId);
            user.Groups.Add(groupId, grouptToAdd.Name);
            db.Users.Save(user);
        }

        public void Leave(ObjectId groupId, User user)
        {
            user.Groups.Remove(groupId);
            db.Users.Save(user);
        }

        public Group Get(ObjectId groupId)
        {
            return db.Groups.FindOneById(groupId);
        }

        public List<User> GetAllUsersThatAreNotInGroup(ObjectId groupId)
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
        }

        public MongoCollection<User> Users { get { return mongoDatabase.GetCollection<User>("users"); } }

        public MongoCollection<Message> Messages { get { return mongoDatabase.GetCollection<Message>("messages"); } }

        public MongoCollection<Group> Groups { get { return mongoDatabase.GetCollection<Group>("groups"); } }
    }
}