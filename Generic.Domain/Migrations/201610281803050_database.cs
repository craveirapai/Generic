namespace Tonnus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Name", c => c.String());
            AddColumn("dbo.User", "Phone", c => c.String());
            AddColumn("dbo.User", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "IsActive");
            DropColumn("dbo.User", "Phone");
            DropColumn("dbo.User", "Name");
        }
    }
}
