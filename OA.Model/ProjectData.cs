using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
   public  class ProjectData:BaseData
    {
        #region 项目
        /// <summary>
        /// 返回计算过的project（searchMsg还没有用来查）
        /// </summary>
        /// <param name="searchMsg"></param>
        /// <param name="empid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<ProjectList> GetProjectList( int empid, int status)
        {
            int[] projectids = db.ProjectMember.Where(n => n.EmpId == empid).Select(n => n.ProjId).ToArray();
            var src = from p in db.Project
                      where projectids.Contains(p.ProjectId) && p.Status == status
                      select new ProjectList
                      {
                          ProjectId = p.ProjectId,
                          Name = p.ProjName,
                          StartDate = p.StartDate,
                          EndDate = p.EndDate,
                          Status = p.Status,
                          DoneTask = (from a in db.TaskList where a.ProjId == p.ProjectId && a.Status == 1 select a).Count(),
                          TaskListCount = (from b in db.TaskList where b.ProjId == p.ProjectId select b).Count()
                      };
            return src.ToList();
        }

        /// <summary>
        /// 返回一个项目实体
        /// </summary>
        /// <param name="Id">项目Id</param>
        /// <returns></returns>
        public Project GetModel(int Id)
        {
            return db.Project.SingleOrDefault(n => n.ProjectId == Id);
        }
        /// <summary>
        /// 返回所有项目列表
        /// </summary>
        /// <param name="searchMsg">查询条件</param>
        /// <param name="status">项目状态</param>
        /// <returns></returns>
        public List<Project> GetAllProject(string searchMsg,int status)
        {
            IQueryable<Project> projectAll;
            List<Project> projectList;
            status = status == 0 ? 0 : 1;
            if (searchMsg != null)
                projectAll = db.Project.Where(n => n.ProjName == searchMsg && n.Status == status);
            else
            {
                projectAll = db.Project.Where(n =>  n.Status == status);
            }
            try
            {
                projectList = projectAll.ToList();
            }
            catch {
                return null;
            }
            return projectList;
        }
        /// <summary>
        /// 获得一个项目的阶段性任务
        /// </summary>
        /// <param name="clientId">项目ID</param>
        /// <returns></returns>
        public IEnumerable <TaskList> GetTaskList(int projectId)
        {
            var taskLists = db.TaskList.Where(n => n.ProjId == projectId).OrderByDescending(n=>n.TLId);
            return taskLists;
        }
     
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="project">项目实体</param>
        public void Save(Project project)
        {
            if (project.ProjectId == 0)
                db.Project.InsertOnSubmit(project);
            db.SubmitChanges();
        }
        /// <summary>
        /// 删除一个项目
        /// </summary>
        /// <param name="id">项目ID</param>
        public bool Delete(int id)
        {
         try{ var tasks = db.Task.Where(n => n.ProjId == id);
                if (tasks.Count() != 0)
                    db.Task.DeleteAllOnSubmit(tasks);
                if (GetTaskList(id).Count() != 0)
                    db.TaskList.DeleteAllOnSubmit(GetTaskList(id));
                var members = db.ProjectMember.Where(n => n.ProjId == id);
                if (members.Count() != 0)
                    db.ProjectMember.DeleteAllOnSubmit(members);
                Project project = GetModel(id);
                db.Project.DeleteOnSubmit(project);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 阶段任务
        /// <summary>
        /// 获取一条阶段性任务
        /// </summary>
        /// <param name="id">阶段性任务ID</param>
        /// <returns></returns>
        public TaskList GetTaskListModel(int id)
        {
            return db.TaskList.SingleOrDefault(n => n.TLId == id);
        }
        /// <summary>
        /// 保存阶段任务
        /// </summary>
        /// <param name="taskList">阶段任务实体</param>
        public void SaveTaskList(TaskList taskList)
        {
            if (taskList.TLId == 0)
                db.TaskList.InsertOnSubmit(taskList);
            db.SubmitChanges();
        }
        /// <summary>
        /// 删除阶段任务
        /// </summary>
        /// <param name="id">阶段任务ID</param>
        public bool DeleteTaskList(int id)
        {
            try
            {
                var task = GetTasks(id);
                if (task.Count() != 0) db.Task.DeleteAllOnSubmit(task);
                TaskList tl = GetTaskListModel(id);
                db.TaskList.DeleteOnSubmit(tl);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 个人分配任务
        /// <summary>
        /// 一个阶段任务的个人任务列表
        /// </summary>
        /// <param name="taskListId">阶段任务ID</param>
        /// <returns></returns>
        public List<Task> GetTasks(int taskListId)
        {
            return db.Task.Where(n => n.TLId == taskListId).ToList();
        }
        #endregion
    }
}
