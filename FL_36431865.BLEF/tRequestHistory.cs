

namespace TicketManagement.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tRequestHistory
    {
        public int historyID { get; set; }
        public int requestID { get; set; }
        public Nullable<int> assignedRole { get; set; }
        public string assignedUser { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
    }
}
