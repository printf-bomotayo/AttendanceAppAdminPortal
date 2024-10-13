using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.NotificationDTOs
{
    public class SendNotificationToCandidateDto
    {
        public int CandidateId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}