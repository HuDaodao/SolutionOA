using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Model
{
    public class MenuHelper
    {
        /// <summary>
        /// 得到所有菜单
        /// </summary>
        /// <returns></returns>
        public static List<MyMenu> AllMenu()
        {
            string menuPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "MyMenu.config";
            XDocument doc = XDocument.Load(menuPath);
            var menus = from x in doc.Descendants("MyMenu")
                        select new MyMenu
                        {
                            Code = (int)x.Attribute("code"),
                            Name = (string)x.Attribute("name"),
                            Right = (bool)x.Attribute("right"),
                            Children=(from t in x.Descendants("ToolBar")
                                      select new ToolBar
                                      {
                                          Code=(int)t.Attribute("code"),
                                          Name=(string)t.Attribute("name"),
                                          Url=(string)t.Attribute("url"),
                                          Right=t.Attribute("right")==null?false:(bool)t.Attribute("right"),
                                      }).ToList()
                        };
            return menus.ToList();
        }

   

        /// <summary>
        /// 无需权限的菜单
        /// </summary>
        /// <returns></returns>
        public static List<MyMenu> CommonMenu()
        {
            List<MyMenu> allMenu = AllMenu();
            return allMenu.Where(n => n.Right == false).ToList();
        }
        /// <summary>
        /// 所有用于权限的菜单
        /// </summary>
        /// <returns></returns>
        public static List<MyMenu> RightMenu()
        {
            return AllMenu().Where(n => n.Right).ToList();
        }

     }
    public class MyMenu
    {
        public int Code { get; set; }
        public string  Name { get; set; }
        public string  Url { get; set; }
        public bool Right { get; set; }
        public List<ToolBar> Children { get; set; }
    }
    public class ToolBar
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Right { get; set; }

    }
}
