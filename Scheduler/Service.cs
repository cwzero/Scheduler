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
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Json;

namespace Scheduler
{
    public class Service<E>
    {
        public SchedulerService scheduler { get; set; } = SchedulerService.INSTANCE;

        public string path { get; set; }

        public Uri uri
        {
            get
            {
                return new Uri(scheduler.uri, path);
            }
        }

        public Service()
        {

        }

        public Service(string path) : this()
        {
            this.path = path;
        }

        public Service(SchedulerService scheduler) : this()
        {
            this.scheduler = scheduler;
        }

        public Service(string path, SchedulerService scheduler) : this(scheduler)
        {
            this.path = path;
        }

        public async Task<List<E>> getList()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    string content = await Task.Run(() => new StreamReader(stream).ReadToEndAsync());
                    return JsonConvert.DeserializeObject<List<E>>(content);
                }
            }
        }

        public async Task<E> getItem(string name)
        {
            var response = await scheduler.httpClient.GetAsync(new Uri(uri, name));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<E>(content);
            }
            return default(E);
        }

        public async Task<HttpResponseMessage> createItem(E item)
        {
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await scheduler.httpClient.PutAsync(uri, content);
        }
    }
}