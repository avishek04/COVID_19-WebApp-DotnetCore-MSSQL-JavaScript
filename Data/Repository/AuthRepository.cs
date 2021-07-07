//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using COVID_19.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;

//namespace COVID_19.Data.Repository
//{
//    public class AuthRepository : IAuthRepository
//    {
//        private readonly AppDbContext _context;
//        private readonly IConfiguration _configuration;

//        public AuthRepository(AppDbContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }

//        private string CreateToken(User user)
//        {
//            List<Claim> claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                new Claim(ClaimTypes.Name, user.Username)
//            };

//            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
//                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

//            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

//            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(claims),
//                Expires = DateTime.Now.AddDays(1),
//                SigningCredentials = creds
//            };

//            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
//            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

//            return tokenHandler.WriteToken(token);
//        }

//        public async Task<ServiceResponse<int>> Register(User user, string password)
//        {
//            ServiceResponse<int> response = new ServiceResponse<int>();
//            if (await UserExists(user.Username))
//            {
//                response.Success = false;
//                response.Message = "User already exists.";
//                return response;
//            }
//            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
//            user.PasswordHash = passwordHash;
//            user.PasswordSalt = passwordSalt;
//            await _context.users.AddAsync(user);
//            await _context.SaveChangesAsync();

//            if (user.Id > 0)
//            {
//                response.Success = true;
//                response.Data = user.Id;
//            }
            
//            return response;
//        }

//        public async Task<ServiceResponse<string>> Login(string username, string password)
//        {
//            ServiceResponse<string> response = new ServiceResponse<string>();
//            User user = await _context.users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));
//            if (user == null)
//            {
//                response.Success = false;
//                response.Message = "User not found.";
//            }
//            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
//            {
//                response.Success = false;
//                response.Message = "Wrong password.";
//            }
//            else
//            {
//                response.Success = true;
//                response.Data = CreateToken(user);
//            }
//            return response;
//        }

//        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//        {
//            using (var hmac = new System.Security.Cryptography.HMACSHA512())
//            {
//                passwordSalt = hmac.Key;
//                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//            }
//        }

//        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
//        {
//            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
//            {
//                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//                for (int i = 0; i < computedHash.Length; i++)
//                {
//                    if (computedHash[i] != passwordHash[i])
//                    {
//                        return false;
//                    }
//                }
//                return true;
//            }
//        }

//        public async Task<bool> UserExists(string username)
//        {
//            if (await _context.users.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
//            {
//                return true;
//            }
//            return false;
//        }
//    }
//}
