using System;
using System.Collections.Generic;
using System.Text;

namespace EcobinSellerService.Domain.Entities
{
    public class Seller : Base
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
    }

}
