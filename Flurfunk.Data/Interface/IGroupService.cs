using Flurfunk.Data.Model;
using System;
using System.Collections.Generic;
namespace Flurfunk.Data.Interface
{
    public interface IGroupService
    {
        Group Create(string name);
        Group Get(string groupId);
        List<Group> Find(string groupName);        
        void Join(string groupId, string userId);
        void Leave(string groupId, string userId);
    }
}
