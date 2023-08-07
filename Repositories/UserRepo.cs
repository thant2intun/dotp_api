using DOTP_BE.Common;
using DOTP_BE.Data;
using DOTP_BE.Helpers;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace DOTP_BE.Repositories
{
    public class UserRepo : IUser
    {
        private readonly ApplicationDbContext _context;
        //19-12-22 (al)
        private readonly EmailConfiguration _emailConfig;
        private readonly IConfiguration _configuration;

        public UserRepo(ApplicationDbContext context, EmailConfiguration emailConfig, IConfiguration configuration)
        {
            _context = context;
            _emailConfig = emailConfig;
            _configuration = configuration;
        }
        public async Task<List<User>> getUserList()
        {
            var result = await _context.Users.ToListAsync();
            return result;
        }
        public async Task<User> getUserById(int id)
        {
            var user = await _context.Users.Where(s => s.UserId == id).FirstOrDefaultAsync();
            return user;
        }
        public  bool Create(UserVM userVM)
        {
            if (userVM.Phone != null && userVM.Phone.Length > 3 && userVM.Phone[3] == '0')
                userVM.Phone = userVM.Phone.Remove(3, 1);
            if (!UserExists(userVM.Email, userVM.Phone))
            {
                string str_ConvertedNRCOrOID = "";
                if (userVM.RegisterWithNrc)
                {
                    int lstindex = userVM.NRC_Number.Length - 6;
                    str_ConvertedNRCOrOID = userVM.NRC_Number.Substring(0, lstindex) + NRCHelper.ChangeNRC_MyanToEnglish(userVM.NRC_Number.Substring(lstindex, 6));
                }else
                {
                    str_ConvertedNRCOrOID = NRCHelper.ChangeNRC_MyanToEnglish(userVM.NRC_Number);
                }
                
                var getPersInfo = _context.PersonInformations.Where(x => x.NRC_Number == str_ConvertedNRCOrOID).FirstOrDefault();//al(13/02/2023)

                var user = new User()
                {
                    Name = userVM.Name,
                    Email = string.IsNullOrWhiteSpace(userVM.Email)? null:userVM.Email,
                    Phone = string.IsNullOrWhiteSpace(userVM.Phone)? null:userVM.Phone,
                    Password = userVM.Password,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    NRC_Number = str_ConvertedNRCOrOID,
                    NRCId = userVM.RegisterWithNrc == true? userVM.NRCId : null,
                    PersonInformationId = getPersInfo != null ? getPersInfo.PersonInformationId :null //al(13/02/2023)

                };
                //19-12-22 (al)
                if (userVM.IsEmail==true) 
                {
                    var message = new Message(new string[] { userVM.Email }, "Activate Your DOTP Account", userVM.Email);
                    var emailMessage = CreateEmailMessage(message, user.Name);
                    Send(emailMessage);
                }
                else
                {
                    user.IsConfirm = true; //if register OTP already confirm at FE al (13/02/2023)
                }
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool Update(int id, UserVM userVM)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {    
                user.Name = userVM.Name;
                user.Email = userVM.Email;
                user.Phone = userVM.Phone;
                user.Password = userVM.Password;
                user.NRCId = userVM.NRCId;
                user.PersonInformationId = userVM.PersonInformationId != null ? userVM.PersonInformationId : null;
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChangesAsync();
            }

        }
        //Get OTP Code TZT_22_12_2022
        public string GetOTP(int id)
        {
            string otp_code = null;
            var user = _context.Users.Find(id);
            if(user != null)
            {
                otp_code = GenerateOTP();
                  
                if (user.Email != null)
                {
                    var message = new Message(new string[] { user.Email }, "This is your OTP code!", user.Email);
                    var messageBody = "Hi " + user.Name + ", <br>We received a request to use OTP at Domestic Operator Licence Solution!<br> <Text><h1>" + otp_code + "</h1></Text><br>It can only be used once and the expiration time of this otp code is 5 minutes!";
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
                    emailMessage.To.AddRange(message.To);
                    emailMessage.Subject = message.Subject;
                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format(messageBody) };
                    Send(emailMessage);
                }                
            }
            return otp_code;
        }

        //Check Validation method
        public bool UserExists(string Email, string Phone) // For Create b
        {
            if(Email.Length>1) //before Email.length >0
            {
                return _context.Users.Any(e => e.Email == Email || e.Phone == Phone);
            }
            else
            {
                return _context.Users.Any(e => e.Phone == Phone);
            }
            
        }

        public async Task<List<ExtendsLicenseVM>?> GetExtendsLicenses(string userId)
        {
            var user = await _context.Users.FindAsync(int.Parse(userId));

            if (user == null)
                return null;

            var distNRCs = await _context.OperatorDetails.AsNoTracking()
                .Include(x => x.Vehicle).ThenInclude(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)                 
                .Where(x => x.NRC == user.NRC_Number &&
                            (x.FormMode == ConstantValue.CreateNew_FM || x.FormMode == ConstantValue.EOPL_FM))                                                        
                .ToListAsync(); //get all data 'Creae New' and 'ExtendOperatorLicense'

            distNRCs = distNRCs.GroupBy(d => d.ApplyLicenseType)
                               .Select(g => g.OrderByDescending(d => d.ApplyDate).First())
                               .ToList(); // group by and select greatest date


            List<ExtendsLicenseVM> result = new List<ExtendsLicenseVM>();
            foreach (var license in distNRCs)
            {
                //apply date, formMode, appLiceseType equal count 
                int addCar = await _context.OperatorDetails.AsNoTracking()
                                           .Where(x => x.NRC == license.NRC &&
                                                       x.ApplyDate.Date >= license.ApplyDate.Date && 
                                                       x.FormMode == ConstantValue.AddNewCar_FM && 
                                                       x.ApplyLicenseType == license.ApplyLicenseType /*&&*/
                                                       /*x.IsDeleted == false*/)
                                           .SumAsync(x => x.TotalCar);

                int decCar = await _context.OperatorDetails.AsNoTracking()
                                    .Where(x => x.NRC == license.NRC &&
                                                x.ApplyDate.Date >= license.ApplyDate.Date &&
                                                x.FormMode == ConstantValue.DecreaseCar_FM &&
                                                x.ApplyLicenseType == license.ApplyLicenseType &&
                                                x.Transaction_Id != license.Transaction_Id)
                                    .SumAsync(x => x.TotalCar);

                //int decCar = await _context.OperatorDetails
                //                           .Where(x => x.NRC == license.NRC &&
                //                                       x.ApplyDate >= license.ApplyDate &&
                //                                       x.FormMode == ConstantValue.DecreaseCar_FM &&
                //                                       x.ApplyLicenseType == license.ApplyLicenseType /*&&*/
                //                                       /*x.IsDeleted == false*/)
                //                           .SumAsync(x => x.TotalCar);

                var vehicleObj = await _context.Vehicles.AsNoTracking()
                                               .Include(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
                                               .Where(x => x.Transaction_Id == license.Transaction_Id /*&&*/
                                                           /*x.IsDeleted == false*/)
                                               .FirstOrDefaultAsync();
                var license_type = await _context.LicenseTypes.Where(lt => lt.LicenseTypeId == vehicleObj.LicenseTypeId).FirstOrDefaultAsync();
                result.Add(new ExtendsLicenseVM
                {
                    OperatorId = license.OperatorId,
                    LicenseType = license_type.LicenseTypeShort, //added TZT 07Aug23
                    LicenseNumberLong = vehicleObj == null ? null : vehicleObj.LicenseNumberLong,
                    RegistrationOfficeName = vehicleObj == null ? null : vehicleObj.LicenseOnly.RegistrationOffice.OfficeLongName,
                    ExpiryDate = license.ExpiredDate,
                    TotalCar = (license.TotalCar + addCar - decCar),
                    //isDeleted = false
                }); ;
            }

            return result;
        }

        #region GetExtendsLicenses Worked
        //public async Task<List<ExtendsLicenseVM>> GetExtendsLicenses(string nrcDto)
        //{
        //    var distNRC = await _context.OperatorDetails.Include(x => x.Vehicle).ThenInclude(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
        //                                            .Where(x => x.NRC == nrcDto)
        //                                            .ToListAsync();

        //    List<ExtendsLicenseVM> result = new List<ExtendsLicenseVM>();
        //    foreach (var license in distNRC)
        //    {
        //        result.Add(new ExtendsLicenseVM
        //        {
        //            ApplyLicenseType = license.ApplyLicenseType,
        //            LicenseNumberLong = license.Vehicle == null? null:license.Vehicle.LicenseNumberLong,
        //            RegistrationOfficeName = license.Vehicle == null? null :license.Vehicle.LicenseOnly.RegistrationOffice.OfficeLongName,
        //            ExpiryDate = license.ExpiredDate,
        //            TotalCar = license.TotalCar,
        //        });
        //    }
            
        //    return result;
        //}
        #endregion

        #region //TZT OTP 22_12_2022
        public static string GenerateOTP()
        {
            Random random = new Random();

            var OTP = new string(Enumerable.Repeat("0123456789", 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return OTP;
        }
        
        #endregion
        #region SendEmailConfirmation_By_al 19-12-22(al)
        private MimeMessage CreateEmailMessage(Message message, string Name)
        {
            var messageBody = GetMailBody(message.Content, Name);
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format(messageBody) };
            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    var ms =  client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        public string GetMailBody(string emailId, string Name)
        {
            string url = _configuration.GetSection("DomainName").Value + "User/ConfirmMail?emailId=" + emailId;
            return string.Format(@"<div style='text-align:center;'>
                                <h1>Welcome {1} to our Domestic Operator Licence Solution</>
                                <h3>Click below link for verify your Email Id</h3>
                                <a href={0} style = 'display:block;
                                                     text-align:center;
                                                     Font-weight:bold;
                                                     backgroud-color: #008CBA;
                                                     font-size: 16px;
                                                     border-radius: 10px;
                                                     color:#ffffff;
                                                     cursor:pointer;
                                                     width:100%
                                                     padding:10px;'>
                                Confirm Mail</a></div>", url, Name);
        }
        #endregion

        public User? FindUserByEmailOrPhone(string EmailOrPhone)
        {
            var user = _context.Users.Where(x => x.Email == EmailOrPhone || x.Phone == EmailOrPhone).SingleOrDefault();

            if (user == null)
                return null;
             
            return user;
        }
    }
}
