using AutoMapper;
using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.AdminResponses;
using Newtonsoft.Json;

namespace DOTP_BE.Repositories
{
    public class AdminUserRepo : IAdminUser
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public AdminUserRepo(ApplicationDbContext context, IMapper map)
        {
            _context = context;
            _mapper = map;
        }

        public async Task<Responses> CheckUser(AdminUserVM vm)
        {
            Responses res = new Responses();
            var checkuser = _context.AdminUsers.Where(x => x.Name == vm.Name && x.Password == vm.Password).FirstOrDefault();
            if(checkuser != null)
            {
                res.Message = "Login Success";
                res.MenuList = _mapper.Map<List<MenuVM>>(_context.Menus.Where(x => x.RoleId == checkuser.RoleId).ToList());
                res.vmAdminUser = _mapper.Map<AdminUserVM>(checkuser);
                return res;
            }
            else
            {
                res.Message = "Login Fail";
                res.MenuList = new List<MenuVM>();
                return res;
            }
        }

        public bool CreateUser(AdminUserVM vm)
        {
            int checkusr = _context.AdminUsers.Where(x => x.Name == vm.Name).Count();
            if (checkusr == 0)
            {
                AdminUser usr = _mapper.Map<AdminUser>(vm);
                _context.AdminUsers.Add(usr);
                _context.SaveChanges();
                var u = _context.AdminUsers.Where(x => x.Name == usr.Name).FirstOrDefault();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string CreOrUpd(AdminUserVM vm)
        {
            int checkusr = _context.AdminUsers.Where(x => x.Name == vm.Name).Count();
            if(checkusr == 0)
            {
                AdminUser usr = _mapper.Map<AdminUser>(vm);
                if (usr.AdminId == 0)
                {
                    _context.AdminUsers.Add(usr);
                }
                else
                {
                    _context.AdminUsers.Update(usr);
                }
                _context.SaveChanges();
                var u = _context.AdminUsers.Where(x => x.Name == usr.Name).FirstOrDefault();
                return JsonConvert.SerializeObject(u);
            }
            else
            {
                return "User Already Exist!";
            }
           
        }

        public bool DeleteById(int id)
        {
           var data = _context.AdminUsers.Find(id);
           _context.AdminUsers.Remove(data);
           bool res =   _context.SaveChanges() > 0 ? true : false;
           return res;
        }

        public List<AdminUserVM> GetAdminUser()
        {
            var lst = _context.AdminUsers.ToList();
            var vmlst = _mapper.Map<List<AdminUserVM>>(lst);
            var offlst = _context.RegistrationOffices.ToList();
            var rolelst = _context.Roles.ToList();
            vmlst.ForEach(x =>
            {
                x.OfficeName = offlst.Where(f => f.OfficeId == x.OfficeId).Select(n => n.OfficeShortName).FirstOrDefault() ?? "";
                x.RoleName = rolelst.Where(r => r.RoleId == x.RoleId).Select(r => r.RoleName).FirstOrDefault() ?? "";
            });
            return vmlst;
        }

        public string GetById(int id)
        {
            var res = _context.AdminUsers.Find(id);
            return JsonConvert.SerializeObject(res);
        }

        public SelectedValuesVM GetSelectedValues()
        {
            var res = new SelectedValuesVM();
            var rlst = _context.Roles.ToList();
            var flst = _context.RegistrationOffices.ToList();
            res.rolelst = _mapper.Map<List<RolesVM>>(rlst);
            res.offlst = _mapper.Map<List<RegistrationOfficeVM>>(flst);
            return res;
        }

        public bool UpdateUser(AdminUserVM vm)
        {
            AdminUser usr = _mapper.Map<AdminUser>(vm);
            _context.AdminUsers.Update(usr);
            _context.SaveChanges();
            var u = _context.AdminUsers.Where(x => x.Name == usr.Name).FirstOrDefault();
            return true;
        }
    }
}
