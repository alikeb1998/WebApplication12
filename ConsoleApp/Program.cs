using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var t = new MyClass();

            var ser = JsonConvert.SerializeObject(t);

            Console.WriteLine(ser);

            var des = JsonConvert.DeserializeObject<MyClass>(ser);

            Console.WriteLine(des);


            t.id = 9;
            t.name = "assss";
            
            ser = JsonConvert.SerializeObject(t);
            Console.WriteLine(ser);

            des = JsonConvert.DeserializeObject<MyClass>(ser);
            Console.WriteLine(des);


            Console.ReadKey();
        }
    }

    public class MyClass
    {
        public int? id { get; set; }

        public string name { get; set; }
    }
}
