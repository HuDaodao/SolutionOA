using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
    public  class DepartmentData: BaseData
    {
        /// <summary>
        /// 得到某个部门下的所有员工
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public List<Employee> GetDeptEmp(int id)
        {
            return db.Employee.Where(n => n.DeptId == id).ToList();
        }

        /// <summary>
        /// 更新及插入
        /// </summary>
        /// <param name="dept"></param>
        public void Save(Dept dept)
        {
            if (dept.DeptId == 0)
            {
                db.Dept.InsertOnSubmit(dept);
            }
            db.SubmitChanges();
        }
        /// <summary>
        /// 获取第一级部门
        /// </summary>
        /// <returns></returns>
        public List<Dept> GetTopList()
        {
            return db.Dept.Where(n => n.ParentId == null).ToList();
        }
        /// <summary>
        /// 通过Id获取一个部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public Dept GetModel(int id)
        {
            return db.Dept.Single(n => n.DeptId == id);
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id">部门Id</param>
        public void Delete(int id)
        {
            Dept dept = GetModel(id);
            db.Dept.DeleteOnSubmit(dept);
            db.SubmitChanges();
        }
        /// <summary>
        /// 获取所有的部门
        /// </summary>
        /// <returns></returns>
        public List<Dept> GetAllDepart()
        {
            return db.Dept.ToList();
        }
        /// <summary>
        /// 可调整至的部门(排除自己及自己的后辈部门)
        /// </summary>
        /// <param name="id">当前部门ID</param>
        /// <returns></returns>
        public List<Dept> GetParentDept(int id)
        {
            List<int> arrs = new List<int>();
            List<int> arr=CanBeArr(arrs,id);
            List<Dept> canBeParentList;
                canBeParentList =( from p in db.Dept
                                  where !arr.Contains(p.DeptId)
                                  select p).ToList();
            return canBeParentList;
        }
        /// <summary>
        /// 将自己及后代部门的ID组成一个List
        /// </summary>
        /// <param name="arrs">id列表</param>
        /// <param name="id">当前ID</param>
        /// <returns></returns>
        public List<int> CanBeArr(List<int> arrs,int id)
        {
            arrs.Add(id);
            var infordept= GetModel(id);
            if (infordept.Dept2.ToList().Count() > 0)
            {
                foreach (var sonDept in infordept.Dept2.ToList())
                {
                    CanBeArr(arrs,sonDept.DeptId);
                }
            }
            return arrs;
        }
    }
}
