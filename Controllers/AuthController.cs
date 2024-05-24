using Azure.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UniYarWiki.Data;
using UniYarWiki.Models;

namespace UniYarWiki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDto userDto)
        {
            if (_context.Users.Any(u => u.Email == userDto.Email))
            {
                return BadRequest("User already exists.");
            }

            var user = new User
            {
                Email = userDto.Email,
                HashPwd = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                First = userDto.First,
                Last = userDto.Last,
                CourseNum = userDto.CourseNum,
                Group = userDto.Group,
                Avatar = userDto.Avatar,
                IsVerified = false
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Message = "User registered successfully.", UserId = user.Id });
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto userDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == userDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.HashPwd))
            {
                return Unauthorized("Invalid credentials.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("this_is_a_longer_secret_key_for_jwt_generation_123456");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "yourdomain.com",
                Audience = "yourdomain.com"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            return Ok(new { Token = tokenString });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Invalidate the token here (if needed)
            return Ok("User logged out successfully.");
        }
        [HttpPost("profileInfo")]
        public IActionResult GetUserProfile([FromBody] TokenDto tokenDto)
        {
            if (string.IsNullOrEmpty(tokenDto.Token))
            {
                return Unauthorized("Token is missing.");
            }

            var token = tokenDto.Token;
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("this_is_a_longer_secret_key_for_jwt_generation_123456");

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourdomain.com",
                    ValidAudience = "yourdomain.com",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Invalid token!!!.");
                }

                var user = _context.Users.SingleOrDefault(u => u.Id == int.Parse(userId));
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var userProfile = new
                {
                    user.Email,
                    user.First,
                    user.Last,
                    user.CourseNum,
                    user.Group,
                    user.Avatar,
                    user.IsVerified,
                    user.Id
                };

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return Unauthorized($"Token validation failed: {ex.Message}");
            }
        }

        [HttpPut("updateProfile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileRequest updateProfileRequest)
        {
            if (string.IsNullOrEmpty(updateProfileRequest.token))
            {
                return Unauthorized("Token is missing.");
            }

            var token = updateProfileRequest.token;
            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("this_is_a_longer_secret_key_for_jwt_generation_123456");

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourdomain.com",
                    ValidAudience = "yourdomain.com",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Invalid token.");
                }

                var user = _context.Users.SingleOrDefault(u => u.Id == int.Parse(userId));
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.First = updateProfileRequest.first;
                user.Last = updateProfileRequest.last;
                user.CourseNum = updateProfileRequest.course_num;
                user.Group = updateProfileRequest.group;
                user.Avatar = updateProfileRequest.avatar;

                _context.Users.Update(user);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return Unauthorized($"Token validation failed: {ex.Message}");
            }
        }
    }

    public class TokenDto
    {
        public string Token { get; set; }
    }
}
