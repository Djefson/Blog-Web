namespace BlogEngine6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBlogComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogComments",
                c => new
                    {
                        BlogCommentID = c.Int(nullable: false, identity: true),
                        BlogID = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                        PostDate = c.DateTime(nullable: false),
                        Message = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.BlogCommentID)
                .ForeignKey("dbo.Blogs", t => t.BlogID, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: false)
                .Index(t => t.BlogID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogComments", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.BlogComments", "BlogID", "dbo.Blogs");
            DropIndex("dbo.BlogComments", new[] { "UserID" });
            DropIndex("dbo.BlogComments", new[] { "BlogID" });
            DropTable("dbo.BlogComments");
        }
    }
}
