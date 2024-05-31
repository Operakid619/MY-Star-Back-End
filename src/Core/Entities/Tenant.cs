using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; } = string.Empty;
        public virtual DateTime? Deleted { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public virtual DateTime Created { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual string? LastModifiedBy { get; set; }



        public string Name { get; set; }
        public string TenantAdminEmail { get; set; }
        public Tenant()
        {
            IsDeleted = false;
            Created = DateTime.UtcNow;
        }

    }
}
