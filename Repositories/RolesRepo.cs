using AutoMapper;
using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Newtonsoft.Json;

namespace DOTP_BE.Repositories
{
    public class RolesRepo : IRole
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        public RolesRepo(IMapper map, ApplicationDbContext context)
        {
            _context = context;
            _mapper = map;
        }
        public async Task<string> CreOrUpd(RolesVM vm)
        {
            //   MenuVM res = new MenuVM();
            Role data = _mapper.Map<Role>(vm);
            if (data.RoleId == 0)
            {
                _context.Roles.Add(data);
                _context.SaveChanges();
                //  res = _context.SaveChanges() > 0 ? true : false;
                var insertdata = _context.Roles.Where(x => x.RoleName == data.RoleName).FirstOrDefault();
                //  res = _mapper.Map<MenuVM>(insertdata);
                var res = JsonConvert.SerializeObject(insertdata, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                return res;
            }
            else
            {
               if(data.Menus.Select(x => x.MenuId).FirstOrDefault() == 0)
                {
                    data.Menus = null;
                    _context.Roles.Update(data);
                    _context.SaveChanges();
                }
                else
                {
                    List<Menu> menus = data.Menus;
                    data.Menus = null;
                    _context.Roles.Update(data);
                    try
                    {
                        //   _context.Menus.UpdateRange(menus);
                        foreach (var m in menus)
                        {
                            m.Role = null;
                            _context.Menus.Update(m);
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    _context.SaveChanges();
                }
                //     res = _context.SaveChanges() > 0 ? true : false;
                var upddata = _context.Roles.Where(x => x.RoleId == data.RoleId).FirstOrDefault();
                //  res = _mapper.Map<MenuVM>(upddata);
                var res = JsonConvert.SerializeObject(upddata, Formatting.None,
                       new JsonSerializerSettings()
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                       });
                return res;
            }
        }

        public bool Delete(int id)
        {
            if(id != 0)
            {
                var data = _context.Roles.Find(id);
                _context.Roles.Remove(data);
               bool res =  _context.SaveChanges() > 0 ? true : false;
                return res;
            }
            else
            {
                return false;
            }
        }

        public string GetById(int id)
        {
            var data =  _context.Roles.Where(x => x.RoleId == id).FirstOrDefault();
            data.Menus = _context.Menus.Where(x => x.RoleId == id).ToList();
            var res = JsonConvert.SerializeObject(data, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            return res;
        }

        public List<RolesVM> Rolelst()
        {
            var lst = _context.Roles.ToList();
            return _mapper.Map<List<RolesVM>>(lst);
        }
    }
}
