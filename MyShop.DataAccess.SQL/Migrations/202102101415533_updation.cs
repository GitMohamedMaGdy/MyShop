namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updation : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BasketItems", new[] { "Basket_Id" });
            DropColumn("dbo.BasketItems", "BasketId");
            RenameColumn(table: "dbo.BasketItems", name: "Basket_Id", newName: "BasketId");
            AlterColumn("dbo.BasketItems", "BasketId", c => c.String(maxLength: 128));
            AlterColumn("dbo.BasketItems", "ProductId", c => c.String());
            CreateIndex("dbo.BasketItems", "BasketId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BasketItems", new[] { "BasketId" });
            AlterColumn("dbo.BasketItems", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.BasketItems", "BasketId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.BasketItems", name: "BasketId", newName: "Basket_Id");
            AddColumn("dbo.BasketItems", "BasketId", c => c.Int(nullable: false));
            CreateIndex("dbo.BasketItems", "Basket_Id");
        }
    }
}
