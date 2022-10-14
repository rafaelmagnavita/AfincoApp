namespace AfincoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class confirmasenha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "ConfirmaSenha", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Usuario", "Senha", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuario", "Senha", c => c.String());
            DropColumn("dbo.Usuario", "ConfirmaSenha");
        }
    }
}
