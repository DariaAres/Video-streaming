using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VideoStreaming.Constants;
using VideoStreaming.Dtos.In;
using VideoStreaming.Dtos.Out;
using VideoStreaming.Exceptions;
using VideoStreaming.Models;
using VideoStreaming.Persistence;
using VideoStreaming.Services;

namespace VideoStreaming.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly VideoStreamingDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService,
            VideoStreamingDbContext context,
            ILogger<AuthController> logger,
            IEmailSender emailSender,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        [HttpPost("signUp")]
        public async Task<ActionResult> SignUpAsync(SignUpDto dto)
        {
            var user = _mapper.Map<User>(dto);

            var emailTaken = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailTaken)
            {
                throw new TerminateRequestException(nameof(dto.Email), "Данная почта уже занята.");
            }

            var userNameTaken = await _context.Users.AnyAsync(u => u.UserName == dto.Username);
            if (userNameTaken)
            {
                throw new TerminateRequestException(nameof(dto.Username), "Данная почта уже занята.");
            }

            user.EmailConfirmationCode = Random.Shared.Next(100000, 999999);

            await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errorText = "";

                    foreach (var error in result.Errors)
                    {
                        errorText += error.Code + ';';
                    }

                    throw new TerminateRequestException(errorText);
                }

                await _userManager.AddToRoleAsync(user, AuthenticationConstants.Roles.User);

                await _emailSender.SendEmailAsync(user.Email, "Код подтверждения",
                    $"Ваш код подтверждения для регистрации: <h3>{user.EmailConfirmationCode}</h3>.");

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                _logger.LogError(ex, "Error when trying register");

                throw;
            }

            return Ok();
        }

        [HttpPost("confirm")]
        public async Task<ActionResult> ConfirmEmailAsync(EmailConfirmationDto dto)
        {
            var user = await _userManager.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return NotFound();
            }

            if (user.EmailConfirmed)
            {
                return Ok();
            }

            if (user.EmailConfirmationCode != dto.Code)
            {
                throw new TerminateRequestException("Неаравильный код подтверждения");
            }

            user.EmailConfirmationCode = 0;
            user.EmailConfirmed = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("signIn")]
        public async Task<ActionResult<SignInResultDto>> SignInAsync(SignInDto dto)
        {
            var isUsername = !dto.UserName.Contains('@');
            var userLogInformation = string.Format(
                "{0}: {1}", isUsername ? "UserName" : "Email",dto.UserName);

            var user = isUsername
                ? await _userManager.FindByNameAsync(dto.UserName)
                : await _userManager.FindByEmailAsync(dto.UserName);

            if (user == null)
            {
                _logger.LogWarning("User not found. {0}", userLogInformation);
                return Unauthorized();
            }

            var signInResult = await _signInManager
                .CheckPasswordSignInAsync(user, dto.Password, false);

            if (!signInResult.Succeeded)
            {
                _logger.LogWarning("Incorrect password. {0}", userLogInformation);
                throw new TerminateRequestException("Неверный логин или пароль.", HttpStatusCode.Unauthorized);
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogInformation("User hasn't confirm the email. {0}", userLogInformation);
                throw new TerminateRequestException("Данная почта не подтверждена.");
            }

            if (user.UserName == "admin")
            {
                return Ok(new SignInResultDto
                {
                    UserName = user.UserName,
                    Token = await _tokenService.GenerateTokenAsync(user)
                });
            }

            var result = new SignInResultDto()
            {
                UserName = user.UserName,
                Token = await _tokenService.GenerateTokenAsync(user),
            };

            return Ok(result);
        }
    }
}
