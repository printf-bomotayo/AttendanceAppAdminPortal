using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.AuthenticationDTOs
{
    public class PasswordResetDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }

    }
}