using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IMenus
    {
        string GetById(int id);
        Task<string> CreOrUpd(MenuVM vm);
        List<MenuVM> GetMenusLst();
        bool Delete(int id);
    }
}
