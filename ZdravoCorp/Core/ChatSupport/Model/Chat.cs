using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.ChatSupport.Model
{
    public class Chat
    {
        [JsonProperty("DoctorJmbg")]
        public string DoctorJMBG { get; set; }
        [JsonProperty("NurseJmbg")]
        public string NurseJMBG { get; set; }
        [JsonProperty("Messages")]
        public Dictionary<string, string> Messages { get; set; }


        public Chat(string doctorJMBG, string nurseJMBG)
        {
            DoctorJMBG = doctorJMBG;
            NurseJMBG = nurseJMBG;
            Messages = new Dictionary<string, string>();
        }


        public void SendMessage(string senderJMBG, string message)
        {
            Messages.Add(senderJMBG+"-"+DateTime.Now.ToString("HH:mm:ss"), message);
        }
    }
}
