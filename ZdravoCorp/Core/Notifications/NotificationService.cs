using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Notifications.Model;
using ZdravoCorp.Core.Notifications.Repository;
using ZdravoCorp.Core.Utilities.Patient;
using ZdravoCorp.Core.Utilities.Patient.Model;

namespace ZdravoCorp.Core.Notifications
{
    public class NotificationService
    {
        public NotificationRepository NotificationRepository;

        public NotificationService()
        {
            NotificationRepository = new NotificationRepository();
        }
        
        public List<Notification> Notifications()
        {
            return NotificationRepository.Notifications;
        }
        public void CreateNotification(string jmbg, string message,DateTime notificationDate)
        {
            NotificationRepository.CreateNotification(jmbg, message,notificationDate);
        }
        public void DeleteNotification(string id)
        {
            NotificationRepository.DeleteNotification(id);
        }
        public void DeletePastNotifications()
        {
            List<string> notificationsToDelete = new List<string>();
            foreach (Notification notification in NotificationRepository.Notifications)
            {
                if (DateTime.Now > notification.NotificationDate)
                {
                    notificationsToDelete.Add(notification.Id);

                }
            }
            foreach (string notification in notificationsToDelete)
                DeleteNotification(notification);
        }
        public void CheckForNotifications(string jmbg)
        {
            DeletePastNotifications();
            List<string> notificationsToDelete = new List<string>();
            foreach (Notification notification in NotificationRepository.Notifications)
            {
                if (notification.PersonJmbg == jmbg)
                {
                    ShowNotification(notification.Message);
                    notificationsToDelete.Add(notification.Id);
                }
            }
            foreach (string notification in notificationsToDelete)
                DeleteNotification(notification);
        }
        public void CheckForPatientNotifications(string jmbg)
        {
            DeletePastNotifications();
            List<string> notificationsToDelete = new List<string>();
            PatientService patientService = new PatientService();
            Patient patient = patientService.FindByJmbg(jmbg);
            foreach (Notification notification in NotificationRepository.Notifications)
            {
                if (notification.PersonJmbg == jmbg)
                {
                    if (DateTime.Now >= notification.NotificationDate.AddMinutes(-patient.NotificationTime.TotalMinutes))
                    {
                        ShowNotification(notification.Message);
                        notificationsToDelete.Add(notification.Id);
                    }
                }
            }
            foreach (string notification in notificationsToDelete)
                DeleteNotification(notification);
        }
        public void ShowNotification(string message)
        {
            NotificationRepository.ShowNotification(message);
        }
        public void Save()
        {
            NotificationRepository.Save();
        }
    }
}
