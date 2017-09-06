using OA.BLL;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
   public class EmployeeData : BaseData
    {
        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <returns></returns>
        public Employee GetModel(int empId)
        {
            return db.Employee.Single(n => n.EmpId == empId);
        }

        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="empUserName">用户名</param>
        /// <returns></returns>
        public Employee GetModel(string empUserName)
        {
            try
            {
                return db.Employee.Single(n => n.EmpName == empUserName);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 保存员工信息的修改
        /// </summary>
        /// <param name="emp">员工实体</param>
        public void Save(Employee emp)
        {
            if (emp.EmpId == 0)
                db.Employee.InsertOnSubmit(emp);
            db.SubmitChanges();
        }
        /// <summary>
        /// 获取当前员工的所有角色
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        public List<EmpRole> empRoles(int id)
        {
            return db.EmpRole.Where(n => n.EmpId == id).ToList();
        }
        /// <summary>
        /// 删除这些角色
        /// </summary>
        /// <param name="rights">角色实体集合</param>
        public void DeleteAllOnSubmit(EntitySet<EmpRole> roles)
        {
            db.EmpRole.DeleteAllOnSubmit(roles);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        public void Detail(int id)
        {
            Employee emp = GetModel(id);
            db.Employee.DeleteOnSubmit(emp);
            db.SubmitChanges();
        }
        /// <summary>
        /// 获取全部员工列表
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetListAll(string empName,bool IsDimission)
        {
            
            List<Employee> empAll;
            bool Dimission = IsDimission.ToString() == null ? false : IsDimission;
            if (string.IsNullOrEmpty(empName))
                empAll = db.Employee.Where(n=>n.Dimission==Dimission).OrderByDescending(n => n.EmpId).ToList();
            else
            {
                empAll = db.Employee.Where(n => n.EmpName.Contains(empName)&&n.Dimission==Dimission).OrderByDescending(n => n.EmpId).ToList();
            }
            return empAll;
        }
        /// <summary>
        /// 分页获取列表 
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public List<Employee> GetListByMe(int pageindex,int pageSize,string empName,bool IsDimission)
        {
            List<Employee> emplist;
            bool Dimission = IsDimission.ToString() == null ? false : IsDimission;
            if (empName!=null)
            {
                 emplist = db.Employee.Where(n=>n.EmpName.Contains(empName)&&n.Dimission==Dimission).OrderByDescending(n=>n.EmpId).Skip((pageindex-1)* pageSize).Take(pageSize).ToList();
            }
            else
            {
                emplist = db.Employee.Where(n=>n.Dimission == Dimission).OrderByDescending(n => n.EmpId).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            foreach(var a in emplist)
            {
                if(!string.IsNullOrEmpty(a.DeptId.ToString()))
                a.DeptName = db.Dept.Single(n => n.DeptId == a.DeptId).DeptName;
                if(!string.IsNullOrEmpty(a.PositionId.ToString()))
                a.PosName = db.Position.Single(n => n.PositionId == a.PositionId).PosName;
            }
            return emplist;
        }
       
    }
}
