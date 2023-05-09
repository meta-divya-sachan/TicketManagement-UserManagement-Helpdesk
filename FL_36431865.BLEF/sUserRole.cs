

namespace TicketManagement.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class sUserRole
    {
        public string userCode { get; set; }
        public int roleID { get; set; }
        public Nullable<System.DateTime> beginDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string creationUser { get; set; }
        public Nullable<System.DateTime> changeDate { get; set; }
        public string changeUser { get; set; }
    
        public virtual sRole sRole { get; set; }
        public virtual sUser sUser { get; set; }
    }
}
