namespace AspNetIdentity.WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserOrders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderName = c.String(nullable: false, maxLength: 500),
                        CreationDate = c.DateTime(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256),
                        FirstName = c.String(maxLength: 30),
                        LastName = c.String(maxLength: 30),
                        Address = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserOrders", "UserId", "dbo.Users");
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.UserOrders", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserOrders");
        }
    }
}
