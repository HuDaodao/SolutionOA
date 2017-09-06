using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OA.Model;

namespace OA.Web.Controllers
{
    public class PersonalInformationController : CommonController
    {
        EmployeeData db = new EmployeeData();
        /// <summary>
        /// 返回个人信息页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Employee currentTemp = db.GetModel(2);
            return View(currentTemp);
        }
        /// <summary>
        ///保存个人信息
        /// </summary>
        /// <param name="id">个人ID</param>
        /// <param name="emp">员工Model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string Id, string EmpName,string EmpUserName,string Email,string EmpPassWord,string Mobile)
        {
            var employee = db.GetModel(int.Parse(Id));
            employee.EmpName = EmpName;
            employee.EmpUserName = EmpUserName;
            employee.Email = Email;
            employee.EmpPassWord = EmpPassWord;
            employee.Mobile = Mobile;
            employee.Sex = Request["Sex"] == "1" ? true : false;
            db.Save(employee);
            return View(employee);
        }
    }
}