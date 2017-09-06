using OA.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
  public   class PlanData:BaseData
    {
        #region 查询
        /// <summary>
        /// 得到所有计划
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public Table<Plan> GetAllPlan(int id)
        {
            var b = db.Plan as Table<Plan>;
            return b;
           // return db.Plan.Where(n => n.EmpId == id) as Table<Plan>;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        public List<PlanPeriod> GetStartTime()
        {
            var a = db.PlanPeriod.ToList();
            return db.PlanPeriod.ToList();
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <param name="timeId"></param>
        /// <returns></returns>
        public List<PlanPeriod> GetEndTime(int timeId)
        {
            return db.PlanPeriod.Where(n => n.TPId > timeId).ToList();
        }
        /// <summary>
        /// 查询一个计划
        /// </summary>
        /// <param name="id">计划ID</param>
        /// <returns></returns>
        public Plan GetOnePlan(int id)
        {
            return db.Plan.Where(n => n.PlanId == id).FirstOrDefault();
        }
        /// <summary>
        /// 查询一个计划
        /// </summary>
        /// <param name="title">计划ID</param>
        /// <returns></returns>
        public Plan GetOnePlan(string title)
        {
            return db.Plan.Where(n => n.PlanTitle == title).FirstOrDefault();
        }
        /// <summary>
        /// 获取指定月份的日程
        /// </summary>
        /// <param name="month">月</param>
        /// <returns></returns>
        public List<RiCheng> GetMonthRiCheng(DateTime month,int id)
        {
           var a= db.RiCheng.Where(n =>n.EmpId==id&& n.RCDateTime.Year == month.Year &&n.RCDateTime.Month==month.Month).ToList();
            return a;
        }
        #endregion

        #region 保存 修改
        /// <summary>
        /// 保存计划
        /// </summary>
        /// <param name="plan">一个计划的实体</param>
        public void Save(Plan plan)
        {
            if (plan.PlanId == 0)
                db.Plan.InsertOnSubmit(plan);
            db.SubmitChanges();
        }
        #endregion

        #region 删除计划
        /// <summary>
        /// 删除计划
        /// </summary>
        /// <param name="id">计划ID</param>
        public void Delete(int id)
        {
           Plan plan=GetOnePlan(id);
            db.Plan.DeleteOnSubmit(plan);
            db.SubmitChanges();
        }
        #endregion

        #region 日程部分
        /// <summary>
        /// 得到一个日程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RiCheng GetOneRiCheng(int id)
        {
            return db.RiCheng.Where(n => n.RCId == id).FirstOrDefault();
        }

        /// <summary>
        /// 保存日程
        /// </summary>
        /// <param name="RC">一个日程的实体</param>
        public void SaveRC(RiCheng  RC)
        {
            if (RC.RCId == 0)
                db.RiCheng.InsertOnSubmit(RC);
            db.SubmitChanges();
        }


        /// <summary>
        /// 删除日程
        /// </summary>
        /// <param name="id">日程ID</param>
        public void DeleteRiCheng(int id)
        {
            RiCheng richeng = GetOneRiCheng(id);
            db.RiCheng.DeleteOnSubmit(richeng);
            db.SubmitChanges();
        }
        #endregion
        #region 其他


        #endregion

    }
}
