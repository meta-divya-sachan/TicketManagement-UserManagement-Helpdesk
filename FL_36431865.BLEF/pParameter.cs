

namespace TicketManagement.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class pParameter
    {
        public int parameterID { get; set; }
        public int itemID { get; set; }
        public string value { get; set; }
        public string text { get; set; }
        public string auxiliar1 { get; set; }
        public Nullable<bool> auxiliar1IsMandatory { get; set; }
        public Nullable<int> auxiliar1Type { get; set; }
        public Nullable<int> auxiliar1Size { get; set; }
        public string auxiliar1Name { get; set; }
        public string auxiliar1Help { get; set; }
        public string auxiliar2 { get; set; }
        public Nullable<bool> auxiliar2IsMandatory { get; set; }
        public Nullable<int> auxiliar2Type { get; set; }
        public Nullable<int> auxiliar2Size { get; set; }
        public string auxiliar2Name { get; set; }
        public string auxiliar2Help { get; set; }
        public string auxiliar3 { get; set; }
        public Nullable<bool> auxiliar3IsMandatory { get; set; }
        public Nullable<int> auxiliar3Type { get; set; }
        public Nullable<int> auxiliar3Size { get; set; }
        public string auxiliar3Name { get; set; }
        public string auxiliar3Help { get; set; }
        public string auxiliar4 { get; set; }
        public Nullable<bool> auxiliar4IsMandatory { get; set; }
        public Nullable<int> auxiliar4Type { get; set; }
        public Nullable<int> auxiliar4Size { get; set; }
        public string auxiliar4Name { get; set; }
        public string auxiliar4Help { get; set; }
        public string auxiliar5 { get; set; }
        public Nullable<bool> auxiliar5IsMandatory { get; set; }
        public Nullable<int> auxiliar5Type { get; set; }
        public Nullable<int> auxiliar5Size { get; set; }
        public string auxiliar5Name { get; set; }
        public string auxiliar5Help { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string creationUser { get; set; }
        public Nullable<System.DateTime> changeDate { get; set; }
        public string changeUser { get; set; }
    }
}
