//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using WebApi.Models;

//namespace WebApi.Controllers
//{
//    // login
//    // logout
//    // register
//    // refresh
//    // forgot & reset password
//    // manage / info

//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly RoleManager<IdentityRole> _roleManager;

//        public AuthController(UserManager<ApplicationUser> userManager,
//                          SignInManager<ApplicationUser> signInManager,
//                          RoleManager<IdentityRole> roleManager)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _roleManager = roleManager;
//        }

//        [HttpPost("login")]
//        public IActionResult Login()
//        {
//            throw new NotImplementedException();
//        }

//        [HttpPost("logout")]
//        public IActionResult Logout()
//        {
//            throw new NotImplementedException();
//        }

//        [HttpPost("register")]
//        public IActionResult Register()
//        {
//            throw new NotImplementedException();
//        }

//        [HttpPost("refresh-token")]
//        public IActionResult RefreshToken()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
