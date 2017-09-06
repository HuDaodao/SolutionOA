using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
    public  class RoleData: BaseData
    {
        /// <summary>
        /// 获取全部角色列表
        /// </summary>
        /// <returns></returns>
        public  List<Role> GetAllList()
        {
            return db.Role.OrderByDescending(n=>n.RoleId).ToList();
        }
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public  List<Role> GetPageList(int pageIndex,int pageSize)
        {
            List<Role> roles = db.Role.OrderByDescending(n => n.RoleId).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return roles;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">当前角色的ID </param>
        public void Delete(int id)
        {
            Role role = GetOneRole(id);
            //要先删除该角色的权限
            DeleteAllOnSubmit(role.RoleRight);
            var empRole = db.EmpRole.Where(n => n.RoleId == id);
            if(empRole!=null)
            db.EmpRole.DeleteAllOnSubmit(empRole);
            db.Role.DeleteOnSubmit(role);
            db.SubmitChanges();
        }
        /// <summary>
        /// 获得一个角色
        /// </summary>
        /// <param name="id">角色的ID</param>
        /// <returns></returns>
        public Role GetOneRole(int id)
        {
            return db.Role.Single(n => n.RoleId == id);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="role">一个角色实体</param>
        public void Save(Role role)
        {
            if (role.RoleId == 0)
            {
                db.Role.InsertOnSubmit(role);
            }
            db.SubmitChanges();
        }
        /// <summary>
        /// 删除这些权限
        /// </summary>
        /// <param name="rights">权限实体合集</param>
        public void DeleteAllOnSubmit(EntitySet<RoleRight> rights)
        {
            db.RoleRight.DeleteAllOnSubmit(rights);
        }

        /// <summary>
        ///获取这个角色的所有权限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        public List<RoleRight> GetRoleRight(int id)
        {
            return db.RoleRight.Where(n => n.RoleId == id).OrderByDescending(n => n.RightId).ToList();
        }


        /// <summary>
        /// 得到一个用户的所有菜单权限的Code
        /// </summary>
        /// <param name="empId">用户ID</param>
        /// <returns></returns>
        public  List<int> GetCodes(int empId)
        {
            List<int> roleId = (from x in db.EmpRole where x.EmpId == empId select x.RoleId).ToList();
            //得到此用户所有的权限Code
            List<int> rightCode = (from y in db.RoleRight where roleId.Contains(y.RoleId) select y.RightCode).ToList();
            return rightCode;
        }
    }
}
