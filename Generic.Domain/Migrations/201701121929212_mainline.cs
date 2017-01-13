namespace Generic.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mainline : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StateId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.State", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UF = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PicturePath = c.String(),
                        RoleId = c.Int(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        DtRegister = c.DateTime(),
                        IsEnabled = c.Boolean(nullable: false),
                        City = c.String(),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "RoleId", "dbo.Role");
            DropForeignKey("dbo.City", "StateId", "dbo.State");
            DropIndex("dbo.User", new[] { "RoleId" });
            DropIndex("dbo.City", new[] { "StateId" });
            DropTable("dbo.User");
            DropTable("dbo.State");
            DropTable("dbo.City");
            DropTable("dbo.Role");
        }
    }
}
