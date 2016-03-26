namespace TMWebRole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Chores",
                c => new
                    {
                        ChoreId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Note = c.String(unicode: false, storeType: "text"),
                        StartDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Reminder = c.DateTime(nullable: false),
                        Recurrence = c.Int(),
                        Priority = c.Int(),
                        Status = c.Int(),
                        Estimate = c.Int(nullable: false),
                        Location = c.String(),
                        Attachment = c.String(),
                        Category_CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ChoreId)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId)
                .Index(t => t.Category_CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chores", "Category_CategoryId", "dbo.Categories");
            DropIndex("dbo.Chores", new[] { "Category_CategoryId" });
            DropTable("dbo.Chores");
            DropTable("dbo.Categories");
        }
    }
}
