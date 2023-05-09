

namespace TicketManagement.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class sRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sRole()
        {
            this.sUserRole = new HashSet<sUserRole>();
        }
    
        public int roleID { get; set; }
        public string roleName { get; set; }
        public string description { get; set; }
        public Nullable<bool> isAdmin { get; set; }
        public Nullable<bool> isSuperadmin { get; set; }
        public string accesses { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string creationUser { get; set; }
        public Nullable<System.DateTime> changeDate { get; set; }
        public string changeUser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sUserRole> sUserRole { get; set; }
    }
}
