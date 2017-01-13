using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Generic.Domain.BaseContext
{
    public class BaseRepository<T> : Context where T : class
    {
        public DbSet<T> DbSet
        {
            get;
            set;
        }

        protected virtual void SetModified(object model, EntityState state)
        {
            ((IObjectContextAdapter)this)
                            .ObjectContext
                            .ObjectStateManager
                            .ChangeObjectState(model,
                            state);
        }

        protected virtual void Detach(object model)
        {

            ((IObjectContextAdapter)this).ObjectContext.Detach(model);

        }

        public int Save(T model)
        {
            this.DbSet.Add(model);
            return this.SaveChanges();
        }

        public virtual int Update(T model)
        {
            try
            {
                var entry = this.Entry(model);

                if (entry.State == EntityState.Detached)
                {
                    this.DbSet.Attach(model);
                }
            }
            catch
            {
                this.DbSet.Add(model);
            }

            this.SetModified(model, EntityState.Modified);
            return this.SaveChanges();
        }

        public virtual void Delete(T model)
        {
            var entry = this.Entry(model);

            if (entry.State == EntityState.Detached)
                this.DbSet.Attach(model);

            this.SetModified(model, EntityState.Deleted);
            this.SaveChanges();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.DbSet.ToList();
        }

        public virtual T GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual IEnumerable<T> Where(Expression<Func<T, bool>> expression)
        {
            return this.DbSet.Where(expression);
        }

        public IEnumerable<T> OrderBy(Expression<Func<T, bool>> expression)
        {
            return this.DbSet.OrderBy(expression);
        }

        public IEnumerable<T> GetAll(int? pageSize)
        {
            if (pageSize.HasValue)
            {
                return (from x in this.DbSet
                        select x).Take(pageSize.Value).ToList();
            }

            return this.GetAll();
        }

        protected virtual T FindOne(Expression<Func<T, bool>> Criteria)
        {
            return (T)this.DbSet.Where(Criteria).FirstOrDefault();
        }

    }
}
