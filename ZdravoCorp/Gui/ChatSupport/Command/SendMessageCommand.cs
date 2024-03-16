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
    public class SendMessageCommand : BaseCommand
    {
        private ChatSupportViewModel _chatSupportViewModel;
        private ChatSupportView _chatSupportView;
        private ChatService _chatService;
        private string _jmbgOfLogged;
        public event EventHandler<bool> SendMessage;
        public SendMessageCommand(ChatSupportView chatSupportView, ChatSupportViewModel chatSupportViewModel, string jmbgOfLogged, ChatService chatService)
        {
            _chatSupportView = chatSupportView;
            _chatSupportViewModel = chatSupportViewModel;
            _chatService = chatService;
            _jmbgOfLogged = jmbgOfLogged;
        }
        public bool CanExecute(object? parameter)
        {
            if (_chatSupportViewModel.Message == "")
                return false;
            return true;
        }
        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                DoctorService doctorService = new DoctorService();
                NurseService nurseService = new NurseService();
                string selected = _chatSupportViewModel.SelectedChat.Split(" ")[0];
                if (doctorService.FindByJmbg(_jmbgOfLogged) != null)
                {
                    Chat chat = _chatService.FindChat(_jmbgOfLogged, selected);
                    chat.SendMessage(_jmbgOfLogged, _chatSupportViewModel.Message);
                    _chatSupportViewModel.Messages.Add(_chatSupportViewModel.Message);
                    _chatService.Save();
                    ShowChat(chat);
                }
                else
                {
                    Chat chat = _chatService.FindChat(selected, _jmbgOfLogged);
                    chat.SendMessage(_jmbgOfLogged,_chatSupportViewModel.Message);
                    _chatSupportViewModel.Messages.Add(_chatSupportViewModel.Message);
                    _chatService.Save();
                    ShowChat(chat);
                }
                _chatSupportViewModel.Message = "";
                _chatSupportView.messageText.Text = "";
                SendMessage?.Invoke(this, true);
            }
            else
            {
                MessageBox.Show("Message can't be empty!");
                SendMessage?.Invoke(this, false);
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
