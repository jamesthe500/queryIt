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

    public interface IRepository<T> : IDisposable
    {
        void Add(T newEntity);
        void Delete(T entity);
        T FindById(int id);
        IQueryable<T> FindAll();
        int Commit();
    }

    // class = force it to be a reference type
    // struct = force it to be a value type like int or double.
    // class, struct, or naming a class, e.g. "Person" have to come first, and there can be only one.
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
            // here some validation would be nice, but because we're using generics,
            // we can't do any type specific validation 
            // a solution is to force T to impliment interfaces
            // See Model.cs IEntity
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
