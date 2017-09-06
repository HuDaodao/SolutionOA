using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace OA.Model
{
    public class MeetingData : BaseData
    {
        /// <summary>
        /// 获取会议信息
        /// </summary>
        /// <param name="meetingId">会议室ID</param>
        /// <returns></returns>
        public Meeting GetModel(int meetingId)
        {
            return db.Meeting.Single(n => n.MeetId == meetingId);
        }
        /// <summary>
        /// 获取与会人员的Name的string
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns></returns>
        public string GetMembers(int id)
        {
            var memberList=db.MeetMember.Where(n => n.MeetId == id).ToList();
            string memberName="";
            foreach(var member in memberList)
            {
                memberName += member.Employee.EmpName+"  ";
            }
            return memberName;
        }

        /// <summary>
        /// 保存会议信息的修改
        /// </summary>
        /// <param name="emp">会议实体</param>
        public void Save(Meeting meeting)
        {
            if (meeting.MeetId == 0)
                db.Meeting.InsertOnSubmit(meeting);
            db.SubmitChanges();
        }
       
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">会议id</param>
        public void Detail(int id)
        {
            Meeting  meeting = GetModel(id);
            DeleteAllOnSubmit(meeting.MeetMember);
            db.Meeting.DeleteOnSubmit(meeting);
            db.SubmitChanges();
        }

        /// <summary>
        /// 在删除这些会议成员
        /// </summary>
        /// <param name="mm">成员实体合集</param>
        public void DeleteAllOnSubmit(EntitySet<MeetMember> mm)
        {
            db.MeetMember.DeleteAllOnSubmit(mm);
        }
        /// <summary>
        /// 获取全部会议列表
        /// </summary>
        /// <param name="Subject">会议主题</param>
        /// <param name="Status">会议状态(2:已撤销 0：未开 1：已开)</param>
        /// <returns></returns>
        public List<Meeting> GetListAll(string Subject, int Status)
        {
            List<Meeting> meetAll;
            if (string.IsNullOrEmpty(Subject))
                meetAll = db.Meeting.Where(n=>n.Status == Status).OrderByDescending(n => n.MeetId).ToList();
            else
            {
                meetAll = db.Meeting.Where(n => n.Subject.Contains(Subject) &&n.Status==Status).OrderByDescending(n => n.MeetId).ToList();
            }
            return meetAll;
        }

        /// <summary>
        /// 分页获取列表 （多表查询）
        /// </summary>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="Subject">会议主题</param>
        /// <param name="Status">会议状态(2:已撤销 0：未开 1：已开)</param>
        /// <returns></returns>
        public List<Meeting> GetListByPage(int pageindex, int pageSize, string Subject, int Status)
        {
            List<Meeting> meeting;
            //meeting=from n in db.Meeting join b in db.Employee on n.
            if (!string.IsNullOrEmpty(Subject))
            {
                meeting = db.Meeting.Where(n => n.Subject.Contains(Subject) && n.Status == Status).OrderByDescending(n => n.MeetId).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                meeting = db.Meeting.Where(n => n.Status == Status).OrderByDescending(n => n.MeetId).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            return meeting;
        }


        /// <summary>
        /// 分页获取列表 
        /// </summary>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="Subject">会议主题</param>
        /// <param name="Status">会议状态(2:已撤销 0：未开 1：已开)</param>
        /// <returns></returns>
        //public List<> GetListByMe(int pageindex,int pageSize,string Subject, int Status)
        //{
            //模板
            //var q = from n in db.NewsModel
            //        join b in db.BigClassModel on n.BigClassID equals b.BigClassID
            //        join s in db.SmallClassModel on n.SmallClassID equals s.SmallClassID
            //        orderby n.AddTime descending
            //        select new
            //        {
            //            n.NewsID,
            //            n.BigClassID,
            //            n.SmallClassID,
            //            n.Title,
            //            b.BigClassName,
            //            s.SmallClassName,
            //        };
            //return q.ToList();

            //if (!string.IsNullOrEmpty(Subject))
            //{
            //    //meeting = db.Meeting.Where(n=>n.Subject.Contains(Subject) &&n.Status == Status).OrderByDescending(n=>n.MeetId).Skip((pageindex-1)* pageSize).Take(pageSize);
            //}
            //else
            //{
            //   var meeting = from n in db.Meeting
            //                     join b in db.Employee on n.EmId equals b.EmpId
            //                     join c in db.MeetRoom on n.RoomId equals c.RoomId
            //                     where n.Status==Status
            //                     orderby n.MeetId descending
            //                     select new
            //                     {
            //                         n.MeetId,
            //                         n.Subject,
            //                         b.EmpName,
            //                         c.RoomName,
            //                         n.StartDateTime,
            //                         n.Minutes,
            //                         n.Status,
            //                     };
            //    var meetingList = meeting.Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            //    return meetingList;
            //    //meeting = db.Meeting.Where(n=>n.Status == Status).OrderByDescending(n => n.MeetId).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            //}
        //}
        /// <summary>
        /// 设置开会状态
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <param name="status">要设置的状态</param>
        public void StatusChange(int id,int status)
        {
            Meeting meeting= GetModel(id);
            meeting.Status = status;
            db.SubmitChanges();
        }
    }
}
