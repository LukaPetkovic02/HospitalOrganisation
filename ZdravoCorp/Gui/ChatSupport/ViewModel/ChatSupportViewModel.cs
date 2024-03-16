using System.Collections.Generic;
using System.Windows.Input;
using ZdravoCorp.Core.ChatSupport;
using ZdravoCorp.Gui.ChatSupport.Command;
using ZdravoCorp.Gui.ChatSupport.View;
using ZdravoCorp.Gui.Commands;
using ZdravoCorp.Gui.VacationRequest.ViewModel;
using ZdravoCorp.Gui.ViewModel;

namespace ZdravoCorp.Gui.ChatSupport.ViewModel
{
    
    public class ChatSupportViewModel : BaseViewModel
    {
        public string SelectedChat { get; set; }
        public string Message { get; set; }
        public List<string> Messages { get; set; }
        public ICommand SelectChatCommand { get; }
        public ICommand SendMessageCommand { get; }
        public ChatSupportViewModel(ChatSupportView chatSupportView,string jmbgOfLogged)
        {
            SelectedChat = "";
            Message = "";
            Messages = new List<string>();
            ChatService chatService = new ChatService();
            SelectChatCommand = new SelectChatCommand(chatSupportView,this,jmbgOfLogged,chatService);
            SendMessageCommand = new SendMessageCommand(chatSupportView, this, jmbgOfLogged,chatService);
        }
    }
}
