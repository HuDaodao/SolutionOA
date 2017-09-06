using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
   public  class ClientData:BaseData
    {
        #region 关于客户
        /// <summary>
        /// 返回一个客户实体
        /// </summary>
        /// <param name="Id">客户Id</param>
        /// <returns></returns>
        public Client GetModel(int Id)
        {
            return db.Client.Single(n => n.CId == Id);
        }
        /// <summary>
        /// 返回所有客户列表
        /// </summary>
        /// <returns></returns>
        public List<Client> GetAllClient(string clientName,int CType)
        {
            IQueryable<Client> clientAll;
            List<Client> clientList;
            if (CType == 0)
            {
                clientAll = db.Client.Where(n => n.CName.Contains(clientName));
            }else
            {
                clientAll = db.Client.Where(n => n.CName.Contains(clientName) && n.CType == CType);
            }
            try
            {
                clientList = clientAll.ToList();
            }
            catch {
                return null;
            }
            return clientList;
        }
        /// <summary>
        /// 分页查客户
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="clientName">客户姓名</param>
        /// <param name="CType">客户类别</param>
        /// <param name="Status">有效性</param>
        /// <returns></returns>
        public List<Client> GetListByPage(int pageIndex,int pageSize,string clientName, int CType, bool Status)
        {
            IQueryable<Client> clients;
            List<Client> clientList;
            if (CType ==-1)
            {
                //查询全部的
                clients = db.Client.Where(n => n.CName.Contains(string.IsNullOrEmpty(clientName) ? "" : clientName)&&n.Status==Status).OrderByDescending(n => n.CId).Skip((pageIndex - 1) * pageSize).Take(pageSize) ;
            }else
            {
                //查重要或普通的
                clients = db.Client.Where(n => n.CName.Contains(string.IsNullOrEmpty(clientName) ? "" : clientName)&&n.CType==CType && n.Status == Status).OrderByDescending(n => n.CId).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            try
            {
                clientList = clients.ToList();
            }catch
            {
                return null;
            }
            return clientList;
        }
        /// <summary>
        /// 获得一个客户的接触记录
        /// </summary>
        /// <param name="clientId">客户ID</param>
        /// <returns></returns>
        public List<ClientRelation> GetRelations0(int clientId)
        {
            var relations = db.ClientRelation.Where(n => n.CId == clientId).OrderByDescending(n=>n.Date);
            try
            {
                return relations.ToList();
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<ClientRelation> GetRelations(int clientId)
        {
            return db.ClientRelation.Where(n => n.CId == clientId).OrderByDescending(n => n.Date);
           
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="client">客户实体</param>
        public void Save(Client client)
        {
            if (client.CId == 0)
                db.Client.InsertOnSubmit(client);
            db.SubmitChanges();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">客户ID</param>
        public void Delete(int id)
        {
            Client  client = GetModel(id);
            db.Client.DeleteOnSubmit(client);
            db.SubmitChanges();
        }
        #endregion

        #region 交涉记录
        /// <summary>
        /// 获取一条交涉记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClientRelation GetCRModel(int id)
        {
            return db.ClientRelation.SingleOrDefault(n => n.CRId == id);
        }
        /// <summary>
        /// 保存交涉记录
        /// </summary>
        /// <param name="cr">交涉记录实体</param>
        public void SaveCR(ClientRelation cr)
        {
            if (cr.CRId == 0)
                db.ClientRelation.InsertOnSubmit(cr);
            db.SubmitChanges();
        }
        /// <summary>
        /// 删除交涉关系
        /// </summary>
        /// <param name="id">交涉关系ID</param>
        public void DeleteCR(int id)
        {
            ClientRelation cr = GetCRModel(id);
            db.ClientRelation.DeleteOnSubmit(cr);
            db.SubmitChanges();
        }
        #endregion
    }
}
