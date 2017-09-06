using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OA.Model;
using System.Web.Mvc;
using System.Text;
using OA.Common;

namespace OA.Web.Controllers
{
    public class PlanController : CommonController
    {
        #region 计划
        PlanData dbp = new PlanData();
        /// <summary>
        /// 转到计划首页
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public ActionResult Index(DateTime? sDate)
        {
            DateTime currentDate = sDate == null ? DateTime.Today : sDate.Value;
            List<PlanPeriod> times = dbp.GetStartTime();
            ViewData["times"] = times;
            DateTime StartDate  = currentDate.AddDays(1 - (int)currentDate.DayOfWeek);
            ViewData["StartDate"] = StartDate;
            List<List<TimeForPlan>>weekPlan=  PlanDataHelper.GetWeekPlan( dbp.GetAllPlan(1),1, StartDate,times,int.Parse(Session["currentUser"].ToString()));
            return View(weekPlan);
        }

        /// <summary>
        /// 转到添加计划页面
        /// </summary>
        /// <returns></returns>
        public  ActionResult Add(string id)
        {
            Plan currentPlan = new Plan();
            if(id==null)
            DropDown(null, null);
            else
            {
                 currentPlan = dbp.GetOnePlan(id);
                DropDown(currentPlan.StartTime, currentPlan.EndTime);
            }
            return View(currentPlan);
        }

        /// <summary>
        /// 保存计划信息
        /// </summary>
        /// <param name="planTitle">计划标题</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="planDate">计划日期（日）</param>
        /// <param name="remark">备注</param>
        /// <param name="planId">计划ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(string planTitle, string startTime, string endTime, string planDate, string remark, int planId=0)
        {
            Plan plan=null;
            if (planId != 0)
            {
                 plan = dbp.GetOnePlan(planId);
            }else
            {
                plan = new Plan();
            }
            plan.PlanTitle = planTitle;
            plan.EmpId = 1;
            plan.StartTime = TimeAddHM(planDate, startTime);
            plan.EndTime = TimeAddHM(planDate, endTime);
            plan.Remark = remark;
            plan.CreateTime = DateTime.Now;
            dbp.Save(plan);
            return Json("Index");
        }

        /// <summary>
        /// 删除计划
        /// </summary>
        /// <param name="id">计划ID</param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            dbp.Delete(id);
            return Json("Yes");
        }
        #endregion

        #region 其他逻辑代码
        /// <summary>
        /// 处理计划时间
        /// </summary>
        /// <param name="day">计划的年月天</param>
        /// <param name="hm">时分</param>
        /// <returns></returns>
        public DateTime TimeAddHM(string day,string hm)
        {
            string str=day.ToString()+" "+hm;
            DateTime time = DateTime.Parse(str);
            return time;
        }
        /// <summary>
        /// 绑定时间下拉列表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        public void DropDown(DateTime? startTime,DateTime? endTime)
        {

            if (startTime != null & endTime != null)
            {
                string[] sArray = null;
                string[] eArray = null;
                sArray = startTime.ToString().Split(' ');
                eArray = endTime.ToString().Split(' ');
                ViewData["PlanDate"] = sArray[0];
                var star = sArray[1].Substring(0, sArray[0].Length - 4);
                var end = eArray[1].Substring(0, sArray[0].Length - 4);
                ViewData["StartTime"] = new SelectList(dbp.GetStartTime(), "Time", "Time", star);
                ViewData["EndTime"] = new SelectList(dbp.GetStartTime(), "Time", "Time", end);
            }
            else
            {
            ViewData["StartTime"] = new SelectList(dbp.GetStartTime(), "Time", "Time");
            ViewData["EndTime"] = new SelectList(dbp.GetStartTime(), "Time", "Time");
            }
        }
        /// <summary>
        /// Ping日程表格
        /// </summary>
        /// <param name="strMonth"></param>
        /// <param name="rcList"></param>
        /// <returns></returns>
        public  string RenderCalendarTable(string strMonth, List<RiCheng> rcList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellspacing=\"1\" cellpadding=\"2\" class=\"tbcalendar table table-striped\">");
            //sb.Append("<table cellspacing=\"1\" cellpadding=\"2\" class=\"tbcalendar\">");
            sb.Append(@"<tr><th>&nbsp;&nbsp;星期日&nbsp;&nbsp;</th><th>&nbsp;&nbsp;星期一&nbsp;&nbsp;</th>
                            <th>&nbsp;&nbsp;星期二&nbsp;&nbsp;</th><th>&nbsp;&nbsp;星期三&nbsp;&nbsp;</th>
                            <th>&nbsp;&nbsp;星期四&nbsp;&nbsp;</th><th>&nbsp;&nbsp;星期五&nbsp;&nbsp;</th>
                            <th>&nbsp;&nbsp;星期六&nbsp;&nbsp;</th></tr>");
            DateTime start;
            if (strMonth == null) {
                start = DateTime.Today.AddDays(1 - DateTime.Today.Day);
            }else
            {
                DateTime curentDay = DateTime.Parse(strMonth);
                start = curentDay.AddDays(1 - curentDay.Day);
            }
            //DateTime start = DateTime.Parse(strMonth + "-1");
           // start =DateTime.Parse( "2017/3/1");
            DateTime end = start.AddMonths(1).AddDays(-1);
            int firstDay = (int)start.DayOfWeek;
            if (firstDay > 0)
            {
                sb.Append("<tr>");
                for (int i = 0; i < firstDay; i++)
                {
                    if (i == 0) sb.Append("<td class='weeken'>&nbsp;</td>");
                    else sb.Append("<td>&nbsp;</td>");
                }
            }
            do
            {
                if (start.DayOfWeek == DayOfWeek.Sunday)
                    sb.Append("<tr><td class='weeken'>");
                else if (start.DayOfWeek == DayOfWeek.Saturday)
                    sb.Append("<td class='weeken'>");
                else
                    sb.Append("<td>");
                List<RiCheng> tmplist = rcList.Where(n => n.RCDateTime.Date == start.Date).ToList();
                StringBuilder tmpSB = new StringBuilder();
                foreach (RiCheng rc in tmplist)
                {
                    tmpSB.AppendFormat("<br><a href='javascript:void(0)'  onclick='alink({1})' >{0}</a>", rc.RCTitle,rc.RCId);
                }
                sb.AppendFormat("{0}{1}</td>", start.Day, tmpSB.ToString());
                if (start.DayOfWeek == DayOfWeek.Saturday)
                    sb.Append("</tr>");
                if (start == end) break;
                start = start.AddDays(1);
            }
            while (true);
            int endDay = (int)end.DayOfWeek;
            if (endDay < 6)
            {
                for (int i = endDay; i < 8; i++)
                {
                    if (i == 5)
                        sb.Append("<td class='weeken'>&nbsp;</td>");
                    else
                        sb.Append("<td>&nbsp;</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        #endregion

        #region 日程
        /// <summary>
        /// 转到日程首页
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult RiCheng(string month)
        {
            DateTime SMonth = DateTime.Today;
            if (month != null && month.Length > 0)
                SMonth = DateTime.Parse(month + "-1");
            List<RiCheng> relist = dbp.GetMonthRiCheng(SMonth,int.Parse(Session["currentUser"].ToString()));
           string strTable= RenderCalendarTable(month, relist);
            ViewData["strTable"] = strTable;
            return View();
        }

        /// <summary>
        /// 保存日程信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="remark">备注</param>
        /// <param name="dateTime">日程时间</param>
        /// <param name="riChengId">日程ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddRiCheng(string title, string remark, string dateTime,int riChengId = 0)
        {
            RiCheng  riCheng = null;
            if (riChengId != 0)
            {
                riCheng = dbp.GetOneRiCheng(riChengId);
            }
            else
            {
                riCheng = new RiCheng();
            }
            riCheng.RCTitle = title;
            riCheng.EmpId = 1;
            riCheng.RCDateTime = DateTime.Parse(dateTime);
            riCheng.Remark = remark;
            riCheng.CreateTime = DateTime.Now;
            dbp.SaveRC(riCheng);
            return Json("OK");
        }
        /// <summary>
        /// 获取一个日程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetOneRC(int id)
        {
            RiCheng  rc= dbp.GetOneRiCheng(id);
            var strResult = "{\"rc\":" + rc.ToJsonString() + "}";
            return Json(strResult);
        }

        public JsonResult DeleteRC(int id)
        {
            dbp.DeleteRiCheng(id);
            return Json("OK");
        }
        #endregion
    }
}