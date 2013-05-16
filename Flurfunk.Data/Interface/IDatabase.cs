using MongoDB.Driver;
using System;
namespace Flurfunk.Data.Interface
{
    public interface IDatabase
    {
        MongoDatabase MongoDatabase { get; }
        MongoDB.Driver.MongoCollection<Flurfunk.Data.Model.Group> Groups { get; }
        MongoDB.Driver.MongoCollection<Flurfunk.Data.Model.Message> Messages { get; }
        MongoDB.Driver.MongoCollection<Flurfunk.Data.Model.User> Users { get; }
    }
}
