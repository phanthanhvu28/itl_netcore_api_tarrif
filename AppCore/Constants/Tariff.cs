
namespace AppCore.Constants
{
    public struct Tariff
    {
        public struct Status
        {           
            public const string Temp = "Temp";
            public const string Draft = "Draft";
            public const string Submit = "PendingApproval";
            public const string Active = "Active";
            public const string Inactive = "Inactive";
            public const string Finish = "Approved";
            public const string ReSubmit = "Amending";
            public const string Decline = "Declined";
        }
    }
}
