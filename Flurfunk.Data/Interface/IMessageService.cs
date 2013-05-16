using MongoDB.Bson;
using System;
namespace Flurfunk.Data.Interface
{
    public interface IMessageService
    {
        Flurfunk.Data.Model.Message Create(string text, MongoDB.Bson.ObjectId creator);
        Flurfunk.Data.Model.Message Create(string text, MongoDB.Bson.ObjectId creator, MongoDB.Bson.ObjectId Group);
        void Delete(MongoDB.Bson.ObjectId messageId);
        System.Collections.Generic.List<Flurfunk.Data.Model.Message> Get(int count, int startIndex);
        System.Collections.Generic.List<Flurfunk.Data.Model.Message> Get(int count, int startIndex, string keyword);
    }
}
