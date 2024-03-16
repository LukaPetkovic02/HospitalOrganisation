using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ZdravoCorp.Core.ChatSupport.Model;

namespace ZdravoCorp.Core.ChatSupport.Repository
{
    public class ChatRepository : IChatRepository
    {
        public List<Chat> Chats;

        public ChatRepository()
        {
            GetAllChats();
        }

        public void GetAllChats()
        {
            Chats = JsonConvert.DeserializeObject<List<Chat>>(File.ReadAllText("../../../Data/chats.json"));
        }
        public Chat FindChat(string doctorJmbg,string nurseJmbg)
        {
            foreach(Chat chat in Chats)
            {
                if (chat.DoctorJMBG == doctorJmbg && chat.NurseJMBG == nurseJmbg)
                    return chat;
            }
            return null;
        }
        public void AddChat(Chat chat)
        {
            Chats.Add(chat);
            Save();
        }
        public void Save()
        {
            File.WriteAllText("../../../Data/chats.json", JsonConvert.SerializeObject(Chats, Formatting.Indented));
        }
    }
}
