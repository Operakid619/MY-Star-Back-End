using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Models.Responses;
using Shared.Models.Requests;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Constants.StringConstants;

namespace Core.Services
{
    public class BusDriverSevice : IBusDriverSevice
    {
        private readonly IBusDriverRepository _busDriverRepository;
        private readonly IBusRepository _busRepository;

        public BusDriverSevice(IBusDriverRepository busDriverRepository, IBusRepository busRepository)
        {
            _busDriverRepository = busDriverRepository;
            _busRepository = busRepository;
        }

        //delete busdriver method        //delete busdriver method
        public async Task<BaseResponse> DeleteBusdriver(Guid id)
        {
            return await _busDriverRepository.DeleteBusdriver(id);
        }

        //edit busdriver method
        public async Task<BaseResponse> EditBusdriver(EditBusdriverRequest request)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status200OK};
            var busdriver = await _busDriverRepository.GetBusdriverById(request.Id);
            if (busdriver == null)
            {
                return new BaseResponse()
                {
                    Status = false,
                    Message = "Busdriver not found",
                    Code = ResponseCodes.Status404NotFound,
                };
            }


            if (request.BusId is not null)
            {
                var bus = await _busRepository.GetBusAsync(request.BusId.Value);
                if (bus is null)
                {
                    response.Status = false;
                    response.Code = ResponseCodes.Status404NotFound;
                    response.Message = "Bus not found with id";
                    return response;
                }
            }


            busdriver.BusId = request.BusId ?? busdriver.BusId;
            busdriver.FirstName = request.FirstName ?? busdriver.FirstName;
            busdriver.LastName = request.LastName ?? busdriver.LastName;
            busdriver.PhoneNumber = request.PhoneNumber ?? busdriver.PhoneNumber;
            busdriver.Modified = DateTime.UtcNow;


            return await _busDriverRepository.EditBusdriver(busdriver);
        }
        //get all busdrivers method
        public async Task<ApiResponse<List<BusDriverResponse>>> GetAllBusdrivers()
        {
            var busdrivers = await _busDriverRepository.GetAllBusdrivers();
            return new ApiResponse<List<BusDriverResponse>>()
            {
                Data = busdrivers.Select(x => new BusDriverResponse()
                {
                    BusDriverId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    BusNumber = x.Bus.Number,
                }).ToList()
            };
        }
        //get busdriver by email method
        public async Task<ApiResponse<BusDriverResponse>> GetBusdriverByEmail(string email)
        {
            var busdriver = await _busDriverRepository.GetBusdriverByEmail(email);
            if (busdriver == null)
            {
                return new ApiResponse<BusDriverResponse>()
                {
                    Data = null,
                    Message = "Busdriver not found"
                };
            }
            return new ApiResponse<BusDriverResponse>()
            {
                Data = new BusDriverResponse()
                {
                    BusDriverId = busdriver.Id,
                    FirstName = busdriver.FirstName,
                    LastName = busdriver.LastName,
                    Email = busdriver.Email,
                    PhoneNumber = busdriver.PhoneNumber,
                    BusNumber = busdriver.Bus.Number,
                }
            };
        }
    }
}
