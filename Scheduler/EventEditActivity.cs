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
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Scheduler
{
    [Activity(Label = "EventEditActivity")]
    public class EventEditActivity : Activity
    {
        public static Event current { get; set; }
        private EditText editText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            editText = FindViewById<EditText>(Resource.Id.edit_event_date);

            editText.Click += (sender, e) =>
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };

            Button saveButton = FindViewById<Button>(Resource.Id.event_save_button);
            saveButton.Click += createEvent;
        }

        public async void createEvent(object sender, EventArgs e)
        {
            EditText editName = FindViewById<EditText>(Resource.Id.edit_event_name);
            EditText editDesc = FindViewById<EditText>(Resource.Id.edit_event_desc);
            EditText editDate = FindViewById<EditText>(Resource.Id.edit_event_date);

            Event newEvent = new Event();
            newEvent.name = editName.Text;
            newEvent.desc = editDesc.Text;
            newEvent.date = new DateTime(long.Parse(editDate.Text));

            await createEvent(current, newEvent);
        }

        public static async Task createEvent(Event oldEvent, Event newEvent)
        {
            string addUri = "";
            if (!string.IsNullOrWhiteSpace(oldEvent.name))
            {
                addUri = oldEvent.name;
            }
            Uri uri = new Uri("http://172.16.0.98:8080/scheduler/api/events/" + addUri);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(requestStream))
                {
                    string body = JsonConvert.SerializeObject(newEvent);
                    writer.WriteLine(body);
                    writer.Flush();
                }
            }

            request.ContentType = "application/json";
            request.Method = "POST";

            using (WebResponse response = await request.GetResponseAsync())
            {
            }
        }

        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            editText.Text = e.Date.ToLongDateString();
        }
    }
}