using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinqExample
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] data = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var result = from d in data
                         where d % 2 == 0
                         orderby d descending
                         select d;

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }

            var result1 = data.Where(d => d % 2 == 0).OrderBy(c => c);

            foreach (var item in result1)
            {
                Console.WriteLine(item);
            }

            int[] data1 = { 1, 2, 3 };
            int[] data2 = { 4, 5, 6 };

            var result3 = from d1 in data1
                          from d2 in data2
                          select d1 * d2;

            Console.WriteLine(string.Join(", ", result3));

            LinqXML();

            Console.Read();
        }

        private static void ProductMethod()
        {
            // Product - Order Example
            var orders = new List<Order>();

            var averageNumberOfOrderLines = orders.Average(o => o.OrderLines.Count);
            var result5 = from o in orders
                          from l in o.OrderLines
                          group l by l.Product into p
                          select new
                          {
                              Product = p.Key,
                              Amount = p.Sum(x => x.Amount)
                          };

            int pageIndex = 1;
            int pageSize = 10;

            var pagedOrders = orders
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }

        public static void LinqXML()
        {

            String xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                    <people>
                      <person firstname=""john"" lastname=""doe"">
                        <contactdetails>
                          <emailaddress>john@unknown.com</emailaddress>
                        </contactdetails>
                      </person>
                      <person firstname=""jane"" lastname=""doe"">
                        <contactdetails>
                          <emailaddress>jane@unknown.com</emailaddress>
                          <phonenumber>001122334455</phonenumber>
                        </contactdetails>
                      </person>
                    </people>";

            XDocument doc = XDocument.Parse(xml);
            IEnumerable<string> personNames = from p in doc.Descendants("person")
                                              select (string)p.Attribute("firstName")
                                                 + " " + (string)p.Attribute("lastName");
            foreach (string s in personNames)
            {
                Console.WriteLine(s);
            }

            // XML create
            XElement root = new XElement("Root",
            new List<XElement>
            {
                new XElement("Child1"),
                new XElement("Child2"),
                new XElement("Child3")
            },
            new XAttribute("MyAttribute", 42));
            root.Save("test.xml");
        }
    }

    [Serializable]
    public class Product
    {
        public string Description { get; set; }

        public decimal Price { get; set; }
    }

    [Serializable]
    public class Orderline
    {
        public int Amount { get; set; }

        public Product Product { get; set; }
    }

    [Serializable]
    public class Order
    {
        public List<Orderline> OrderLines { get; set; }
    }
}
