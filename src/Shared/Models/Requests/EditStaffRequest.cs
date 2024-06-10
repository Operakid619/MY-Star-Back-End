using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Requests
{
    public class EditStaffRequest
    {
        [Required]
        public Guid StaffId { get; set; }

        public string LastName { get; set; } = string.Empty;

        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;


        public string PhoneNumber { get; set; } = string.Empty;
    }
}
