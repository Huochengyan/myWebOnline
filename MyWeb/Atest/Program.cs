using Atest._23;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atest
{
    class Program
    {
        private static object o = new object();
        private static List<Product> _Products { get; set; }
        static void Main(string[] args)
        {
            GenericClass genericClass = new GenericClass();
            GenericClass.Get<Product>(new Product());
        }

        /*执行集合数据添加操作*/
        static void AddProducts()
        {
            Parallel.For(0, 1000, (i) =>
            {
                lock (o)
                { 
                    Product product = new Product();
                    product.Name = "name" + i;
                    product.Category = "Category" + i;
                    product.SellPrice = i;
                    _Products.Add(product);
                   // Console.WriteLine(i.ToString());
                    //File.AppendAllText("11111.txt",i.ToString());
                }
            });
        }

        /// <summary>
        ///  模拟多个
        /// </summary>
        static Queue<Product> products = new Queue<Product>();
        static void GetProducts()
        {
            for (int i = 0; i < 100; i++)
            {
                Product product = new Product
                {
                    Name = i.ToString()
                };
                products.Enqueue(product);
            }

            while (products.Count > 0)
            {
               Console.WriteLine(products.Dequeue().Name);
            }
            Console.ReadLine();
        }


        private static void getstr()
        {
            string str = "123<56489>45<156>7>8<9";

            strtt(str);

        }
        private static string strtt(string  str)
        {
            try
            {
                while ((str.IndexOf("<") != -1 && str.IndexOf(">") != -1))
                {
                    string s1 = str.Substring(str.IndexOf("<"), str.IndexOf(">") - str.IndexOf("<") + 1);
                    str = str.Replace(s1, "");
                }
            }
            catch (Exception ex)
            {
                return str;
            }

            return str;
        }

    }

    class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int SellPrice { get; set; }
    }

}
