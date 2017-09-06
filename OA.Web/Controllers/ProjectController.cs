using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OA.Model;
using OA.Common;

namespace OA.Web.Controllers
{
    public class ProjectController : Controller
    {
        ProjectData db = new ProjectData();
        ProjectList pl = new ProjectList();
        #region 项目管理
        /// <summary>
        /// 返回页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int status=0)
        {
            var projectList = db.GetProjectList(1,status);
            return View(projectList);
        }
        /// <summary>
        /// 转到项目详细页
        /// </summary>
        /// <param name="id">项目ID</param>
        /// <returns></returns>
        public  ActionResult Detail(int id)
        {
            ViewData["projId"] = id;
            IEnumerable<TaskList> taskList = db.GetTaskList(id);
            if (taskList.Count()==0) return View("NoTask");
            return View(taskList);
        }
      /// <summary>
      /// 转到新增项目
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        public ActionResult CreateAndEditer(int id=0)
        {
            Project project = new Project();
            if (id != 0)
                project = db.GetModel(id);
            return View(project);
        }

        /// <summary>
        /// 保存项目信息
        /// </summary>
        /// <param name="client">项目实体</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateAndEdit(Project proj)
        {
            Project  currentProj;
            if (proj.ProjectId == 0) { currentProj = new Project(); currentProj.Status = 0; }
            else { currentProj = db.GetModel(proj.ProjectId); }
            currentProj.ProjName = proj.ProjName;

            currentProj.StartDate = proj.StartDate;
            currentProj.EndDate = proj.EndDate;
            var taskLists = db.GetTaskList(proj.ProjectId);
            if (taskLists.Count() == 0)
                currentProj.Status = 1;
            else
            {
                foreach (var taskList in taskLists)
                {
                    if (taskList.Status == 0)
                    {
                        taskList.Status = 0;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            currentProj.BudGet = proj.BudGet;
            currentProj.Remark = proj.Remark;
            db.Save(currentProj);
            return Json("Ok");
        }

        /// <summary>
        /// 修改项目状态
        /// </summary>
        /// <param name="id">项目ID</param>
        /// <returns></returns>
        public JsonResult StatusChange(int id)
        {
            Project  proj = db.GetModel(id);
            proj.Status = proj.Status==0?1:0;
            db.Save(proj);
            return Json("OK");
        }


        /// <summary>
        /// 返回一个项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetOneT(int id)
        {
            Project proj = db.GetModel(id);
            var str = "{\"proj\":" + proj.ToJsonString() + "}";
            return Json(str);
        }



        /// <summary>
        /// 删除一个项目
        /// </summary>
        /// <param name="id">项目ID</param>
        /// <returns></returns>
        public JsonResult DeleteProject(int id)
        {
            bool deletereturn=db.Delete(id);
            if (deletereturn == true)
                return Json("yes");
            else
                return Json(null);
        }
        ///// <summary>
        ///// 返回详细信息页面
        ///// </summary>
        ///// <param name="id">客户ID</param>
        ///// <returns></returns>
        //public ViewResult DetailMsg(int id)
        //{
        //    ViewData["clientId"] = id;
        //    IEnumerable<ClientRelation> relations = db.GetRelations(id);
        //    return View(relations);
        //}

        #endregion

        #region 阶段任务

        /// <summary>
        /// 保存阶段任务记录
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="title">标题</param>
        /// <param name="Remark">备注</param>
        /// <param name="projectId">所属项目ID</param>
        /// <param name="TLId">阶段任务ID</param>
        /// <returns></returns>
        public JsonResult AddTL(DateTime startTime, DateTime endTime, string title, string Remark, int projectId, int TLId = 0)
        {
            TaskList taskList;
            if (TLId == 0) {
                taskList = new TaskList();
                taskList.Status = 0;
            }
            else { taskList = db.GetTaskListModel(TLId); }
            taskList.TLName = title;
            taskList.StartTime = startTime;
            taskList.Remark = Remark;
            taskList.EndTime = endTime;
            taskList.ProjId = projectId;
            var tasks = db.GetTasks(TLId);
            taskList.Status = 1;
            if (tasks.Count() == 0)
            {
                taskList.Status = 0;
            }else
            {
               foreach(var task in tasks)
                        {
                            if (task.Status == 0)
                            {
                                taskList.Status = 0;
                                break;
                            }else
                            {
                                continue;
                            }
                        }
            }
            db.SaveTaskList(taskList);
            return Json("OK");
        }
        /// <summary>
        /// 返回一个阶段任务
        /// </summary>
        /// <param name="id">阶段计划ID</param>
        /// <returns></returns>
        public JsonResult GetOneTL(int id)
        {
            TaskList tl = db.GetTaskListModel(id);
            var str = "{\"tl\":" + tl.ToJsonString() + "}";
            return Json(str);
        }
        /// <summary>
        /// 删除一条交涉记录
        /// </summary>
        /// <param name="id">阶段任务ID</param>
        /// <returns></returns>
        public JsonResult DeleteTL(int id)
        {
           var deleResult= db.DeleteTaskList(id);
            if(deleResult==true)
            return Json("OK");
            return Json(null);
        }
        #endregion
    }
}