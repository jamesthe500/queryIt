using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryIt
{
    // this class would normally separate all these classes into separate files,
    // but for demo purposes, they're all here.

    public class EmployeeDb : DbContext
    {
        // using the DbContext methods
        // this is all that's needed. The DB will create the tables, etc.
        // we will be using this for managers as well, 
        // though normally we'd want to do more mapping to make sure it always works correctly
        public DbSet<Employee> Employees { get; set; } 
    }

    // we don't know where exactly this will be stored, this is a layer of abstration.
    // we do know that the we will want it to be clean-upable, so derive from IDisposable
    public interface IRepository<T> : IDisposable
    {
        // the methods we'll probably need.
        // keeping it abstract "entity" as we don't want to lock into just one thing.
        void Add(T newEntity);
        void Delete(T entity);
        T FindById(int id);
        IQueryable<T> FindAll();
        // here is where changes made get reconciled with the DB
        int Commit();
    }

    // this where statement, a generic Constraint says that a SqlRepository must be a class. 
    // This is necessary to make sure that DbSet can work. It can't handle all types.
    public class SqlRepository<T> : IRepository<T> where T : class
    {
        // this constructor is needed in order to do anything with the SQL DB
        // using DbContext rather than specific EmployeeDb as it can work with anything, and don't want to limit.
        DbContext _ctx;
        DbSet<T> _set; 
        public SqlRepository(DbContext ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<T>();
        }

        public void Add(T newEntity)
        {
            _set.Add(newEntity);
        }

        public int Commit()
        {
            // needs to return an int. Will do so as the number of records that were changed.
            return _ctx.SaveChanges();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public IQueryable<T> FindAll()
        {
            throw new NotImplementedException();
        }

        public T FindById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
