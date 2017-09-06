using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace OA.Common
{
    public static class ToJson
    {
        public static string ToJsonString(this Object obj)
        {
            //JavaScriptSerializer s = new JavaScriptSerializer();
            //StringBuilder sb = new StringBuilder();
            //s.Serialize(obj, sb);
            //return sb.ToString();
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return JsonConvert.SerializeObject(obj, jsSettings);
        }
    }
}
