using log4net;
using Generic.Domain.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Domain.BaseContext
{
    public class Context : DbContext, IDisposable
    {
        bool disposed = false;

        ILog Logger = log4net.LogManager.GetLogger("EntityFrameworkLog");

        public Context()
            : base("conn")
        {
            Database.SetInitializer<Context>(null);

            var enableLog = System.Configuration.ConfigurationManager.AppSettings["EnableEntityFrameworkLog"].ToString();

            if (enableLog == "1")
            {
                this.Database.Log = (message =>
                {
                    Logger.Info(message);
                });
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new StateMap());
            modelBuilder.Configurations.Add(new UserMap());
            base.OnModelCreating(modelBuilder);
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                disposed = true;
            }

        }
    }
}


