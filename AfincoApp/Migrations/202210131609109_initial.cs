namespace AfincoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Balanco",
                c => new
                    {
                        BalancoID = c.Int(nullable: false, identity: true),
                        Ano = c.Int(nullable: false),
                        Periodo = c.Int(nullable: false),
                        ClienteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BalancoID)
                .ForeignKey("dbo.Cliente", t => t.ClienteID, cascadeDelete: true)
                .Index(t => t.ClienteID);
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        ClienteID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.ClienteID);
            
            CreateTable(
                "dbo.Movimentacao",
                c => new
                    {
                        MovimentacaoID = c.Int(nullable: false, identity: true),
                        Tipo = c.Int(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalancoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MovimentacaoID)
                .ForeignKey("dbo.Balanco", t => t.BalancoID, cascadeDelete: true)
                .Index(t => t.BalancoID);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        UsuarioID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Sobrenome = c.String(),
                        Login = c.String(),
                        Senha = c.String(),
                    })
                .PrimaryKey(t => t.UsuarioID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movimentacao", "BalancoID", "dbo.Balanco");
            DropForeignKey("dbo.Balanco", "ClienteID", "dbo.Cliente");
            DropIndex("dbo.Movimentacao", new[] { "BalancoID" });
            DropIndex("dbo.Balanco", new[] { "ClienteID" });
            DropTable("dbo.Usuario");
            DropTable("dbo.Movimentacao");
            DropTable("dbo.Cliente");
            DropTable("dbo.Balanco");
        }
    }
}
