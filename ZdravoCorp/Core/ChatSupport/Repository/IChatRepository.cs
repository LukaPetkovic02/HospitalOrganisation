using ZdravoCorp.Core.ChatSupport.Model;

namespace ZdravoCorp.Core.ChatSupport.Repository
{
    public interface IChatRepository
    {
        void GetAllChats();
        Chat FindChat(string doctorJmbg, string nurseJmbg);
        void AddChat(Chat chat);
        void Save();
    }
}
