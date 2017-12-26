namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMembershipNames : DbMigration
    {
        public override void Up()
        {
            Sql("update MembershipTypes set name = 'Pay as You Go' where DurationInMonths = 0");
            Sql("update MembershipTypes set name = 'Monthly' where DurationInMonths = 1");
            Sql("update MembershipTypes set name = 'Quarterly' where DurationInMonths = 3");
            Sql("update MembershipTypes set name = 'Annual' where DurationInMonths = 12");
        }
        
        public override void Down()
        {
        }
    }
}
