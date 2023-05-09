

namespace TicketManagement.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tRequestComment
    {
        public int commentID { get; set; }
        public Nullable<int> requestID { get; set; }
        public string comment { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string creationUser { get; set; }
        public Nullable<System.DateTime> changeDate { get; set; }
        public string changeUser { get; set; }

        // Output
        public string creationDateT { get; set; }
        public string creationUserName { get; set; }
    }
}
