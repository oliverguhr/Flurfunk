using System;
namespace Flurfunk.Data.Interface
{
    interface IGroupService
    {
        Flurfunk.Data.Model.Group Create(string name);
        Flurfunk.Data.Model.Group Get(MongoDB.Bson.ObjectId groupId);
        System.Collections.Generic.List<Flurfunk.Data.Model.User> GetAllUsersThatAreNotInGroup(MongoDB.Bson.ObjectId groupId);
        void Join(MongoDB.Bson.ObjectId groupId, Flurfunk.Data.Model.User user);
        void Leave(MongoDB.Bson.ObjectId groupId, Flurfunk.Data.Model.User user);
    }
}
