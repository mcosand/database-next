namespace Kcsara.Database.Data.Migrations
{
  using System;
  using System.Data.Entity.Migrations;

  public partial class UsersAndContacts : DbMigration
  {
    // Workaround for https://github.com/aspnet/dnx/issues/3023
    static UsersAndContacts()
    {
      System.Reflection.Assembly.Load(typeof(UsersAndContacts).Assembly.GetName());
    }

    public override void Up()
    {
      CreateTable(
          "dbo.MemberContactTypes",
          c => new
          {
            Id = c.Guid(nullable: false),
            CategoryLabel = c.String(),
            SubTypeLabel = c.String(),
            RegEx = c.String(),
            LastChanged = c.DateTime(nullable: false),
            ChangedBy = c.String(),
          })
          .PrimaryKey(t => t.Id);

      CreateTable(
          "dbo.Members",
          c => new
          {
            Id = c.Guid(nullable: false),
            WorkerNumber = c.String(maxLength: 50),
            LastName = c.String(),
            FirstName = c.String(),
            BirthDate = c.DateTime(),
            Gender = c.Int(nullable: false),
            PhotoFile = c.String(),
            LastChanged = c.DateTime(nullable: false),
            ChangedBy = c.String(),
          })
          .PrimaryKey(t => t.Id);

      CreateTable(
          "dbo.MemberContacts",
          c => new
          {
            Id = c.Guid(nullable: false),
            TypeId = c.Guid(nullable: false),
            Value = c.String(),
            Priority = c.Int(nullable: false),
            MemberId = c.Guid(nullable: false),
            LastChanged = c.DateTime(nullable: false),
            ChangedBy = c.String(),
          })
          .PrimaryKey(t => t.Id)
          .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
          .ForeignKey("dbo.MemberContactTypes", t => t.TypeId, cascadeDelete: true)
          .Index(t => t.TypeId)
          .Index(t => t.MemberId);

    }

    public override void Down()
    {
      DropForeignKey("dbo.MemberContacts", "TypeId", "dbo.MemberContactTypes");
      DropForeignKey("dbo.MemberContacts", "MemberId", "dbo.Members");
      DropIndex("dbo.MemberContacts", new[] { "MemberId" });
      DropIndex("dbo.MemberContacts", new[] { "TypeId" });
      DropTable("dbo.MemberContacts");
      DropTable("dbo.Members");
      DropTable("dbo.MemberContactTypes");
    }
  }
}
