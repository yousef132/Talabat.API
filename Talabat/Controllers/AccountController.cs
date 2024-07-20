using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuthService authService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authService = authService;
            this.mapper = mapper;
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized(new ApiResponse(401, "Invalid Login"));

            var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401, "Invalid Login"));


            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await authService.CreateTokenAsync(user, userManager)
            });
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.Email.Split("@")[0],
                PhoneNumber = dto.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse
                {
                    Errors = result.Errors.Select(e => e.Description)
                });

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await authService.CreateTokenAsync(user, userManager)
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {

            //
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = email,
                Token = await authService.CreateTokenAsync(user, userManager)
            });
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await userManager.FindUserWithAddressAsync(User);
            var mappedAddress = mapper.Map<AddressDto>(user.Address);

            return Ok(mappedAddress);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<Address>> UpdateAddress(AddressDto dto)
        {

            var updatedAddress = mapper.Map<Address>(dto);
            var user = await userManager.FindUserWithAddressAsync(User);

            updatedAddress.Id = user.Address.Id;

            user.Address = updatedAddress;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = result.Errors.Select(e => e.Description)
                });

            return Ok(dto);

        }
    }
}
