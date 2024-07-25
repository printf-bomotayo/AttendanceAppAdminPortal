using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.NotificationService
{
    public interface INotificationService
    {
        Task SendNotificationToUser(int userId, string title, string message);
        Task SendNotificationToGroup(int groupId, string title, string message);
    }
}