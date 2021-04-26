using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace lundk.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;

        public EventController(ILogger<EventController> logger)
        {
            _logger = logger;
        }

        [HttpGet("public")]
        public string Get()
        {
            return "This is public.";
        }

        [Authorize]
        [HttpGet("secured")]
        public string GetSecured()
        {
            return $"Hello {User.FindFirstValue(ClaimTypes.GivenName)}. This is a secure content and you are allowed to see it.";
        }

        [HttpGet("token/{registrationCode}")]
        public ActionResult<string> GetToken(string registrationCode)
        {
            if (registrationCode.Equals("4711-4711-4711"))
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningOptions.Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var permClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("eventId", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.FamilyName, "Brandl"),
                    new Claim(JwtRegisteredClaimNames.GivenName, "Martin")
                };

                var token = new JwtSecurityToken(SigningOptions.Issuer,
                    SigningOptions.Issuer,
                    permClaims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }

            return BadRequest("Invalid registration code");
        }
    }
}
