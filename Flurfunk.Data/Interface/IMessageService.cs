using MongoDB.Bson;
using System;
namespace Flurfunk.Data.Interface
{
    public interface IMessageService
    {
        Flurfunk.Data.Model.Message Create(string text, string creator);
        Flurfunk.Data.Model.Message Create(string text, string creator, string Group);
        void Delete(string messageId);
        System.Collections.Generic.List<Flurfunk.Data.Model.Message> GetNewerThan(DateTime time, string keyword = "");
        System.Collections.Generic.List<Flurfunk.Data.Model.Message> GetOlderThan(int count, DateTime time, string keyword = "");
        System.Collections.Generic.List<Flurfunk.Data.Model.Message> Get(int count, int startIndex);
        System.Collections.Generic.List<Flurfunk.Data.Model.Message> Get(int count, int startIndex, string keyword);
    }
}
