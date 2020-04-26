using Default;
using System;
using System.Linq;

namespace ODataConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceRoot = "https://localhost:44306/odata/";

            var context = new Container(new Uri(serviceRoot));

            var products = context.Products.ExecuteAsync().Result;

            products.ToList().ForEach(x =>
            {
                Console.WriteLine(x.Id + "-" + x.Name);
            });
            Console.ReadLine();
        }
    }
}