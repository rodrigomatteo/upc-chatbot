namespace FormBot.Models
{
    public class DialogFlowParameter
    {
        public Fields Fields { get; set; }
    }

    public class Fields
    {
        public Course Course { get; set; }
        public Assignment Assignment { get; set; }
    }

    public class Course
    {
        public int NullValue { get; set; }
        public float NumberValue { get; set; }
        public string StringValue { get; set; }
        public bool BoolValue { get; set; }
        public int KindCase { get; set; }
    }

    public class Assignment
    {
        public int NullValue { get; set; }
        public float NumberValue { get; set; }
        public string StringValue { get; set; }
        public bool BoolValue { get; set; }
        public int KindCase { get; set; }
    }

}