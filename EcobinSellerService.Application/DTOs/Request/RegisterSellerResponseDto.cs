using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcobinSellerService.Application.DTOs.Request
{
    public class RegisterSellerResponseDto
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
    }
}
