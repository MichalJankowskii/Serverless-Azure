namespace SubmitToEventGrid
{
    using System;

    public class Event
    {
        public string id { get; set; }
        public DateTime eventTime { get; set; }
        public string eventType { get; set; }
        public string subject { get; set; }
        public object data { get; set; }
    }
}
