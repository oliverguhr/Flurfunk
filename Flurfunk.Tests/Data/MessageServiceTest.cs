using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Flurfunk.Data;
using Flurfunk.Data.Interface;
using Flurfunk.Data.Model;
using System.Linq;

namespace Flurfunk.Tests.Data
{
    [TestClass]
    public class MessageServiceTest
    {
        IDatabase Db { get; set; }
        IMessageService messageService { get; set; }
        IUserService userService { get; set; }
        public MessageServiceTest()
        {
            Db = new Database(ConfigurationManager.AppSettings["mongoDb"], ConfigurationManager.AppSettings["mongoDbName"]);
            messageService = new MessageService(Db);
            userService = new UserService(Db);
        }

        ~MessageServiceTest()
        {
            Db.Messages.RemoveAll();
            Db.Users.RemoveAll();
        }

        [TestMethod]
        public void FullTextSerach()
        {
            User newuser = userService.Create("Oliver", "testprovider", "0815");
            var testMessage =messageService.Create("Hello World", newuser._id);
            messageService.Create("None", newuser._id);
            messageService.Create("Lorem ipsum dolor sit amet, consetetur sadipscing elitr", newuser._id);

            var result = messageService.Get(10, 0, "World");

            Assert.IsNotNull(result.SingleOrDefault(x => x._id == testMessage._id));
        }
    }
}
