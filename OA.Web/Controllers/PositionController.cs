using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OA.Model;
using System.Text;
using OA.Common;

namespace OA.Web.Controllers
{
    public class PositionController : CommonController
    {
        PositionData db = new PositionData();
        /// <summary>
        /// 转到职位管理的首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ajax 获取出职位树
        /// </summary>
        /// <returns></returns>
        public ActionResult PositionTree()
        {
            List<Position> topPosition = db.GetTopList();
            var str = PingPositionTree(topPosition);
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 递归Ping 出职位列表
        /// </summary>
        /// <param name="position">职位列表</param>
        /// <returns></returns>
        public string PingPositionTree(List<Position> position)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var posit in position)
            {
                var sonList = posit.Position2.ToList();
                sb.AppendFormat("<li><span>{0}</span><a href='javascript:void(0);' onclick= 'EditClick({2})' class='myEdit' id={1}>编辑</a>", posit.PosName, posit.PositionId,posit.PositionId);
                if (sonList.Count() > 0)
                {
                    sb.Append(PingPositionTree(sonList));
                }
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        /// <summary>
        /// 得到部门信息传回JSon
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  ActionResult Edit(string id)
        {
            Position posit = db.GetModel(int.Parse(id));
            EmployeeData emdb = new EmployeeData();
            var OperUser = posit.OperUser == null ? null : emdb.GetModel(int.Parse(posit.OperUser)).EmpName;
            //List<Position> positions = db.GetParentPost(int.Parse(id));
            var strResult = "{\"OperUser\":\"" + OperUser + "\",\"posit\":" + posit.ToJsonString() + "}";
            return Json(strResult);
        }
        /// <summary>
        /// 保存职位信息
        /// </summary>
        /// <param name="positionId">职位ID</param>
        /// <param name="posName">职位名</param>
        /// <param name="parentId">父级职位</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateAndEditer(string positionId,string posName,string parentId,string remark)
        {
            Position posit;
            if (string.IsNullOrEmpty(positionId))
            { posit = new Position(); }
            else
            { posit = db.GetModel(int.Parse(positionId)); }
            posit.PosName = posName;
            if (string.IsNullOrEmpty(parentId))
            {
                posit.ParentId = null;
            }
            else
            {
                posit.ParentId = int.Parse(parentId);
            }
            posit.Remark = remark;
            posit.LastChangeTime = DateTime.Now;
            db.Save(posit);
            return Json("OK");
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeletePosition(string id)
        {
            int posId = int.Parse(id);
            Position posit = db.GetModel(posId);
            if (posit.Position2.ToList().Count()>0)
            {
                return Json("No");
            }
            else
            {
                List<Employee> emps = db.GetPositEmp(posId);
                foreach(Employee emp in emps)
                {
                    emp.PositionId = null;
                }
                db.Delete(posId);
                return Json("Yes");
            }
        }
        /// <summary>
        /// 获取当前职位可用的父职位 返回Json
        /// </summary>
        /// <param name="id">当前职位的ID</param>
        /// <returns></returns>
        public JsonResult DropDownParent(string id)
        {
            List<Position> positsList ;
            if (id == null || id == "" || id == "null")
            {
                positsList = db.GetAllPosition();
            }else
            {
                positsList = db.GetParentPosit(int.Parse(id));
            }
            var strResult = "{\"DropList\":" + positsList.ToJsonString() + "}";
            return Json(strResult, JsonRequestBehavior.AllowGet);
        }
    }
}