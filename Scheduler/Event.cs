using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Scheduler
{
    public class Event
    {
        public enum ReoccurType
        {
            DEFAULT
        }
        public string name { get; set; }
        public string desc { get; set; }
        public DateTime date { get; set; }
        public ReoccurType reoccurType { get; set; }
        public List<string> users { get; set; } = new List<string>();
        public List<string> groups { get; set; } = new List<string>();
    }
}