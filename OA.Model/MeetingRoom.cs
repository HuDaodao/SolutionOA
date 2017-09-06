using System.Collections.Generic;
using System.Linq;

namespace OA.Model
{
    public class MeetingRoomData : BaseData
    {
        /// <summary>
        /// 获取会议室信息
        /// </summary>
        /// <param name="roomId">会议室ID</param>
        /// <returns></returns>
        public MeetRoom GetModel(int roomId)
        {
            return db.MeetRoom.Single(n => n.RoomId == roomId);
        }

        /// <summary>
        /// 获取会议室信息
        /// </summary>
        /// <param name="roomName">会议室名</param>
        /// <returns></returns>
        public MeetRoom GetModel(string roomName)
        {
            try
            {
                return db.MeetRoom.Single(n => n.RoomName == roomName);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 保存会议室信息的修改
        /// </summary>
        /// <param name="emp">会议室实体</param>
        public void Save(MeetRoom room)
        {
            if (room.RoomId == 0)
                db.MeetRoom.InsertOnSubmit(room);
            db.SubmitChanges();
        }
       
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">会议室id</param>
        public void Detail(int id)
        {
            MeetRoom room = GetModel(id);
            db.MeetRoom.DeleteOnSubmit(room);
            db.SubmitChanges();
        }
        /// <summary>
        /// 获取全部会议室列表
        /// </summary>
        /// <param name="roomName">会议室名</param>
        /// <param name="IsValid">是否有效</param>
        /// <returns></returns>
        public List<MeetRoom> GetListAll(string roomName,bool IsValid)
        {
            List<MeetRoom> roomAll;
            bool isValid = IsValid.ToString() == null ? true : IsValid;
            if (string.IsNullOrEmpty(roomName))
                roomAll = db.MeetRoom.Where(n=>n.IsValid== isValid).OrderByDescending(n => n.RoomId).ToList();
            else
            {
                roomAll = db.MeetRoom.Where(n => n.RoomName.Contains(roomName)&&n.IsValid==isValid).OrderByDescending(n => n.RoomId).ToList();
            }
            return roomAll;
        }
        /// <summary>
        /// 分页获取列表 
        /// </summary>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="roomName">会议室名</param>
        /// <param name="IsValid">是否有效</param>
        /// <returns></returns>
        public List<MeetRoom > GetListByMe(int pageindex,int pageSize,string roomName, bool IsValid)
        {
            List<MeetRoom> roomList;
            bool isValid = IsValid.ToString() == null ? false : IsValid;
            if (roomName != null&&roomName!="")
            {
                roomList = db.MeetRoom.Where(n=>n.RoomName.Contains(roomName) &&n.IsValid == IsValid).OrderByDescending(n=>n.RoomId).Skip((pageindex-1)* pageSize).Take(pageSize).ToList();
            }
            else
            {
                roomList = db.MeetRoom.Where(n=>n.IsValid == IsValid).OrderByDescending(n => n.RoomId).Skip((pageindex - 1) * pageSize).Take(pageSize).ToList();
            }
            return roomList;
        }
        /// <summary>
        /// 设置为不可用
        /// </summary>
        /// <param name="id">会议室ID</param>
        public void ValidChangeNo(int id)
        {
            MeetRoom room= GetModel(id);
            room.IsValid = false;
            db.SubmitChanges();
        }
        /// <summary>
        /// 设置为可用
        /// </summary>
        /// <param name="id">会议室ID</param>
        public void ValidChangeYes(int id)
        {
            MeetRoom room = GetModel(id);
            room.IsValid = true;
            db.SubmitChanges();
        }

    }
}
