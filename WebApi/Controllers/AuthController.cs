namespace WebApi.Controllers;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IServices;
using Abstraction.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IUserService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var user = await _authService.AuthenticateAsync(model.Username, model.Password);
        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    [HttpPost("google-verify")]
    public async Task<IActionResult> VerifyGoogleToken([FromBody] GoogleTokenRequest model)
    {
        try
        {
            var payload = await this.VerifyGoogleTokenAsync(model.Credential);

            if (payload == null)
            {
                return Unauthorized("Invalid Google token");
            }

            var user = await _authService.FindOrCreateUserByEmailAsync(payload);

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    username = user.Username
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest("Google authentication failed");
        }
    }

    private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string token)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = [this._configuration ["Google:ClientId"]],
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            return payload;
        }
        catch
        {
            return null;
        }
    }

    private string GenerateJwtToken(UserModel user)
    {
        var claims = new []
        {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role)
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration ["JwtSettings:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration ["JwtSettings:Issuer"],
            audience: _configuration ["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class GoogleTokenRequest
{
    public string Credential { get; set; }
}
