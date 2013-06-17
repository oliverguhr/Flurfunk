using System;
namespace Flurfunk.Data.Interface
{
    public interface IUserService
    {
        void AddFilter(string keyword, Flurfunk.Data.Model.User user);
        Flurfunk.Data.Model.User Create(string name, string providerName, string providerId);
        void Delete(MongoDB.Bson.ObjectId userId);
        Flurfunk.Data.Model.User Get(MongoDB.Bson.ObjectId userid);
        Flurfunk.Data.Model.User Get(string userName);
        Flurfunk.Data.Model.User GetByProviderId(string providerId);
        
    }
}
