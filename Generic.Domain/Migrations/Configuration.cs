using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Generic.Domain.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Generic.Domain.BaseContext.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Generic.Domain.BaseContext.Context context)
        {
           
        }
    }
}
