using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OA.Web.Controllers
{
    public class HomeController : CommonController
    {
        //[AllowAnonymous]
        public ActionResult LogIn()
        {
            return View();
        }

        /// <summary>
        /// 返回登录页
        /// </summary>
        /// <param name="EmpUserName"></param>
        /// <param name="EmpPassWord"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogIn(string EmpUserName, string EmpPassWord)
        {
            EmployeeData db = new EmployeeData();
            Employee emp = db.GetModel(EmpUserName);
            if (emp == null)
            {
                ModelState.AddModelError("EmpUserName", "用户不存在");
            }
            else
            {
                if (emp.EmpPassWord != EmpPassWord)
                {
                    ModelState.AddModelError("EmpPassWord", "密码错误");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(emp.EmpId.ToString(), true, FormsAuthentication.FormsCookiePath);
                   Session["currentUser"]=emp.EmpId;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        //[Authorize]
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["MenuStr"] = menuData();
            return View();
        }
     public ActionResult TextAction()
        {
            return View();
        }
    }
}