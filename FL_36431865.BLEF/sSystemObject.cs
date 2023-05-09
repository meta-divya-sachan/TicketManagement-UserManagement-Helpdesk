

namespace TicketManagement.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class sSystemObject
    {
        public int objectID { get; set; }
        public string objectName { get; set; }
        public Nullable<int> parentObjectID { get; set; }
        public Nullable<int> sequence { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string creationUser { get; set; }
        public Nullable<System.DateTime> changeDate { get; set; }
        public string changeUser { get; set; }
    }
}
