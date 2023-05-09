

namespace TicketManagement.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class sUserAccess
    {
        public long accessID { get; set; }
        public string userCode { get; set; }
        public Nullable<System.DateTime> accessDate { get; set; }
        public string accessIP { get; set; }
        public Nullable<int> results { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string creationUser { get; set; }
        public Nullable<System.DateTime> changeDate { get; set; }
        public string changeUser { get; set; }
    }
}
