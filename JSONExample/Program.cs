using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace JSONExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = "{\"employees\":[{\"firstName\":\"John\", \"lastName\":\"Doe\"}, {\"firstName\":\"Anna\", \"lastName\":\"Smith\"}, {\"firstName\":\"Peter\",\"lastName\":\"Jones\"}]}";
            var serializer = new JavaScriptSerializer();
            var result = serializer.Deserialize<Dictionary<string, object>>(json);

            Console.Read();
        }
    }
}
