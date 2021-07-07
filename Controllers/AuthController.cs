//using System;
//using System.Threading.Tasks;
//using COVID_19.Data;
//using COVID_19.Data.Repository;
//using COVID_19.Models;
//using COVID_19.Models.ViewModels;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace COVID_19.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]/[action]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly IAuthRepository _authRepo;
//        public AuthController(IAuthRepository authRepository)
//        {
//            _authRepo = authRepository;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Register(RegisterViewModel request)
//        {
//            ServiceResponse<int> response = await _authRepo.Register(
//                new User { Username = request.UserName }, request.Password);
//            if (!response.Success)
//            {
//                return BadRequest(response);
//            }
//            return Ok(response);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login(LoginViewModel request)
//        {
//            ServiceResponse<string> response = await _authRepo.Login(
//                request.UserName, request.Password);
//            if (!response.Success)
//            {
//                return BadRequest(response);
//            }
//            return Ok(response);
//        }
//    }
//}
