namespace AfincoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logintipo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Tipo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Tipo");
        }
    }
}
