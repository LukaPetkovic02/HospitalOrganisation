using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Notifications.Model
{
    public class Notification
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("PersonJmbg")]
        public string PersonJmbg { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("NotificationDate")]
        public DateTime NotificationDate { get; set; }
        [JsonConstructor]
        public Notification(string id,string personJmbg, string message, DateTime notificationDate)
        {
            Id = id;
            PersonJmbg = personJmbg;
            Message = message;
            NotificationDate = notificationDate;
        }

    }
}
