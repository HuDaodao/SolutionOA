using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
    public class ProjectList
    {
        public  int ProjectId { get; set;  }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int  Status { get; set; }
        /// <summary>
        /// 已完成的任务
        /// </summary>
        public  int DoneTask { get; set; }
        /// <summary>
        /// 任务总数
        /// </summary>
        public  int TaskListCount  { get; set;}

        public int Done
        {
            get
            {
                if (TaskListCount == 0)
                    return 0;
                else
                {
                    return DoneTask  * 100/ TaskListCount;
                }
            }
        }
    }
}
