using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
  public class PositionData:BaseData
    {
        #region 查询

        /// <summary>
        /// 得到某个职位下的所有员工
        /// </summary>
        /// <param name="id">职位ID</param>
        /// <returns></returns>
        public List<Employee> GetPositEmp(int id)
        {
            return db.Employee.Where(n => n.PositionId == id).ToList();
        }
        /// <summary>
        /// 获取所有的职位
        /// </summary>
        /// <returns></returns>
        public List<Position> GetAllPosition()
        {
            return db.Position.ToList();
        }

        /// <summary>
        /// 通过ID得到一个部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public Position GetModel(int id)
        {
            return db.Position.Single(n => n.PositionId == id);
        }

        /// <summary>
        /// 得到一级职位
        /// </summary>
        /// <returns></returns>
        public List<Position> GetTopList()
        {
            return db.Position.Where(n => n.ParentId == null).ToList();
        }

        public List<Position> GetParentPosit(int id)
        {
            List<int> arr = new List<int>();
            List<int> arrs = CanBeArr(arr, id);
            List<Position> canBeParentList = (from p in db.Position
                                              where !arrs.Contains(p.PositionId)
                                              select p).ToList();
            return canBeParentList;
        }
        #endregion

        #region 保存

        /// <summary>
        /// 保存修改或新增
        /// </summary>
        /// <param name="posit"></param>
        public void Save(Position posit)
        {
            if (posit.PositionId == 0)
            {
                db.Position.InsertOnSubmit(posit);
            }
            db.SubmitChanges();
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id">职位ID</param>
        public void Delete(int id)
        {
            Position posit = GetModel(id);
            db.Position.DeleteOnSubmit(posit);
            db.SubmitChanges();
        }

        #endregion

        #region 辅助逻辑
        /// <summary>
        /// 将自己及后代的ID组成一个List
        /// </summary>
        /// <param name="arrs">id列表</param>
        /// <param name="id">当前ID</param>
        /// <returns></returns>
        public List<int> CanBeArr(List<int> arrs,int id)
        {
            arrs.Add(id);
            var inforposit = GetModel(id);
            if (inforposit.Position2.ToList().Count() > 0)
            {
                foreach(var sonPosit in inforposit.Position2.ToList())
                {
                    CanBeArr(arrs, sonPosit.PositionId);
                }
            }
            return arrs;
        }
        #endregion

    }
}
