using OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OA.Web.Helper
{
    public static  class HtmlHelperExt
    {
        public static string RenderTitle(this HtmlHelper html,string strTitle)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='hd' style='width:100%;'>
              <table cellspacing='0' cellpadding='0' border='0' style='width:100%;border-collapse:collapse;'>
                <tr style='width:100%;'><td align='left'><div align='left'><b>");
            sb.Append(strTitle);
            sb.Append("</b></div></td></tr></table></div><br/>");
            return sb.ToString();
        }

        public static string RenderCalendarTable(string strMonth,List<RiCheng>rcList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellspacing=\"1\" cellpadding=\"2\" class=\"tbcalendar\">");
            sb.Append("<tr><th>星期日</th><th>星期一</th><th>星期二</th><th>星期三</th><th>星期四</th><th>星期五</th><th>星期六</th></tr>");
            DateTime start = DateTime.Parse(strMonth + "-1");
            DateTime end = start.AddMonths(1).AddDays(-1);
            int firstDay = (int)start.DayOfWeek;
            if (firstDay > 0)
            {
                sb.Append("<tr>");
                for(int i = 0; i < firstDay; i++)
                {
                    if (i == 0) sb.Append("<td class='weeken'>&nbsp;</td>");
                    else sb.Append("<td>&nbsp;</td>");
                }
            }
            do
            {
                if (start.DayOfWeek == DayOfWeek.Sunday)
                    sb.Append("<tr><td class='weeken'>");
                else if (start.DayOfWeek == DayOfWeek.Saturday)
                    sb.Append("<td class='weeken'>");
                else
                    sb.Append("<td>");
                List<RiCheng> tmplist = rcList.Where(n => n.RCDateTime.Date == start.Date).ToList();
                StringBuilder tmpSB = new StringBuilder();
                foreach(RiCheng rc in tmplist)
                {
                    tmpSB.AppendFormat("<br><a href='#'>{0}</a>",rc.RCTitle);
                }
                sb.AppendFormat("{0}{1}</td>", start.Day,tmpSB.ToString());
                if (start.DayOfWeek == DayOfWeek.Saturday)
                    sb.Append("</tr>");
                if (start == end) break;
                start = start.AddDays(1);
            }
            while (true);
            int endDay = (int)end.DayOfWeek;
            if (endDay < 6)
            {
                for(int i = endDay; i < 8; i++)
                {
                    if (i == 5)
                        sb.Append("<td class='weeken'>&nbsp;</td>");
                    else
                        sb.Append("<td>&nbsp;</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
    }
}