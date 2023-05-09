using TicketManagement.Model;
using System.Collections.Generic;

namespace TicketManagement.BLL
{
    public class Request
    {
        public int action { get; set; }
        public BLL.tRequest request { get; set; } 
        public List<BLL.tRequestComment> comments { get; set; }

        public Request()
        {
            action = 0;
            request = new BLL.tRequest();
            comments = new List<BLL.tRequestComment>();
        }
    }
}
