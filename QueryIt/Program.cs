using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryIt
{
    class Program
    {
        static void Main(string[] args)
        {
            // tells the entity framework to drop and recreate the DB everytime the application runs.
            // simpler that way.
            Database.SetInitializer(new DropCreateDatabaseAlways<EmployeeDb>());

            // this is a repository for employees. 
            // it is created by constructing the concrete type SqlRepository<employee>
            // a SqlRepositry needs a DbContext instance. Gives enough info to create teh schema 
            // wrapped inusing because the object is disposable. 
            using (IRepository<Employee> employeeRepository = new SqlRepository<Employee>(new EmployeeDb()))
            {
                AddEmployees(employeeRepository);
                CountEmployees(employeeRepository);

            }
        }

        private static void CountEmployees(IRepository<Employee> employeeRepository)
        {
            Console.WriteLine(employeeRepository.FindAll().Count());
        }

        private static void AddEmployees(IRepository<Employee> employeeRepository)
        {
            employeeRepository.Add(new Employee { Name = "Scott" });
            employeeRepository.Add(new Employee { Name = "Joseph"});
            employeeRepository.Commit();        
        }
    }
}
