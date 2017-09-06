using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace OA.Model
{
    public class ProjectMemberData : BaseData
    {
        /// <summary>
        /// 获取项目成员
        /// </summary>
        /// <param name="projId">项目ID</param>
        /// <returns></returns>
        public List<ProjectMember> GetMembers(int projId)
        {
            return db.ProjectMember.Where(n => n.ProjId == projId).ToList();
        }
       /// <summary>
       /// 删除项目成员
       /// </summary>
       /// <param name="id"></param>
        public void DeleteMember(int id)
        {

        }
    }
}
