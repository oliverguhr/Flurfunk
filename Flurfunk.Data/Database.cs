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