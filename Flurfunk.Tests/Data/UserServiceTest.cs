using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flurfunk.Data;
using System.Configuration;
using Flurfunk.Data.Interface;
using Flurfunk.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace Flurfunk.Tests.Data
{
    [TestClass]
    public class UserServiceTest
    {
        IDatabase Db { get; set; }
        IUserService userService { get; set; }
        public UserServiceTest()
        {
            Db = new Database(ConfigurationManager.AppSettings["mongoDb"], ConfigurationManager.AppSettings["mongoDbName"]);
            userService = new UserService(Db);
        }

        ~UserServiceTest()
        {            
            Db.Users.RemoveAll();
        }

        [TestMethod]
        public void CreateNewUser()
        {            
            User newuser = userService.Create("Oliver", "testprovider", "0815");

            Assert.IsNotNull(newuser._id);

        }
        [TestMethod]
        [ExpectedException(typeof(ValidationException),"A username of null was inappropriately allowed.")]
        public void CreateNewUserWithoutName()
        {            
            User newuser = userService.Create(null, "testprovider", "0815");
            newuser = userService.Create("", "testprovider", "0815"); 
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException), "A username of 0 chars was inappropriately allowed.")]
        public void CreateNewUserWithZerolengthName()
        {                
            User newuser = userService.Create("", "testprovider", "0815");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException), "A username of 1 chars was inappropriately allowed.")]
        public void CreateNewUserWithOneCharName()
        {            
            User newuser = userService.Create("a", "testprovider", "0815");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException), "A long username was inappropriately allowed.")]
        public void CreateNewUserWithAToLongUsername()
        {            
            User newuser = userService.Create(new String('a', 45), "testprovider", "0815");            
        }
    }
}
