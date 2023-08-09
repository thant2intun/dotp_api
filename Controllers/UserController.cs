using DOTP_BE.Common;
using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
//using DOTP_BE.Models;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.MobileResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _repo;
        private readonly JWTSetting setting;
        private readonly IConfiguration _configuration;
        public UserController(IOptions<JWTSetting> options, IConfiguration configuration, IUser repo )
        {
            _repo = repo;
            setting = options.Value;
            _configuration = configuration;
        }

        [Route("Authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody] UserCredentials ucre)
        {
            if (ucre.EmailOrPhone == null || ucre.EmailOrPhone == String.Empty) //for empty string
                return BadRequest();
            var checkEmail = ucre.EmailOrPhone.ToString().Contains('@');
            if (!checkEmail)
            {
                #region for +959... format
                if (ucre.EmailOrPhone[0] == '0')
                    ucre.EmailOrPhone = "+95" + ucre.EmailOrPhone.Remove(0, 1);
                else if (ucre.EmailOrPhone[0] == '9')
                    ucre.EmailOrPhone = "+95" + ucre.EmailOrPhone;
                #endregion
            }

            User? user = _repo.FindUserByEmailOrPhone(ucre.EmailOrPhone);
            if (user == null)
                return NotFound("User Not Found.");
            if(user.IsActive == false)
                return BadRequest("Inactive");
            if (user.IsConfirm == false)
                return BadRequest("You Are Not Confirmed Yet.");
            if (user.Password != ucre.Password)
                return BadRequest("Incorrect Password");
            string _token = GenerateToken(user); //TZT Split Method for Generate Authentication Token 040723
            return Ok(new { userId = user.UserId, token = _token });
        }

        #region TZT For Mobile Added 040723
        [HttpPost("CreateMobile")]
        public IActionResult CreateMobile([FromBody] UserVM userVM)
        {
            var user = _repo.Create(userVM);
            ResponseMessage um_res = new ResponseMessage();
            if (user)
            {
                um_res.Status = true;
                um_res.Message = "success";
                return Ok(um_res);
            }
            um_res.Message = "User already exists.";
            return BadRequest(um_res);
        }

        [Route("AuthenticateMobile")] 
        [HttpPost]
        public IActionResult AuthenticateMobile([FromBody] UserCredentials ucre)
        {
            if (ucre.EmailOrPhone == null || ucre.EmailOrPhone == String.Empty)
                return BadRequest();
            var checkEmail = ucre.EmailOrPhone.ToString().Contains('@');
            if (!checkEmail)
            {
                #region for +959... format
                if (ucre.EmailOrPhone[0] == '0')
                    ucre.EmailOrPhone = "+95" + ucre.EmailOrPhone.Remove(0, 1);
                else if (ucre.EmailOrPhone[0] == '9')
                    ucre.EmailOrPhone = "+95" + ucre.EmailOrPhone;
                #endregion
            }

            User? user = _repo.FindUserByEmailOrPhone(ucre.EmailOrPhone);
            if (user == null)
                return NotFound(new { Status = false, Message = "User Not Found!"});
            if (user.IsConfirm == false)
                return BadRequest(new { Status = false, Message = "Confirm your email first!"});
            if (user.Password != ucre.Password)
                return BadRequest(new { Status = false, Message = "Incorrect Password"});
            string _token = GenerateToken(user);
            UserAuthenticateMobileResponse usr_res = new UserAuthenticateMobileResponse();
            usr_res.Token = _token;
            usr_res.Message = "success";
            usr_res.Status = true;
            usr_res.UserId = user.UserId;
            return Ok(usr_res);
        }

        private string GenerateToken(User? user)
        {
            List<Claim> claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name,user.Name),
                 //new Claim(ClaimTypes.Role,user.Role)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTSetting").GetSection("securitykey").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserId.ToString()),
                        //new Claim(ClaimTypes.Role, user.Role.ToString())

                    }
                ),
                Expires = DateTime.Now.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };

            // var token = tokenHandler.CreateToken(toeknDescription);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenHandler.WriteToken(token);
            return finaltoken;
        }

        //modified tzt 040723 reference from al 20/02/2023
        [HttpGet("SearchUserByEmailOrPhoneMobile/{emailOrPhone}")]
        public IActionResult SearchUserByEmailOrPhoneMobile([FromRoute] string emailOrPhone)
        {
            var checkEmail = emailOrPhone.ToString().Contains('@');
            if (!checkEmail)
            {
                #region for +959... format
                if (emailOrPhone[0] == '0')
                    emailOrPhone = "+95" + emailOrPhone.Remove(0, 1);
                else if (emailOrPhone[0] == '9')
                    emailOrPhone = "+95" + emailOrPhone;
                #endregion
            }

            User? user = _repo.FindUserByEmailOrPhone(emailOrPhone);
            if (user == null)
                return NotFound(new {Status = false, Message = "User Not Found!" }); 
            if (user.IsConfirm == false)
                return BadRequest(new {Status=false, Message = "Your account is not confirmed yet!" });
            return Ok( new UserMobileResponse() { Status=true, Message="success", Data=user});
        }
        //TZT 22_12_2022
        [HttpGet("GetOTPMobile/{id}")]
        public IActionResult GetOTPMobile([FromRoute] int id)
        {
            var _otp = _repo.GetOTP(id);
            if (_otp == null)
            {
                return BadRequest(new ResponseMessage() { Status = false ,Message= "User does not exist!" });
            }
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(5);
            return Ok(new UserOTPResp() { Status=true,Message="success",OtpCode=_otp, ExpireTime=expirationTime});
        }
        [HttpGet("Mobile/{id}")]
        public async Task<IActionResult> GetUserByIdMobile([FromRoute] int id)
        {
            var user = await _repo.getUserById(id);
            if (user == null)
            {
                return BadRequest(new ResponseMessage() { Status=false,Message="User does not exist!"});
            }
            return Ok(new UserMobileResponse() { Status=true,Message="success",Data=user});
        }
        [HttpPut("Mobile/{id}")]
        public IActionResult UpdateUserMobile(int id, [FromBody] UserVM userVM)
        {
            var OUpdate = _repo.Update(id, userVM);
            if (OUpdate)
            {
                return Ok(new ResponseMessage() { Status=true,Message="Your password has succcessfully been changed!"});
            }
            return BadRequest(new ResponseMessage() { Status=false,Message="Fail!"});
        }
        [HttpGet("ExtendsLicenseMobile/{userId}")]
        public async Task<IActionResult> Extends_LicenseMobile(string userId)
        {
            if (userId == null)
                return BadRequest();
            var data = await _repo.GetExtendsLicenses(userId);
            if (data == null)
                return NotFound(new ResponseMessage() { Status = false, Message = "Record not found!" });
            return Ok(new { Status = true, Message = "success", Data = data });
        }
        #endregion

        [HttpGet("")]
        public async Task<IActionResult> GetAllUser()
        {
            var user = await _repo.getUserList();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _repo.getUserById(id);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserVM userVM)
        {
            var user = _repo.Create(userVM);
            if (user)
            {
                return Ok();
            }

            return BadRequest("User already exists.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserVM userVM)
        {
            var OUpdate = _repo.Update(id, userVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest("Fail to Update.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }

        //TZT 22_12_2022
        [HttpGet("GetOTP/{id}")]
        public IActionResult GetOTP([FromRoute] int id)
        {
            var _otp =  _repo.GetOTP(id);
            if(_otp == null)
            {
                return BadRequest("User does not exist!");
            }
            return Ok(_otp);
        }

        //al 20Dec22
        //[HttpGet("ConfirmMail")]
        //public IActionResult ConfirmMail(string emailId)
        //{
        //    var user = _context.Users.Where(x => x.Email == emailId).FirstOrDefault();
        //    if (user == null)
        //        return NotFound("Your email is not found");
        //    if(user.IsConfirm == true)
        //    {
        //        return Ok("Your email is already confirmed!");
        //    }
        //    user.IsConfirm = true;
        //    _context.SaveChanges();
        //    return Ok("Your Email " + emailId + " is successfully confirm. Now you can Login in to DOTP.");

        //}

        [HttpGet("ExtendsLicense/{userId}")]
        public async Task<IActionResult> Extends_License(string userId)
        {
            if (userId == null)
                return BadRequest();
            var data = await _repo.GetExtendsLicenses(userId);
            if(data == null)
                return NotFound();
            return Ok(data);
        }

        //al 20/02/2023
        [HttpGet("SearchUserByEmailOrPhone/{emailOrPhone}")]
        public IActionResult SearchUserByEmailOrPhone([FromRoute] string emailOrPhone)
        {
            var checkEmail = emailOrPhone.ToString().Contains('@');
            if (!checkEmail)
            {
                #region for +959... format
                if (emailOrPhone[0] == '0')
                    emailOrPhone = "+95" + emailOrPhone.Remove(0, 1);
                else if (emailOrPhone[0] == '9')
                    emailOrPhone = "+95" + emailOrPhone;
                #endregion
            }

            User? user = _repo.FindUserByEmailOrPhone(emailOrPhone);
            if (user == null)
                return NotFound("User Not Found.");
            if (user.IsConfirm == false)
                return BadRequest("Your account is not confirmed yet!");
            return Ok(user);
        }
    }
}
