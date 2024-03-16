using System;
using System.Windows;
using ZdravoCorp.Core.ChatSupport;
using ZdravoCorp.Core.ChatSupport.Model;
using ZdravoCorp.Core.Utilities.Doctor;
using ZdravoCorp.Core.Utilities.Nurse;
using ZdravoCorp.Gui.ChatSupport.View;
using ZdravoCorp.Gui.ChatSupport.ViewModel;
using ZdravoCorp.Gui.Commands;

namespace ZdravoCorp.Gui.ChatSupport.Command
{
    public class SelectChatCommand : BaseCommand
    {
        private ChatSupportViewModel _chatSupportViewModel;
        private ChatSupportView _chatSupportView;
        private ChatService _chatService;
        private string _jmbgOfLogged;
        public event EventHandler<bool> SelectChat;
        public SelectChatCommand(ChatSupportView chatSupportView,ChatSupportViewModel chatSupportViewModel, string jmbgOfLogged, ChatService chatService)
        {
            _chatSupportView = chatSupportView;
            _chatSupportViewModel = chatSupportViewModel;
            _chatService = chatService;
            _jmbgOfLogged = jmbgOfLogged;
        }
        public bool CanExecute(object? parameter)
        {
            if (_chatSupportViewModel.SelectedChat == "")
                return false;
            return true;
        }
        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                DoctorService doctorService = new DoctorService();
                NurseService nurseService = new NurseService();
                if (doctorService.FindByJmbg(_jmbgOfLogged) != null)
                {
                    string selected = _chatSupportViewModel.SelectedChat.Split(" ")[0];
                    Chat chat = _chatService.FindChat(_jmbgOfLogged, selected);
                    if (chat == null)
                    {
                        chat = new Chat(_jmbgOfLogged, selected);
                        _chatService.AddChat(chat);
                        MessageBox.Show("Added new chat");
                    }
                    ShowChat(chat);
                }
                else
                {
                    string selected = _chatSupportViewModel.SelectedChat.Split(" ")[0];
                    Chat chat = _chatService.FindChat(selected,_jmbgOfLogged);
                    if (chat == null)
                    {
                        chat = new Chat(selected,_jmbgOfLogged);
                        _chatService.AddChat(chat);
                        MessageBox.Show("Added new chat");
                    }
                    ShowChat(chat);
                }

                SelectChat?.Invoke(this, true);
            }
            else
            {
                MessageBox.Show("You didn't select a chat!");
                SelectChat?.Invoke(this, false);
            }
        }
        
        public void ShowChat(Chat chat)
        {
            _chatSupportView.chatListBox.Items.Clear();
            _chatSupportViewModel.Messages.Clear();
            foreach (var message in chat.Messages)
            {
                _chatSupportViewModel.Messages.Add(message.Key.Split("-")[0] + " at " + message.Key.Split("-")[1] + " : " + message.Value);
                _chatSupportView.chatListBox.Items.Add(message.Key.Split("-")[0] + " at " + message.Key.Split("-")[1] + " : " + message.Value);
            }
            _chatSupportView.chatListBox.Items.Refresh();
        }
    }
}
