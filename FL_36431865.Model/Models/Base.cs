using System.Collections.Generic; 

namespace TicketManagement.Model
{
    public class Base<T>
    {
        public T Data { get; set; }
        public List<T> Items { get; set; }
    }
}
 