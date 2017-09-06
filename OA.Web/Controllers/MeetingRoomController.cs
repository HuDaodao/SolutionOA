using OA.Common;
using OA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OA.Web.Controllers
{
    public class MeetingRoomController : CommonController
    {
        MeetingRoomData db = new MeetingRoomData();
        /// <summary>
        /// 返回视图
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="page">当前页</param>
        /// <param name="searchMsg">会议室名称</param>
        /// <param name="IsValid">是否可用</param>
        /// <returns></returns>
        public ActionResult List_Json(int? id,int? page, string searchMsg, bool IsValid)
        {
            int pageIndex = page ?? 1;
            const int pageSize = 10;
            IEnumerable<MeetRoom> list = db.GetListByMe(pageIndex, pageSize, searchMsg, IsValid);
            int rowCount = db.GetListAll(searchMsg, IsValid).Count();
            var pageCount = 0;
            if (rowCount % pageSize != 0)
            {
                if (rowCount - pageSize < 0)
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
        /// 创建及修改获取会议室
        /// </summary>
        /// <param name="id">会议室ID</param>
        /// <returns></returns>
        public ActionResult CreateAndEditer(int? id)
        {
            MeetRoom emp = null;
            if (id != null)
            {
                emp = db.GetModel(id.Value);
            }
            if (emp == null)
                emp = new MeetRoom();
            return View(emp);
        }

        /// <summary>
        /// 保存会议室信息
        /// </summary>
        /// <param name="id">会议室ID</param>
        /// <param name="remark">备注</param>
        /// <param name="person">容纳人数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAndEditer(int? id,string name,string remark,int person)
        {
            MeetRoom currentRoom;
            if (id == 0||id==null)  { currentRoom = new MeetRoom(); }
            else { currentRoom = db.GetModel(int.Parse(id.ToString())); }
            currentRoom.RoomName = name;
            currentRoom.Person =person;
            currentRoom.Remark  =remark;
            currentRoom.IsValid = true;
            db.Save(currentRoom);
            return Json("OK");
        }

        /// <summary>
        /// 删除会议室
        /// </summary>
        /// <param name="Id">会议室Id</param>
        public JsonResult Delete(int Id)
        {
            db.Detail(Id);
            return Json("yes");
        }
        /// <summary>
        /// 设置为不可用
        /// </summary>
        /// <param name="id">会议室Id</param>
        /// <returns></returns>
        public ActionResult ValidChangeNo(int id)
        {
            db.ValidChangeNo(id);
            return View("List");
        }

        /// <summary>
        /// 设置为可用
        /// </summary>
        /// <param name="id">会议室Id</param>
        /// <returns></returns>
        public ActionResult ValidChangeYes(int id)
        {
            db.ValidChangeYes(id);
            return View("List");
        }
    }
}