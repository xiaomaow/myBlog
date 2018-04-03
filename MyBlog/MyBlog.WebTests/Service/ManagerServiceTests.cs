using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Web.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MyBlog.Web.Service.Tests
{
    [TestClass()]
    public class ManagerServiceTests
    {
        [TestMethod()]
        public void Update_PassWordTest()
        {
            try
            {
                ManagerService _service = new ManagerService();
                int result = _service.Update_PassWord(1, "123456");
                if (result <= 0)
                {
                    Assert.Fail();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();               
            }
        }
    }
}
