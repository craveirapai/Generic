namespace Tonnus.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class database2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TotalClasses = c.Int(nullable: false),
                        DtPurchase = c.DateTime(nullable: false),
                        TotalPurchase = c.Double(nullable: false),
                        IdTransaction = c.Int(nullable: false),
                        PersonalId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personal", t => t.PersonalId)
                .ForeignKey("dbo.Student", t => t.StudentId)
                .Index(t => t.PersonalId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "StudentId", "dbo.Student");
            DropForeignKey("dbo.Payments", "PersonalId", "dbo.Personal");
            DropIndex("dbo.Payments", new[] { "StudentId" });
            DropIndex("dbo.Payments", new[] { "PersonalId" });
            DropTable("dbo.Payments");
        }
    }
}
