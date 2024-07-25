using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        public NotificationService(AppDbContext context)
        {
            _context = context;

        }

        public async Task SendNotificationToUser(int candidateId, string title, string message)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                CreatedDate = DateTime.UtcNow,
                CandidateId = candidateId,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task SendNotificationToGroup(int groupId, string title, string message)
        {
            var group = await _context.Groups.Include(g => g.Candidates).FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
            {
                throw new InvalidOperationException("Group not found.");
            }

            foreach (var candidate in group.Candidates)
            {
                var notification = new Notification
                {
                    Title = title,
                    Message = message,
                    CreatedDate = DateTime.UtcNow,
                    CandidateId = candidate.Id,
                    GroupId = groupId,
                    IsRead = false
                };

                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync();
        }
    }
}