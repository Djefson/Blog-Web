namespace BlogEngine6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BadWords",
                c => new
                {
                    BadWordID = c.Int(nullable: false, identity: true),
                    Word = c.String(nullable: false),
                })
                .PrimaryKey(t => t.BadWordID);

        }
        
        public override void Down()
        {
            DropTable("dbo.BadWords");
            
        }
    }
}
