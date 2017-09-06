using OA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;

namespace OA.Web.Controllers
{
    public class CommonController : Controller
    {
        RoleData rd = new RoleData();
        // GET: Common
        public  string  menuData()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" <ul id='menuList'>");
            List<MyMenu> menus = MenuHelper.AllMenu();
            List<int> codeStr = rd.GetCodes(1);

            foreach (MyMenu menu in menus)
            {
                sb.AppendFormat("<li class='one'><a href='#'>{0}</a><ul>", menu.Name);
                foreach (ToolBar child in menu.Children)
                {
                    if (codeStr.Exists(x => x == child.Code) || child.Right == false)
                        sb.AppendFormat("<li><a href='{0}' target='content'>{1}</a></li>", child.Url, child.Name);
                }
                sb.Append("</ul></li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        private  Employee currentUser;
        protected Employee CurrentUser
        {
            get
            {
                if (currentUser != null)
                    return currentUser;
                string userid = User.Identity.Name;
                string userid2 = WindowsIdentity.GetCurrent().Name;
                if (string.IsNullOrEmpty(userid))
                    return null;
                EmployeeData ed = new EmployeeData();
                currentUser = ed.GetModel(int.Parse(userid));
                return currentUser;
            }
        }


        /// <summary>
        /// 是否
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> YesNo()
        {
            List<SelectListItem> list = new List<SelectListItem>() {
                      (new SelectListItem() {Text = "否", Value = "false"}),
                      (new SelectListItem() {Text = "是", Value = "true"})
                    };
            return list;
        }

    }
}