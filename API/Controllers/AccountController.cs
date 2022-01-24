using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
   
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _contx;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext contx, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _contx = contx;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO register)
        {
            if (await UserExists(register.UserName)) return BadRequest("Username already taken");
           
            using var hmac = new HMACSHA256();

            var user = new AppUser
            {
                UserName = register.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hmac.Key
            };

            await _contx.Users.AddAsync(user);
            await _contx.SaveChangesAsync();

            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _contx.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.UserName.ToLower());

            if(user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            using var hmac = new HMACSHA256(user.PasswordSalt);
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for(int i = 0; i < ComputedHash.Length; i++)
            {
                if(ComputedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid username or password");
            }
            return new UserDTO
            {
                userName = user.UserName,
                token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _contx.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}