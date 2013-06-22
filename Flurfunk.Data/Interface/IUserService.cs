using System;
namespace Flurfunk.Data.Interface
{
    public interface IUserService
    {
        void AddFilter(string keyword, string user);
        void RemoveFilter(string keyword, string userId);
        Flurfunk.Data.Model.User Create(string name, string providerName, string providerId);
        void Delete(string userId);
        Flurfunk.Data.Model.User Get(string userid);
        Flurfunk.Data.Model.User GetByName(string userName);
        Flurfunk.Data.Model.User GetByProviderId(string providerId);
        
    }
}
