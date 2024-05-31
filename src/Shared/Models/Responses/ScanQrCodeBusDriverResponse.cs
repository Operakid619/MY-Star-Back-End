using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Responses
{
    public record ScanQrCodeBusDriverResponse
    {
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public string BusNumber { get; set; } = string.Empty;
        public DateTime InTimer { get; set; }
        public DateTime OutTimer { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
