using MongoDB.Bson;
using System;
namespace Flurfunk.Data.Interface
{
    public interface IMessageService
    {
        Flurfunk.Data.Model.Message Create(string text, string creator);
        Flurfunk.Data.Model.Message Create(string text, string creator, string groupId);
        void Delete(string messageId);
        System.Collections.Generic.List<Flurfunk.Data.Model.Message> GetNewerThan(DateTime time, string keyword = "", string groupId ="");
        System.Collections.Generic.List<Flurfunk.Data.Model.Message> GetOlderThan(int count, DateTime time, string keyword = "", string groupId = "");
       
    }
}
