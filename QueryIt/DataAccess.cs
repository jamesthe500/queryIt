using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryIt
{
    public class EmployeeDb : DbContext
    {
        public DbSet<Employee> Employees { get; set; } 
    }

    // this interface is making it possible for ___ to be co-variant.
    // we want the method in Program, DumpPeople, to be able to handle IRepository<Person> as well as <Employee>
    // even though every Employee derives from a Person object, this isn't allowed by default.
    // If it's reading Persons, that's fine, but if it tries to write to a Person, 
    // it might use members unique to Employee objects. 
    // We've pulled the methods that return items of type T or IQueryable<T>
    public  interface IReadOnlyRepository<out T> : IDisposable
    {
        T FindById(int id);
        IQueryable<T> FindAll();

    }

    // this is not covaiant, but the one above is
    // so we add it to the inheritance list.
    public interface IRepository<T> : IReadOnlyRepository<T>, IDisposable
    {
        void Add(T newEntity);
        void Delete(T entity);
        int Commit();
    }

    
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity
    {
        DbContext _ctx;
        DbSet<T> _set; 
        public SqlRepository(DbContext ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<T>();
        }

        public void Add(T newEntity)
        {
            if (newEntity.IsValid())
            {
                _set.Add(newEntity);
            }
        }

        public int Commit()
        {
            return _ctx.SaveChanges();
        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public IQueryable<T> FindAll()
        {
            return _set;
        }

        public T FindById(int id)
        {
            return _set.Find(id);
        }
    }
}
