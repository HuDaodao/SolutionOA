using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
   public  class MeetMemberData:BaseData
    {
        //删除一个成员
        public void  Delete(int mmid)
        {
            MeetMember mm = GetModel(mmid);
            db.MeetMember.DeleteOnSubmit(mm);
            db.SubmitChanges();
        }
        /// <summary>
        /// 得到一个会议成员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MeetMember GetModel(int id)
        {
            return db.MeetMember.Single(n => n.MMId == id);
        }
        /// <summary>
        /// 修改出席状态
        /// </summary>
        /// <param name="id"></param>
        public void JoinChange(int id)
        {
            MeetMember mm = GetModel(id);
            if (mm.Joined == 0)
            { mm.Joined = 1; }
            else
            {mm.Joined = 0; }
            db.SubmitChanges();
        }
        /// <summary>
        /// 查询一个会议成员
        /// </summary>
        /// <param name="empId">员工Id</param>
        /// <param name="meetId">会议ID</param>
        /// <returns></returns>
        public List<MeetMember> GetOneMM(int empId,int meetId)
        {
            List<MeetMember> mm = db.MeetMember.Where(n => n.EmpId == empId && n.MeetId == meetId).ToList();
            return mm;
        }
        /// <summary>
        /// 保存所有会议成员信息
        /// </summary>
        /// <param name="mm"></param>
        public  void SaveAllMM(List<MeetMember> mm)
        {
            db.MeetMember.InsertAllOnSubmit(mm);
            db.SubmitChanges();
        }
        /// <summary>
        /// 返回一个员工的所有会议字符串
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <returns></returns>
        public List<int> MeetingStr(int empId)
        {
          List<int> arrs=new List<int>();
            List<MeetMember> mm = new List<MeetMember>();
            try
            {
                mm=db.MeetMember.Where(n => n.EmpId == empId).OrderBy(n => n.MMId).ToList();
                foreach(var m in mm)
                            {
                                arrs.Add(m.MeetId);
                            }
            }
            catch
            {
                return null;
            }
            return arrs;
        }
        /// <summary>
        /// 得到一个会议的全部成员的string
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns></returns>
        public string GetAllMembersStr(int id)
        {
            string membersStr = "";
            List<MeetMember> mm = db.MeetMember.Where(n => n.MeetId == id).ToList();
            foreach(var m in mm)
            {
                membersStr += m.Employee.EmpName+",";
            }
            membersStr = membersStr.Substring(0, membersStr.Length - 1);
            return membersStr;
        }
        /// <summary>
        /// 得到一个会议的全部成员
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <returns></returns>
        public List<MeetMember> GetAllMember(int id)
        {
            try
            {
                return db.MeetMember.Where(n => n.MeetId == id).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
