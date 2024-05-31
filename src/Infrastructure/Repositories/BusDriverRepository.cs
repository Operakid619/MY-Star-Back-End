using Core.Entities.Users;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Constants.StringConstants;

namespace Infrastructure.Repositories
{
    public class BusDriverRepository : IBusDriverRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<BusDriverRepository> _logger;
        public BusDriverRepository(AppDbContext dbContext, ILogger<BusDriverRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<Busdriver?> GetBusdriverByEmail(string email)
        {
            return await _dbContext.Busdrivers.Include(x => x.Bus).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<BaseResponse> EditBusdriver(Busdriver busdriver)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status200OK };

            var user = _dbContext.Users.FirstOrDefault(x => x.Id == busdriver.PersonaId);
            if (user is null)
            {
                response.Message = "User not found";
                response.Status = false;
                response.Code = ResponseCodes.Status404NotFound;
            }

            user.FirstName = busdriver.FirstName;
            user.LastName = busdriver.LastName;
            user.PhoneNumber = busdriver.PhoneNumber;

            _dbContext.Users.Update(user);

            _dbContext.Busdrivers.Update(busdriver);

            await _dbContext.SaveChangesAsync();
            return response;
        }

        public async Task<Busdriver?> GetBusdriverById(Guid id)
        {
            return await _dbContext.Busdrivers.Include(x => x.Bus).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Busdriver>> GetAllBusdrivers()
        {
            return await _dbContext.Busdrivers.AsNoTracking().ToListAsync();
        }

        public async Task<Busdriver> AddBusdriver(Busdriver busdriver)
        {
            await _dbContext.Busdrivers.AddAsync(busdriver);
            await _dbContext.SaveChangesAsync();
            return busdriver;
        }

        public async Task<BaseResponse> DeleteBusdriver(Guid id)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status200OK };

            var busdriver = await _dbContext.Busdrivers.FirstOrDefaultAsync(x => x.Id == id);
            if (busdriver is null)
            {
                response.Message = "Busdriver not found";
                response.Status = false;
                response.Code = ResponseCodes.Status404NotFound;
                return response;
            }

            _dbContext.Busdrivers.Remove(busdriver);

            var user = _dbContext.Users.FirstOrDefault(x => x.Id == busdriver.PersonaId);
            if (user is null)
            {
                response.Message = "User not found";
                response.Status = false;
                response.Code = ResponseCodes.Status404NotFound;
                return response;
            }
            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();
            return response;
        }
    }
}
