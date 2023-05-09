using System.IO;

namespace TicketManagement.Helpers
{
    public class JsonProvider
    {
        // Load Json
        public static string LoadJson(string p_archivo)
        {
            string json = "";
            try
            { 
                using (var sr = new StreamReader(p_archivo))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch
            {
                throw;
            }
            return json;
        }
    }
}