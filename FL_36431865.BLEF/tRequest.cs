

namespace TicketManagement.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tRequest
    {
        public int requestID { get; set; }
        public string requestTitle { get; set; }
        public string description { get; set; }
        public Nullable<int> requestType { get; set; }
        public string location { get; set; }
        public Nullable<int> assignedRole { get; set; }
        public string assignedUser { get; set; }
        public Nullable<int> situation { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string creationUser { get; set; }
        public Nullable<System.DateTime> changeDate { get; set; }
        public string changeUser { get; set; }

        // Output
        public string requestTypeName { get; set; }
        public string situationName { get; set; }
        public string assignedUserName { get; set; }
    }
}
