using OA.Common;
using OA.Model;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;

namespace OA.Web.Controllers
{
    public class RoleController : CommonController
    {
        RoleData db = new RoleData();
        /// <summary>
        /// 返回视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            InitEdit();
            return View();
        }

        // GET: Role
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page">当前页</param>
        /// <returns></returns>
        public ActionResult PageList(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = 10;
            List<Role> roles = db.GetPageList(pageIndex ,pageSize);
            int rowCount = db.GetAllList().Count();
            var pageCount = 0;
            if (rowCount % pageSize != 0)
            {
                if (rowCount - pageSize < 0)
                {
                    pageCount = 1;
                }else
                {
                    pageCount = rowCount / pageSize + 1;
                }
            }
            else
            {
                pageCount = rowCount / pageSize;
            }
            //ViewData["pageCount"] = pageCount;
            //ViewData["currentPage"] = page==null?1:page;
            var strResult = "{\"pageCount\":" + pageCount + ",\"currentPage\":" + pageIndex + ",\"list\":" + roles.ToJsonString() + "}";
            return Json(strResult,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除一个角色
        /// </summary>
        /// <param name="id">要删除的角色的ID</param>
        /// <returns></returns>
        public JsonResult DeleteRole(string  id)
        {
            db.Delete(int.Parse(id));
            return Json(id);
        }

        /// <summary>
        /// 点击编辑得到一个部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            Role role = db.GetOneRole(int.Parse(id));
            List<RoleRight> roleRight = db.GetRoleRight(int.Parse(id));
            var str = "{\"role\":" + role.ToJsonString() + ",\"roleRight\":"+roleRight.ToJsonString()+"}";
            return Json(str);
        }
        /// <summary>
        /// 提交处理
        /// </summary>
        /// <param name="RoletId">角色Id</param>
        /// <param name="Name">角色名</param>
        /// <param name="Remark">备注</param>
        /// <param name="Right">权利Id的数组</param>
        /// <returns></returns>
        public ActionResult  CreateEdit(string RoleId, string Name,string Remark,int[] Right)
        {
            Role role;

            EntitySet<RoleRight> rights = new EntitySet<RoleRight>();
            if (Right != null)
            {
              foreach(int code in Right)
                  {
                       RoleRight rr = new RoleRight();
                        rr.RightCode = code;
                       rights.Add(rr);
                  }
            }
          
            if (string.IsNullOrEmpty(RoleId))
            {
                role = new Role();
            }
            else
            {
                role = db.GetOneRole(int.Parse(RoleId));
            }
            db.DeleteAllOnSubmit(role.RoleRight);
            role.RoleRight = rights;
            role.Name = Name;
            role.Remark = Remark;
            db.Save(role);
            return Json("OK");
        }
        /// <summary>
        /// 得到权限菜单
        /// </summary>
        public void InitEdit()
        {
            ViewData["Right"]= MenuHelper.RightMenu();
        }
    }
}