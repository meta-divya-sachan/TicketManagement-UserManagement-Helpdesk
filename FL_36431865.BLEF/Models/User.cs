using System.Collections.Generic;

namespace TicketManagement.BLL
{
    public class User
    {
        public int accion { get; set; }
        public BLL.sUser usuario { get; set; } 
        public List<BLL.sRole> perfiles { get; set; }

        public User()
        {
            accion = 0;
            usuario = new BLL.sUser(); 
            perfiles = new List<sRole>();
        }
    }
}
