//------------------------------------------------------------------------------
// <auto-generated>
//     
// </auto-generated>
//------------------------------------------------------------------------------

namespace TicketManagement.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class sUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sUser()
        {
            this.sUserRole = new HashSet<sUserRole>();
        }
    
        public string userCode { get; set; }
        public string userName { get; set; }
        public Nullable<int> docType { get; set; }
        public string docNumber { get; set; }
        public string userCodeAD { get; set; }
        public string zone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string guidImage1 { get; set; }
        public string guidImage2 { get; set; }
        public Nullable<System.DateTime> lastAccessDate { get; set; }
        public string lastAccessIP { get; set; }
        public Nullable<System.DateTime> joinDate { get; set; }
        public Nullable<bool> hasExpiration { get; set; }
        public Nullable<System.DateTime> expiracyDate { get; set; }
        public Nullable<int> errorCounter { get; set; }
        public Nullable<System.DateTime> lockDate { get; set; }
        public Nullable<System.DateTime> unlockDate { get; set; }
        public string unlockUser { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string creationUser { get; set; }
        public Nullable<System.DateTime> changeDate { get; set; }
        public string changeUser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sUserRole> sUserRole { get; set; }
    }
}