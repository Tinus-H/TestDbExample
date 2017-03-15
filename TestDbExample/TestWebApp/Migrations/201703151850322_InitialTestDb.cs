namespace TestWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialTestDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IdentifierEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Version = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        ModifiedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SharedEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        EventType = c.String(),
                        EventCount = c.Int(nullable: false),
                        IdentifierEntityId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        Version = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        ModifiedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentifierEntities", t => t.IdentifierEntityId)
                .Index(t => t.IdentifierEntityId);
            
            CreateTable(
                "dbo.InheritedSharedEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GroupType = c.String(),
                        StringValue = c.String(),
                        ObjectType = c.String(maxLength: 128),
                        IntValue = c.Int(),
                        BoolValue = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SharedEntities", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InheritedSharedEntities", "Id", "dbo.SharedEntities");
            DropForeignKey("dbo.SharedEntities", "IdentifierEntityId", "dbo.IdentifierEntities");
            DropIndex("dbo.InheritedSharedEntities", new[] { "Id" });
            DropIndex("dbo.SharedEntities", new[] { "IdentifierEntityId" });
            DropTable("dbo.InheritedSharedEntities");
            DropTable("dbo.SharedEntities");
            DropTable("dbo.IdentifierEntities");
        }
    }
}
