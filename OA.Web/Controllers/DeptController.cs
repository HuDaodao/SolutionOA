using OA.Common;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OA.Web.Controllers
{
    public class DeptController : CommonController
    {
        DepartmentData db = new DepartmentData();
        // GET: Dept
        /// <summary>
        /// 转到部门首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ajax 获取部门列表(树）
        /// </summary>
        /// <returns></returns>
        public ActionResult DeptMenu()
        {
            List<Dept> topDept = db.GetTopList();
            var str = PingDeptMenu(topDept);
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 递归ping 出部门的树
        /// </summary>
        /// <param name="Depts">部门列表</param>
        /// <returns></returns>
        public string PingDeptMenu(List<Dept> Depts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var Dept in Depts)
            {
                var sonList = Dept.Dept2.ToList();
                sb.AppendFormat("<li><span>{0}</span><a href='javascript:void(0);' onclick= 'EditClick({2})' class='myEdit' id={1} >编辑</a>", Dept.DeptName, Dept.DeptId,Dept.DeptId);
                if (sonList.Count() > 0)
                {
                    sb.Append(PingDeptMenu(sonList));
                }
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        /// <summary>
        /// 点击编辑转到的Action 得到该部门的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            Dept dept = db.GetModel(int.Parse(id));
            EmployeeData emdb = new EmployeeData();
            var OperUser = dept.OperUser == null ? null : emdb.GetModel(int.Parse(dept.OperUser)).EmpName;
            List<Dept> Dropdepts = db.GetParentDept(int.Parse(id));
            var strResult = "{\"OperUser\":\"" + OperUser + "\",\"dept\":" + dept.ToJsonString() + "}";
            return Json(strResult);
        }

        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <param name="DeptId">部门ID</param>
        /// <param name="DeptName">部门名字</param>
        /// <param name="ParentId">父级部门</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateAndEditer(string DeptId, string DeptName, string ParentId, string Remark)
        {
            Dept dept;
            if (string.IsNullOrEmpty(DeptId))
            { dept = new Dept(); }
            else
            { dept = db.GetModel(int.Parse(DeptId)); }
            dept.DeptName = DeptName;
            if (string.IsNullOrEmpty(ParentId))
            {
                dept.ParentId = null;
            }else
            {
            dept.ParentId =int.Parse(ParentId);
            }
            dept.Remark = Remark;
            dept.LastChangeTime = DateTime.Now;
            db.Save(dept);
            return Json("OK");
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public JsonResult DeleteDept(string id)
        {
            int deptId = int.Parse(id);
            Dept dept = db.GetModel(deptId);
            if (dept.Dept2.ToList().Count() > 0)
            {
                return Json("No");
            }
            List<Employee> emps = db.GetDeptEmp(deptId);
            foreach(Employee emp in emps)
            {
                emp.DeptId = null;
            }
            db.Delete(deptId);
            return Json("Yes");
        }

        /// <summary>
        ///获取当前部门的可用父级部门
        /// </summary>
        /// <param name="id">当前部门的ID</param>
        /// <returns>JSon数据</returns>
        public JsonResult DropDownParent(string id)
        {
            List<Dept> dropList;
            if (id==null||id==""||id=="null")
            {
                dropList = db.GetAllDepart();
            }
            else
            {
                dropList = db.GetParentDept(int.Parse(id));
            }
            var strResult = "{\"DropList\":" + dropList.ToJsonString() + "}";
            return Json(strResult, JsonRequestBehavior.AllowGet);
        }
    }
}