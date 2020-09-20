namespace BlogEngine6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blogFavorites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavoriteBlogs",
                c => new
                    {
                        FavoriteBlogID = c.Int(nullable: false, identity: true),
                        BlogID = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.FavoriteBlogID)
                .ForeignKey("dbo.Blogs", t => t.BlogID, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: false)
                .Index(t => t.BlogID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavoriteBlogs", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.FavoriteBlogs", "BlogID", "dbo.Blogs");
            DropIndex("dbo.FavoriteBlogs", new[] { "UserID" });
            DropIndex("dbo.FavoriteBlogs", new[] { "BlogID" });
            DropTable("dbo.FavoriteBlogs");
        }
    }
}
