using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TicketManagement.Model
{
    public class MSBar2DChart
    {
        public SChart chart { get; set; }
        public List<SCategory> categories { get; set; }
        public List<SDataset> dataset { get; set; }
        //public List<STrendline> trendlines { get; set; }
        public MSBar2DChart()
        {
            categories = new List<SCategory>();
            dataset = new List<SDataset>();
            //trendlines = new List<STrendline>();   
        }
    }

    public class SChart
    {
        public string caption { get; set; }
        public string placevaluesinside { get; set; }
        public string showvalues { get; set; }
        public string plottooltext { get; set; }
        public string theme { get; set; }
    }

    // Category
    public class SCategory
    {
        public List<SLabel> category { get; set; }
        public SCategory()
        {
            category = new List<SLabel>();
        }
    }
    public class SLabel
    {
        public string label { get; set; }
        public SLabel()
        {
            label = "";
        }
    }

    // Dataset 
    public class SDataset
    {
        public string seriesname { get; set; }
        public List<SValue> data { get; set; }
        public SDataset()
        {
            seriesname = "";
            data = new List<SValue>();
        }
    }
    public class SValue
    {
        public string value { get; set; }
        public SValue()
        {
            value = "";
        }
    }

    // Trendline 
    public class STrendline
    { 
        public List<SLine> line { get; set; }
    }
    public class SLine
    {
        public string startvalue { get; set; }
        public string color { get; set; }
        public string thickness { get; set; }
        public string displayvalue { get; set; }
        public SLine()
        {
            startvalue = "";
            color = "";
            thickness = "";
            displayvalue = "";
        }
    }
}
