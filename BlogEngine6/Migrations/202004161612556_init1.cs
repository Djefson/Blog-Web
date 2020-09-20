namespace BlogEngine6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TagBlogs", "Tag_TagID", "dbo.Tags");
            DropForeignKey("dbo.TagBlogs", "Blog_BlogID", "dbo.Blogs");
            DropIndex("dbo.TagBlogs", new[] { "Tag_TagID" });
            DropIndex("dbo.TagBlogs", new[] { "Blog_BlogID" });
            AddColumn("dbo.Tags", "Blog_BlogID", c => c.Int());
            AddColumn("dbo.Tags", "Blog_BlogID1", c => c.Int());
            AlterColumn("dbo.Tags", "Name", c => c.String());
            CreateIndex("dbo.Tags", "Blog_BlogID1");
            AddForeignKey("dbo.Tags", "Blog_BlogID1", "dbo.Blogs", "BlogID");
            DropTable("dbo.TagBlogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TagBlogs",
                c => new
                    {
                        Tag_TagID = c.Int(nullable: false),
                        Blog_BlogID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagID, t.Blog_BlogID });
            
            DropForeignKey("dbo.Tags", "Blog_BlogID1", "dbo.Blogs");
            DropIndex("dbo.Tags", new[] { "Blog_BlogID1" });
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.Tags", "Blog_BlogID1");
            DropColumn("dbo.Tags", "Blog_BlogID");
            CreateIndex("dbo.TagBlogs", "Blog_BlogID");
            CreateIndex("dbo.TagBlogs", "Tag_TagID");
            AddForeignKey("dbo.TagBlogs", "Blog_BlogID", "dbo.Blogs", "BlogID", cascadeDelete: true);
            AddForeignKey("dbo.TagBlogs", "Tag_TagID", "dbo.Tags", "TagID", cascadeDelete: true);
        }
    }
}
