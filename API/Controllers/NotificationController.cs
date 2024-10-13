using API.DTOs.NotificationDTOs;
using API.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class NotificationController : BaseApiController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("sendToCandidate")]
        public async Task<IActionResult> SendNotificationToUser([FromBody] SendNotificationToCandidateDto dto)
        {
            await _notificationService.SendNotificationToUser(dto.CandidateId, dto.Title, dto.Message);
            return Ok();
        }

        [HttpPost("sendToGroup")]
        public async Task<IActionResult> SendNotificationToGroup([FromBody] SendNotificationToGroupDto dto)
        {
            await _notificationService.SendNotificationToGroup(dto.GroupId, dto.Title, dto.Message);
            return Ok();
        }

    }
}