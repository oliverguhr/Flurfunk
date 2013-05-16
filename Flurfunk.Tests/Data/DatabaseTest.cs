using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flurfunk.Data;
using System.Configuration;
using Flurfunk.Data.Interface;

namespace Flurfunk.Tests.Data
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void ConnectToDatabase()
        {
            IDatabase db = new Database(ConfigurationManager.AppSettings["mongoDb"], ConfigurationManager.AppSettings["mongoDbName"]);
            Assert.IsNotNull(db);
        }
    }
}
