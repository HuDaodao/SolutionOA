using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
     public static class PlanDataHelper
    {
        public static List<List<TimeForPlan>> GetWeekPlan(this Table<Plan> source, int empId, DateTime startDate, List<PlanPeriod> PlanTimes,int id)
        {
            List<List<TimeForPlan>> lst = new List<List<TimeForPlan>>();
            for(int i = 0; i < 7; i++)
            {
                startDate = i==0?startDate.AddDays(0):startDate.AddDays(1);
                List<TimeForPlan> tfp = new List<TimeForPlan>();
                List<Plan> src = source.Where(n => n.StartTime.Date == startDate.Date&&n.EmpId==id).ToList();
                List<string> starts = src.Select(n => n.StartTime.ToString("HH:mm")).ToList();
                List<string> ends = src.Select(n => n.EndTime.ToString("HH:mm")).ToList();
                bool hasWork = false;
                foreach(PlanPeriod pt in PlanTimes)
                {
                    TimeForPlan tmp = new TimeForPlan();
                    tmp.Status = 0;
                    tmp.HasWork = hasWork;

                    int sindex = starts.IndexOf(pt.Time);
                    if (sindex > -1)
                    {
                        tmp.Status = 1;
                        tmp.HasWork = true;
                        tmp.Title = src[sindex].PlanTitle;
                        hasWork = true;
                    }
                    int end = ends.IndexOf(pt.Time);
                    if (end > -1)
                    {
                        tmp.Status = -1;
                        tmp.HasWork = true;
                        tmp.Title = src[end].PlanTitle;
                        hasWork = false;
                    }
                    tfp.Add(tmp);
                }
                lst.Add(tfp);
            }
            return lst;
        }
    }
    /// <summary>
    /// 每个时间点的情况
    /// </summary>
    public class TimeForPlan
    {
        /// <summary>
        /// 当前是不是有工作
        /// </summary>
        public bool HasWork { get; set; }
        /// <summary>
        /// 这个点是事件的开始还是结束
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 工作的标题
        /// </summary>
        public string Title { get; set; }
    }
}
