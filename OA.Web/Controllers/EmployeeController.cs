using OA.Common;
using OA.Model;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;

namespace OA.Web.Controllers
{
    public class EmployeeController : CommonController
    {
        // GET: Employee
        EmployeeData db = new EmployeeData();
        /// <summary>
        /// 返回视图
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            //var a = CurrentUser.EmpName;
            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="page">当前页</param>
        /// <param name="searchMsg">搜索信息（员工名）</param>
        /// <returns></returns>
        public ActionResult List_Json(int? id, int? page, string searchMsg,bool IsDimission)
        {
            int pageIndex = page ?? 1;
            const int pageSize = 10;
            IEnumerable<Employee> list = db.GetListByMe(pageIndex, pageSize,searchMsg,IsDimission);
            int rowCount = db.GetListAll(searchMsg,IsDimission).Count();
            var pageCount = 0;
            if (rowCount % pageSize != 0)
            {
                if (rowCount - pageSize <0)
                {
                    pageCount = 1;
                }
                else
                {
                    pageCount = rowCount / pageSize + 1;
                }
            }
            else
            {
               pageCount = rowCount / pageSize;
            }
            var strResult = "{\"pageCount\":" + pageCount + ",\"CurrentPage\":" + pageIndex + ",\"list\":" + list.ToJsonString() + "}";
            return Json(strResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建及修改获取员工
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        public ActionResult CreateAndEditer(int? id)
        {
            Employee emp = null;
            string inRolestr="";
            if (id != null)
            {
                emp = db.GetModel(id.Value);
                inRolestr = roleIdStr(int.Parse(id.ToString()));
            }
            if (emp == null)
                emp = new Employee();
           DropDown(emp.DeptId,emp.PositionId);
            ViewData["InRole"] = inRolestr;
            RoleData rdb = new RoleData();
            ViewData["Roles"] = rdb.GetAllList();
            return View(emp);
        }
        /// <summary>
        /// 拥有角色的字符串
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        public string roleIdStr(int id)
        {
            string inRolestr = "";
            List<EmpRole> roles = db.empRoles(int.Parse(id.ToString()));
            if (roles.Count() != 0)
            {
                foreach (var role in roles)
                {
                    inRolestr += role.RoleId.ToString() + ",";
                }
                inRolestr = inRolestr.Substring(0, inRolestr.Length - 1);
            }
            return inRolestr;
        }

        /// <summary>
        /// 保存员工信息
        /// </summary>
        /// <param name="id">个人ID</param>
        /// <param name="emp">员工Model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAndEditer(int? id,Employee emp)
        {
            Employee currentEmp;
            if (emp.EmpId == 0)  { currentEmp = new Employee(); }
            else { currentEmp = db.GetModel(emp.EmpId); }
            currentEmp.EmpName = emp.EmpName;
            currentEmp.EmpUserName = emp.EmpUserName;
            currentEmp.Sex = Request["Sex"] == "1" ? true : false;
            currentEmp.Dimission = Request["Dimission"] == "1" ? true : false;
            currentEmp.Email = emp.Email ?? "";
            currentEmp.EmpPassWord = emp.EmpPassWord;
            currentEmp.Admin = Request["Admin"] == "1" ? true : false;
            currentEmp.Mobile = emp.Mobile??"";
            currentEmp.DeptId = emp.DeptId;
            currentEmp.PositionId = emp.PositionId;
            db.Save(currentEmp);
            DropDown(null,null);
            RoleData rdb = new RoleData();
            ViewData["Roles"] = rdb.GetAllList();
            var str= "{\"emp\":" + currentEmp.ToJsonString() +  "}";
            ViewData["InRole"] = "";
            return Json(str);
        }
        /// <summary>
        /// 保存该员工的角色信息
        /// </summary>
        /// <param name="Id">要设置角色的员工的ID</param>
        /// <param name="RoleCodes">角色的代码字符串</param>
        /// <returns></returns>
        public JsonResult SaveEmpRole(int[] RoleCodes,int Id)
        {
            EntitySet<EmpRole> empRole = new EntitySet<EmpRole>();
            if (RoleCodes != null)
            {
                foreach(int roleCode in RoleCodes)
                {
                    EmpRole er = new EmpRole();
                    er.RoleId = roleCode;
                    empRole.Add(er);
                }
            }
            Employee emp = db.GetModel(Id);
            db.DeleteAllOnSubmit(emp.EmpRole);
            emp.EmpRole = empRole;
            db.Save(emp);
            return Json("OK");
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="Id"></param>
        public JsonResult Delete(int Id)
        {
            db.Detail(Id);
            return Json("yes");
        }

        /// <summary>
        /// 绑定下拉列表的值
        /// </summary>
        private  void DropDown(int? dept, int? position)
        {
            DepartmentData deptdb = new DepartmentData();
            ViewData["DeptId"] = new SelectList(deptdb.GetAllDepart(), "DeptId", "DeptName",dept);
            PositionData positiondb = new PositionData();
            ViewData["PositionId"] = new SelectList(positiondb.GetAllPosition(), "PositionId", "PosName",position);
        }
    }
}