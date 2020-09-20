namespace BlogEngine6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class featuredBlog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeaturedBlogs",
                c => new
                {
                    FeaturedBlogID = c.Guid(nullable: false),
                    UserID = c.String(nullable: false, maxLength: 128),
                    BlogID = c.Int(nullable: false),
                    FeatureDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.FeaturedBlogID)
                .ForeignKey("dbo.Blogs", t => t.BlogID, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: false)
                .Index(t => t.UserID)
                .Index(t => t.BlogID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeaturedBlogs", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.FeaturedBlogs", "BlogID", "dbo.Blogs");
            DropIndex("dbo.FeaturedBlogs", new[] { "BlogID" });
            DropIndex("dbo.FeaturedBlogs", new[] { "UserID" });
            DropTable("dbo.FeaturedBlogs");
        }
    }
}
