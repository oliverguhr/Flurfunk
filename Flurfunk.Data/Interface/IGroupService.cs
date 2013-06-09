using System;
namespace Flurfunk.Data.Interface
{
    interface IGroupService
    {
        Flurfunk.Data.Model.Group Create(string name);
        Flurfunk.Data.Model.Group Get(string groupId);
        System.Collections.Generic.List<Flurfunk.Data.Model.User> GetAllUsersThatAreNotInGroup(string groupId);
        void Join(string groupId, Flurfunk.Data.Model.User user);
        void Leave(string groupId, Flurfunk.Data.Model.User user);
    }
}
