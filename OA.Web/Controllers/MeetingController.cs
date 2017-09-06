using OA.Common;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;

namespace OA.Web.Controllers
{
    public class MeetingController : CommonController
    {
        MeetingData db = new MeetingData();
        OaDataClassDataContext od = new OaDataClassDataContext();
        MeetMemberData md = new MeetMemberData();

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
        /// <param name="searchMsg">会议主题</param>
        /// <param name="Status">会议状态</param>
        /// <returns></returns>
        public ActionResult List_Json(int? id, int? page, string searchMsg, int Status)
        {
            int pageIndex = page ?? 1;
            const int pageSize = 10;
          
            //未多表查询的方法
            //IEnumerable<Meeting> list = db.GetListByMe(pageIndex, pageSize, searchMsg, Status);

            int rowCount = db.GetListAll(searchMsg, Status).Count();
            var pageCount = 1;
            if (rowCount != 0)
            {
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
            }
            //====多表查询star====
            if (string.IsNullOrEmpty(searchMsg))
            {
                var meeting = from n in od.Meeting
                              join b in od.Employee on n.EmId equals b.EmpId
                              join c in od.MeetRoom on n.RoomId equals c.RoomId
                              where n.Status == Status
                              orderby n.MeetId descending
                              select new
                              {
                                  n.MeetId,
                                  n.Subject,
                                  b.EmpName,
                                  c.RoomName,
                                  n.StartDateTime,
                                  n.Minutes,
                                  n.Status,
                              };
                var meetingList = meeting.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var strResult = "{\"pageCount\":" + pageCount + ",\"CurrentPage\":" + pageIndex + ",\"list\":" + meetingList.ToJsonString() + "}";
                return Json(strResult, JsonRequestBehavior.AllowGet);
                //meeting = db.Meeting.Where(n=>n.Status == Status).OrderByDescending(n => n.MeetId).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                var meeting = from n in od.Meeting
                              join b in od.Employee on n.EmId equals b.EmpId
                              join c in od.MeetRoom on n.RoomId equals c.RoomId
                              where n.Status == Status&&n.Subject.Contains(searchMsg)
                              orderby n.MeetId descending
                              select new
                              {
                                  n.MeetId,
                                  n.Subject,
                                  b.EmpName,
                                  c.RoomName,
                                  n.StartDateTime,
                                  n.Minutes,
                                  n.Status,
                              };
                var meetingList = meeting.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var strResult = "{\"pageCount\":" + pageCount + ",\"CurrentPage\":" + pageIndex + ",\"list\":" + meetingList.ToJsonString() + "}";
                return Json(strResult, JsonRequestBehavior.AllowGet);
                //meeting = db.Meeting.Where(n=>n.Subject.Contains(Subject) &&n.Status == Status).OrderByDescending(n=>n.MeetId).Skip((pageindex-1)* pageSize).Take(pageSize);
            }
            //=========多表查询End====

            //以前总的return
            //var strResult = "{\"pageCount\":" + pageCount + ",\"CurrentPage\":" + pageIndex + ",\"list\":" + list.ToJsonString() + "}";
            //return Json(strResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建及修改获取会议（获取与会人员）
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns></returns>
        public ActionResult CreateAndEditer(int? id)
        {
            Meeting meeting = null;
            if (id != null)
            {
                meeting = db.GetModel(id.Value);
            }
            if (meeting == null)
                meeting = new Meeting ();
            MeetingRoomData md = new MeetingRoomData();
            ViewData["RoomId"] =new SelectList(md.GetListAll(string.Empty, true),"RoomId","RoomName",meeting.RoomId);
            ViewData["Members"] =id==null?"": db.GetMembers(int.Parse(id.ToString()));
            ViewData["Remark"] = meeting.Remark;
            return View(meeting);
        }
        /// <summary>
        /// 修改会议状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult StatusChange(int id, int status)
        {
            Meeting meet=db.GetModel(id);
            meet.Status = status;
            db.Save(meet);
            return Json("Ok");
        }

        /// <summary>
        /// 保存会议信息
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <param name="subject">会议主题</param>
        /// <param name="remark">备注</param>
        /// <param name="startTime">会议开始时间</param>
        /// <param name="Minutes">会议用时</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAndEditer(string id,string subject,string remark, string roomId,DateTime startTime, int Minutes)
        {
            Meeting currentMeeting;
            if (string.IsNullOrEmpty(id)||id=="0")
            {
                currentMeeting = new Meeting();
                currentMeeting.Status = 0;
            }
            else
            {
                currentMeeting = db.GetModel(int.Parse(id.ToString()));
            }
            currentMeeting.Subject = subject;
            currentMeeting.Remark = remark;
            currentMeeting.StartDateTime  =startTime;
            currentMeeting.Minutes = Minutes;

            currentMeeting.RoomId = int.Parse(roomId);
            currentMeeting.CreatTime = DateTime.Now;


            //当前用户
            //var a= HttpContext.Current.User.Identity.Name;
            currentMeeting.EmId =1 ;
            db.Save(currentMeeting);
            return Json(currentMeeting.MeetId);
        }
        /// <summary>
        /// 选取与会人员（获取所有部门，选择部门下拉列表，获得该部门人员。获取未离职的）
        /// </summary>
        /// <param name="id">会议Id</param>
        /// <returns></returns>
        public JsonResult GetMeetingMember(int id)
        {
            return null;
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
        public ActionResult ValidChangeNo(int id,int status)
        {
            db.StatusChange(id,status);
            return View("List");
        }
        /// <summary>
        ///转到与会人员列表页面
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns></returns>
        public ActionResult MeetingMember(int id)
        {
            ViewData["MeetId"] = id;
            return View();
        }
        /// <summary>
        /// 与会人员列表
        /// </summary>
        /// <param name="Joined"></param>
        /// <param name="meetId"></param>
        /// <returns></returns>
        public JsonResult MMList(int Joined,int meetId)
        {
            
            var MMList =from n in od.MeetMember
                                join b in od.Employee on n.EmpId equals b.EmpId
                                where n.Joined == Joined &&n.MeetId==meetId
                                orderby n.MMId descending
                                select new
                                {
                                    n.MMId,
                                    n.Joined,
                                    b.EmpName,
                                };
            if (MMList == null) return Json(null);
            var strResult = "{\"list\":" + MMList.ToList().ToJsonString() + "}";
            return Json(strResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回所的员工
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllEmp()
        {
            var empNoCheck = from n in od.Employee
                             join b in od.Dept on n.DeptId equals b.DeptId
                             where n.Dimission==false
                             orderby n.DeptId descending
                             select new
                             {
                                 n.EmpId,
                                 n.EmpName,
                                 b.DeptName,
                             };
           var empAll = empNoCheck.ToList();
            var strResult = "{\"AllEmps\":" + empAll.ToJsonString() + "}";
            return Json(strResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加一个成员
        /// </summary>
        /// <param name="meetId"></param>
        /// <param name="empId"></param>
        /// <returns></returns>
        public JsonResult addOneMember(int meetId,int empId)
        {
            var member=od.MeetMember.Where(n => n.MeetId == meetId && n.EmpId == empId).ToList();
            if (member.Count!=0)
            {
                return Json("No");
            }
            MeetMember mm = new MeetMember();
            mm.MeetId = meetId;
            mm.EmpId = empId;
            mm.Joined = 0;
            od.MeetMember.InsertOnSubmit(mm);
            od.SubmitChanges();
            return Json("Ok");
        }
        /// <summary>
        /// 删除一个成员
        /// </summary>
        /// <param name="id">MeetMemberId</param>
        /// <returns></returns>
        public JsonResult DeleteMember(int id)
        {
            md.Delete(id);
            return Json("OK");
        }
        /// <summary>
        /// 改变出席状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult JoinChange(int id)
        {
            md.JoinChange(id);
            return Json("ok");
        }
        /// <summary>
        /// 批量加入成员
        /// </summary>
        /// <param name="members">成员的id数组</param>
        /// <param name="meetId">会议Id</param>
        /// <returns></returns>
        public JsonResult sureAdd(int[] members,int meetId)
        {
            List<MeetMember> metMembers = new List<MeetMember>();
            if (members != null)
            {
                foreach(int empId in members)
                {
                    var metmeb = md.GetOneMM(empId, meetId);
                    if (metmeb.Count!=0)
                    {
                        continue;
                    }
                    MeetMember mm = new MeetMember();
                    mm.EmpId = empId;
                    mm.MeetId = meetId;
                    mm.Joined = 0;
                    metMembers.Add(mm);
                }
                md.SaveAllMM(metMembers);
            }
            return Json("Ok");
        }
    }
}
