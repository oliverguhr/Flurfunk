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

        public Message Create(string text, string creator, string groupId)
        {
            Message newMessage = new Message() { CreatorId = creator, Text = text, GroupId = groupId, Created = DateTime.Now };
            newMessage.Validate();
            db.Messages.Insert(newMessage);
            return newMessage;
        }

        public void Delete(string messageId)
        {
            db.Messages.Remove(Query.EQ("_id", messageId));
        }

        public List<Message> GetNewerThan(DateTime time, string keyword = "", string groupId = "")
        {
            IQueryable<Message> data = string.IsNullOrWhiteSpace(keyword) ? db.Messages.AsQueryable() : Filter(keyword).AsQueryable();

            if (!string.IsNullOrWhiteSpace(groupId))
            {
                data = data.Where(x => x.GroupId == groupId);
            }
            else
            {
                data = data.Where(x => x.GroupId == ObjectId.Empty.ToString());
            }

            var messages = data.Where(x => x.Created > time).OrderByDescending(x => x.Created);

            return LoadUsers(messages.ToList());
        }
        public List<Message> GetOlderThan(int count, DateTime time, string keyword = "", string groupId = "")
        {
            IQueryable<Message> data = string.IsNullOrWhiteSpace(keyword) ? db.Messages.AsQueryable() : Filter(keyword).AsQueryable();

            if (!string.IsNullOrWhiteSpace(groupId))
            {
                data = data.Where(x => x.GroupId == groupId);
            }
            else
            {
                data = data.Where(x => x.GroupId == ObjectId.Empty.ToString());
            }

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
}
