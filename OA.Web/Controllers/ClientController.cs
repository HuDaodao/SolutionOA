using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OA.Model;
using OA.Common;

namespace OA.Web.Controllers
{
    public class ClientController : Controller
    {
        ClientData db = new ClientData();
        #region 客户
        /// <summary>
        /// 返回页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="searchMsg">客户姓名</param>
        /// <param name="CType">客户类型</param>
        /// <param name="Status">有效性</param>
        /// <returns></returns>
        public JsonResult List_Json(int? page,string searchMsg,int CType,bool Status)
        {
            int pageIndex=page??1;
            int pageSize = 10;
            List<Client> list = db.GetListByPage(pageIndex, pageSize, searchMsg, CType,Status);
            int rowCount = db.GetAllClient(searchMsg, CType).Count();
            var pageCount = 0;
            if (rowCount % pageSize != 0)
            {
                if (rowCount - pageSize < 0)
                {
                    pageCount = 1;
                }
                else
                {
                    pageCount = rowCount / pageSize + 1;
                }
            }
            else
            {
                pageCount = rowCount / pageSize;
            }
            var strResult = "{\"pageCount\":" + pageCount + ",\"CurrentPage\":" + pageIndex + ",\"list\":" + list.ToJsonString() + "}";
            return Json(strResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改客户类型
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <param name="aimType">目标类型</param>
        /// <returns></returns>
        public JsonResult CTypeChange(int id,int aimType)
        {
            Client client = db.GetModel(id);
            client.CType = aimType;
            db.Save(client);
            return Json("OK");
        }
        /// <summary>
        /// 修改数据有效性
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <returns></returns>
        public JsonResult StatusChange(int id)
        {
            Client client = db.GetModel(id);
            client.Status = !client.Status;
            db.Save(client);
            return Json("OK");
        }
        /// <summary>
        /// 新增客户
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <returns></returns>
        public ViewResult CreateAndEdit(int id=0)
        {
            Client client = null;
            if (id != 0)
            {
                client = db.GetModel(id);
            }
            if (client == null)
                    client = new Client();
            return View(client);
        }
        /// <summary>
        /// 保存客户信息
        /// </summary>
        /// <param name="client">客户实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateAndEdit(Client client)
        {
            Client currentClient;
            if (client.CId == 0) { currentClient = new Client(); }
            else { currentClient = db.GetModel(client.CId); }
            currentClient.CName = client.CName;
            currentClient.Company = client.Company;
            currentClient.Mail = client.Mail;
            currentClient.Address = client.Address;
            currentClient.Phone = client.Phone;
            currentClient.Position = client.Position;
            currentClient.Department = client.Department;
            currentClient.Status = currentClient.Status??true;
            currentClient.EmpId = int.Parse(Session["currentUser"].ToString());
           currentClient.CType = Request["CType"]=="1"?1:0;
            currentClient.Remark = client.Remark;
            db.Save(currentClient);
            return Json("OK");
        }

        /// <summary>
        /// 删除一个客户
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            db.Delete(id);
            return Json("yes");
        }
        /// <summary>
        /// 返回详细信息页面
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <returns></returns>
        public ViewResult DetailMsg(int id)
        {
            ViewData["clientId"] = id;
            IEnumerable<ClientRelation> relations = db.GetRelations(id);
            return View(relations);
        }
        /// <summary>
        /// 返回一个客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetOneClient(int id)
        {
            Client client = db.GetModel(id);
            var str = "{\"client\":" + client.ToJsonString() + "}";
            return Json(str);
        }
        #endregion

        #region 交涉记录
        /// <summary>
        /// 保存交涉记录
        /// </summary>
        /// <param name="time">交涉时间</param>
        /// <param name="title">时间标题</param>
        /// <param name="Remark">备注</param>
        /// <param name="CId">客户ID</param>
        /// <param name="EmpId">用户ID</param>
        /// <param name="CRId">交涉记录ID</param>
        /// <returns></returns>
        public JsonResult AddCR (DateTime time, string title,string Remark,int CId,int CRId=0)
        {
            ClientRelation curRelation;
            if (CRId== 0) { curRelation = new ClientRelation(); }
            else { curRelation = db.GetCRModel(CRId); }
            curRelation.Title = title;
            curRelation.Date = time;
            curRelation.Remark = Remark;
            curRelation.EmpId = int.Parse(Session["currentUser"].ToString());
            curRelation.Status = true;
            curRelation.CId = CId;
            db.SaveCR(curRelation);
            return Json("OK");
        }
        /// <summary>
        /// 返回一个交涉记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetOneCR(int id)
        {
            ClientRelation cr = db.GetCRModel(id);
            var str = "{\"cr\":" + cr.ToJsonString() + "}";
            return Json(str);
        }
        /// <summary>
        /// 删除一条交涉记录
        /// </summary>
        /// <param name="id">交涉记录ID</param>
        /// <returns></returns>
        public JsonResult DeleteCR(int id)
        {
            db.DeleteCR(id);
            return Json("OK");
        }
        #endregion
    }
}