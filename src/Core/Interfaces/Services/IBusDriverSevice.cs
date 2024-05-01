using Shared.Models.Requests;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IBusDriverSevice
    {
        public Task<BaseResponse> DeleteBusdriver(Guid id);
        public Task<BaseResponse> EditBusdriver(EditBusdriverRequest request);
    }
}
