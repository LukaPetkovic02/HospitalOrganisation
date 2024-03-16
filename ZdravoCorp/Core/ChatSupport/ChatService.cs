using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.ChatSupport.Model;
using ZdravoCorp.Core.ChatSupport.Repository;

namespace ZdravoCorp.Core.ChatSupport
{
    public class ChatService
    {
        public IChatRepository ChatRepository { get; set; }
        public ChatService()
        {
            ChatRepository = new ChatRepository();
        }
        public void GetAllChats()
        {
            ChatRepository.GetAllChats();
        }
        public void AddChat(Chat chat)
        {
            ChatRepository.AddChat(chat);
        }
        public Chat FindChat(string doctorJmbg,string nurseJmbg)
        {
            return ChatRepository.FindChat(doctorJmbg, nurseJmbg);
        }
        public void Save()
        {
            ChatRepository.Save();
        }
    }
}
