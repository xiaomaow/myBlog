using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Web.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBlog.Web.Models;


namespace MyBlog.Web.Service.Tests
{
    [TestClass()]
    public class ManagerServiceTests
    {
        [TestMethod()]
        public void Update_PassWordTest()
        {
            //try
            //{
            //    ManagerService _service = new ManagerService();
            //    int result = _service.Update_PassWord(1, "123456");
            //    if (result <= 0)
            //    {
            //        Assert.Fail();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Assert.Fail();
            //}
        }

        [TestMethod()]
        public void admin_listTest()
        {
            //try
            //{
            //    ManagerService _service = new ManagerService();
            //    PageInfo _page_info = new PageInfo();
            //    _page_info.page = 0;
            //    _page_info.page_size = 20;
            //    List<admin> _admin_list = _service.admin_list(_page_info);
            //    if (_admin_list.Count == 0)
            //    {

            //        Assert.Fail();
            //    }
            //    Console.WriteLine(string.Format("查询到{0}行数据", _admin_list.Count()));
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Assert.Fail();
            //}

        }

        [TestMethod()]
        public void LoginTest()
        {
            try
            {
                ManagerService _serivice = new ManagerService();
                admin _admin = _serivice.Login("xiaomaow", "123456");
                if (_admin == null)
                {
                    Console.WriteLine("登录失败");
                }
                else
                {
                    Console.WriteLine("登录成功");
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
