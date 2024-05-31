using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Enums;
using Shared.Models.Requests;
using Shared.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace API.Controllers.SPE
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly SignInManager<Persona> _signInManager;
        private readonly UserManager<Persona> _userManager;
        private readonly ILogger<SignupController> _logger;

        public SignupController(AppDbContext dbContext, SignInManager<Persona> signInManager,
            UserManager<Persona> userManager, ILogger<SignupController> logger)
        {
            _dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [SwaggerOperation(
          Summary = "Create a new Tenant School Endpoint",
          Description = "This endpoint creates a new Tenant. It requires Admin privilege",
          OperationId = "tenant.create",
          Tags = new[] { "SignUpEndpoints" })
        ]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        [HttpPost("create-tenant")]
        public async Task<IActionResult> CreateTenant(CreateTenantRequest request)
        {
            _logger.LogInformation("Creating a new tenant with name: {0}", request.SchoolName);

            var response = new BaseResponse() { Code = 201 };

            if (!string.Equals(request.Password, request.ConfirmPassword))
            {
                response.Status = false;
                response.Code = StatusCodes.Status400BadRequest;
                response.Message = "Password Field not the same as that of ConfirmPassword field";
                return BadRequest(response);
            }

            //var tenantExist = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.TenantAdminEmail == request.Email);
            //if (tenantExist is not null)
            //{
            //    response.Code = StatusCodes.Status400BadRequest;
            //    response.Status = false;
            //    response.Message = "Tenant already exist";
            //    return BadRequest(response);
            //}

            var tenant = new Tenant()
            {
                Id = Guid.NewGuid(),
                Name = request.SchoolName,
                TenantAdminEmail = request.Email,
            };
            await _dbContext.Tenants.AddAsync(tenant);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result is false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new { message = "Unable to register school"});
            }
            _logger.LogInformation("Successfully created a new tenant with name: {0}", request.SchoolName);


            _logger.LogInformation("Creating a new tenant admin with tenant-name: {0} and admins name: {1}", request.SchoolName, request.Firstname + " " + request.Lastname);
            var user = new Persona() {  Id = Guid.NewGuid(), UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber, FirstName = request.Firstname, LastName = request.Lastname, EmailConfirmed = true, PesonaType = PersonaType.Admin };
            //var user = new Persona() { TenantKey = tenant.Id.ToString(), Id = Guid.NewGuid(), UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber, FirstName = request.Firstname, LastName = request.Lastname, EmailConfirmed = true, PesonaType = PersonaType.Admin };
            var creationResult = await _userManager.CreateAsync(user, request.Password);
            if (!creationResult.Succeeded)
            {
                response.Code = StatusCodes.Status400BadRequest;
                response.Status = false;
                response.Message = string.Join(',', creationResult.Errors.Select(a => a.Description));
                _logger.LogInformation("Unable to create tenant admin for tenant {0} with the following error {1}",request.SchoolName, response.Message);
                return BadRequest(response);
            }
            _logger.LogInformation("Tenant admin Creation is successful");


            var roleResult = await _userManager.AddToRoleAsync(user, AuthConstants.Roles.ADMIN);
            if (!roleResult.Succeeded)
            {
                _logger.LogInformation("Unable add tenant admin user to admin role.");
                response.Status = false;
                response.Message = "Unable add tenant admin to admin role.";
                response.Code = StatusCodes.Status500InternalServerError;
                return StatusCode(500, response);
            }


            response.Message = "Tenant School created successfully! You can login with your credentials";
            response.Status = true;

            return Ok(response);
        }
    }
}
