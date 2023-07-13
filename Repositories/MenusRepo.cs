using AutoMapper;
using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Newtonsoft.Json;

namespace DOTP_BE.Repositories
{
    public class MenusRepo : IMenus
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        public MenusRepo(ApplicationDbContext context, IMapper map)
        {
            _context = context;
            _mapper = map;
        }
        //public async Task<MenuVM> CreOrUpd(MenuVM vm)
        public async Task<string> CreOrUpd(MenuVM vm)
        {
         //   MenuVM res = new MenuVM();
            Menu data = _mapper.Map<Menu>(vm);
            if(data.MenuId == 0)
            {
                data.Role = null;
               _context.Menus.Add(data);
                _context.SaveChanges();
                //  res = _context.SaveChanges() > 0 ? true : false;
                var insertdata = _context.Menus.Where(x => x.MenuName == data.MenuName).FirstOrDefault();
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
                data.Role = null;
                _context.Menus.Update(data);
               _context.SaveChanges();
                //     res = _context.SaveChanges() > 0 ? true : false;
                var upddata = _context.Menus.Where(x => x.MenuId == data.MenuId).FirstOrDefault();
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
          var data =  _context.Menus.Find(id);
          _context.Menus.Remove(data);
          bool res = _context.SaveChanges() > 0 ? true : false;
          return res;
        }

        public string GetById(int id)
        {
            var data = _context.Menus.Find(id);
            data.Role = _context.Roles.Where(x => x.RoleId == data.RoleId).FirstOrDefault();
            var res = JsonConvert.SerializeObject(data, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            return res;
        }

        public List<MenuVM> GetMenusLst()
        {
            var menulst = _context.Menus.ToList();
            return _mapper.Map<List<MenuVM>>(menulst);
        }
    }
}
