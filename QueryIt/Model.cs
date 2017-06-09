using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryIt
{
    public class Person
    {
        public string Name { get; set; }
    }

    public class Employee : Person
    {
        public int Id { get; set; }
        public virtual void DoWork()
        {
            Console.WriteLine("Doing real work");
        }

    }

    public class Manager : Employee
    {
        public override void DoWork()
        {
            Console.WriteLine("Createing a meeting");
        }
    }
}
