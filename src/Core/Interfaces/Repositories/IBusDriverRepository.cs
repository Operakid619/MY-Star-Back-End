using Core.Entities.Users;
using Microsoft.Extensions.Logging;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IBusDriverRepository
    {
        public Task<Busdriver?> GetBusdriverByEmail(string email);
        public Task<BaseResponse> EditBusdriver(Busdriver busdriver);
        public Task<Busdriver?> GetBusdriverById(Guid id);
        public Task<List<Busdriver>> GetAllBusdrivers();
        public Task<Busdriver> AddBusdriver(Busdriver busdriver);
        public Task<BaseResponse> DeleteBusdriver(Guid id);

    }
}
