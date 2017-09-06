using OA.Common;
using OA.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;

namespace OA.Web.Controllers
{
    public class ProjMemberController : CommonController
    {
        ProjectMemberData pm = new ProjectMemberData();
        ProjectData pd = new ProjectData();
        OaDataClassDataContext od = new OaDataClassDataContext();

        /// <summary>
        /// 返回视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //返回所有未完成的项目
           var list= pd.GetAllProject(null, 0);
            if (list.Count() == 0) return View("No");
            return View(list);
        }

        /// <summary>
        /// 项目人员人员列表(check?)
        /// </summary>
        /// <param name="projId">项目ID</param>
        /// <returns></returns>
        public JsonResult PMList(int projId)
        {
            var members = pm.GetMembers(projId);
            var strResult = "{\"members\":" + members.ToJsonString() + "}";
            return Json(strResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 移除项目人员人员
        /// </summary>
        /// <param name="Id">项目人员Id</param>
        public JsonResult DeleteMember(string Id)
        {
            //pm.DetailMember(Id);
            return Json("yes");
        }
        
        /// <summary>
        /// 返回所有的员工
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
        /// <param name="projId">项目ID</param>
        /// <param name="empId">员工ID</param>
        /// <returns></returns>
        public JsonResult addOneMember(int projId,int empId)
        {
            var member=od.ProjectMember.Where(n => n.ProjId == projId && n.EmpId == empId).ToList();
            if (member.Count!=0)
            {
                return Json("No");
            }
            ProjectMember pm = new ProjectMember();
            pm.ProjId = projId;
            pm.EmpId = empId;
            od.ProjectMember.InsertOnSubmit(pm);
            od.SubmitChanges();
            return Json("Ok");
        }
        /// <summary>
        /// 删除一个成员
        /// </summary>
        /// <param name="id">成员ID</param>
        /// <returns></returns>
        public JsonResult DeleteMember(int id)
        {
            pm.DeleteMember(id);
            return Json("OK");
        }
       
        ///// <summary>
        ///// 批量加入成员
        ///// </summary>
        ///// <param name="members">成员的id数组</param>
        ///// <param name="meetId">会议Id</param>
        ///// <returns></returns>
        //public JsonResult sureAdd(int[] members,int meetId)
        //{
        //    List<MeetMember> metMembers = new List<MeetMember>();
        //    if (members != null)
        //    {
        //        foreach(int empId in members)
        //        {
        //            var metmeb = md.GetOneMM(empId, meetId);
        //            if (metmeb.Count!=0)
        //            {
        //                continue;
        //            }
        //            MeetMember mm = new MeetMember();
        //            mm.EmpId = empId;
        //            mm.MeetId = meetId;
        //            mm.Joined = 0;
        //            metMembers.Add(mm);
        //        }
        //        md.SaveAllMM(metMembers);
        //    }
        //    return Json("Ok");
        //}
    }
}
