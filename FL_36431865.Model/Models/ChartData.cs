namespace TicketManagement.Model
{
    public class ChartData
    {
        public string label { get; set; }
        public string serieName { get; set; }
        public string value { get; set; }

        public ChartData()
        {
            label = "";
            serieName = "";
            value = "";
        }
    }
}
