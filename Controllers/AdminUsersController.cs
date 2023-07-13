using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.AdminResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly IAdminUser _adminusrRepo;
        private readonly IConfiguration _configuration;

        public AdminUsersController(IAdminUser repo, IConfiguration config)
        {
            _adminusrRepo = repo;
            _configuration = config;
        }

        [HttpGet]
        public IActionResult AdminUserLst()
        {
            var usrlst = _adminusrRepo.GetAdminUser();
            return Ok(usrlst);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var res =    _adminusrRepo.GetById(id);
            return Ok(res);
        }

        [HttpPost("CreOrUpd")]
        public IActionResult CreOrUpd(AdminUserVM vm)
        {
            if(vm != null)
            {
                var res = vm.AdminId != 0 ? _adminusrRepo.UpdateUser(vm) : _adminusrRepo.CreateUser(vm);

                if (res)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest ("User Already Exists");
                }

            }
            else
            {
                return BadRequest("Don't have data!");
            }
        }

        [HttpDelete("DeleteById/{id}")]
        public IActionResult DeleteById(int id)
        {
            bool res = _adminusrRepo.DeleteById(id);
            return Ok(res);
        }

        [Route("Authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate(AdminUserVM vm)
        {
            TokenResponse tokenResponse = new TokenResponse();
            if (vm.Name != null && vm.Password != null)
            {
                //AdminUserVM vm = new AdminUserVM();
                //vm.Name = username;
                //vm.Password = password;
                var res = await _adminusrRepo.CheckUser(vm);
                if(res.vmAdminUser != null)
                {
                    List<Claim> claims = new List<Claim>
                        {
                             new Claim(ClaimTypes.Name, res.vmAdminUser.Name),
                             new Claim(ClaimTypes.Role, res.vmAdminUser.RoleId.ToString())
                        };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTSetting").GetSection("securitykey").Value);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {

                        Subject = new ClaimsIdentity(
                            new Claim[]
                            {
                        new Claim(ClaimTypes.Name, res.vmAdminUser.Name),
                        new Claim(ClaimTypes.Role, res.vmAdminUser.RoleId.ToString())
                            }),
                        Expires = DateTime.Now.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                    };

                    // var token = tokenHandler.CreateToken(toeknDescription);
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    string finaltoken = tokenHandler.WriteToken(token);
                    tokenResponse.Token = finaltoken;
                    return Ok(tokenResponse);
                }
                else
                {
                    return Unauthorized(tokenResponse);
                }               
            }
            else
                return Unauthorized(tokenResponse);
          
        }

        //[HttpPost("AdminLogin")]
        [HttpGet("AdminLogin")]
        public async Task<IActionResult> Login(string username, string password)
      //  public async Task<IActionResult> Login(AdminUserVM vm)
        {
            //Responses result = new Responses();
            AdminUserVM vm = new AdminUserVM();
            vm.Name = username;
            vm.Password = password;
            var res = await _adminusrRepo.CheckUser(vm);
              //  if (res.vmAdminUser != null)
              if(res.Message == "Login Success")
                {
                    List<Claim> claims = new List<Claim>
                        {
                             new Claim(ClaimTypes.Name, res.vmAdminUser.Name),
                             new Claim(ClaimTypes.Role, res.vmAdminUser.RoleId.ToString())
                        };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTSetting").GetSection("securitykey").Value);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {

                        Subject = new ClaimsIdentity(
                            new Claim[]
                            {
                        new Claim(ClaimTypes.Name, res.vmAdminUser.Name),
                        new Claim(ClaimTypes.Role, res.vmAdminUser.RoleId.ToString())
                            }),
                        Expires = DateTime.Now.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                    };

                    // var token = tokenHandler.CreateToken(toeknDescription);
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    string finaltoken = tokenHandler.WriteToken(token);
                    res.Token = finaltoken;
                    return Ok(res);
                }
                else
                {
                //  return Unauthorized(res);
                return Ok(res);
                }

        }

        #region Selected Values
        [HttpGet("SelectedValue")]
        public IActionResult SelectedValue()
        {
            var res = _adminusrRepo.GetSelectedValues();
            return Ok(res);
        }
        #endregion
    }
}
