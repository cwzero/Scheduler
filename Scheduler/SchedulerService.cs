using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Scheduler
{
    public class SchedulerService
    {
        public static SchedulerService INSTANCE { get; } = new SchedulerService();
        private static string HOST { get; } = "172.16.0.98";

        public enum Environment
        {
            DEVELOPMENT, TESTING, PRODUCTION
        }

        public HttpClient httpClient { get; set; } = new HttpClient();
        public Environment environment { get; set; } = Environment.DEVELOPMENT;
        public Service<Event> eventService { get; set; } = new Service<Event>("events");
        public Service<Group> groupService { get; set; } = new Service<Group>("groups");
        public Service<User> userService { get; set; } = new Service<User>("users");

        public Uri uri
        {
            get
            {

                switch (environment)
                {
                    default:
                    case Environment.DEVELOPMENT:
                        return new Uri("http://" + HOST + ":8080/scheduler/api/");
                    case Environment.PRODUCTION:
                        return new Uri("http://scheduler.liquidforte.com/api/");
                    case Environment.TESTING:
                        return new Uri("http://localhost:8080/scheduler/api/");
                }
            }
        }

        public SchedulerService() : this(Environment.DEVELOPMENT)
        {

        }

        public SchedulerService(Environment environment)
        {
            this.environment = environment;
        }
    }
}