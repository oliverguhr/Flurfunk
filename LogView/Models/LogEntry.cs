using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogView.Models
{
    public class LogEntry
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}