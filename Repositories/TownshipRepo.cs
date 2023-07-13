using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class TownshipRepo : ITownship
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<TownshipRepo> _logger;

        public TownshipRepo(ApplicationDbContext applicationDbContext, ILogger<TownshipRepo> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<List<Township>> GetTownshipList()
        {
            List<Township> model = new List<Township>();
            try
            {
                model = await _applicationDbContext.Townships.AsNoTracking().OrderByDescending(x => x.TownshipId).ToListAsync();
                _logger.LogInformation("Information: " + model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message: " + ex.Message.ToString(), ex);
            }
            return model;
        }


        public async Task<List<string>> GetTownshipMyanmarNameList()
        {
            List<string> model = new List<string>();

            try
            {
                model = await _applicationDbContext.Townships.AsNoTracking().Select(x => x.TownshipNameMyanmar).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message : " + ex.Message.ToString(), ex);
            }

            return model;
        }

        public async Task<Township> GetTownshipByID(int id)
        {
            Township model = new Township();
            try
            {
                int _id = Convert.ToInt32(id);
                model = await _applicationDbContext.Townships.AsNoTracking().Where(x => x.TownshipId == _id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message: " + ex.Message.ToString(), ex);
            }
            return model;
        }

        public async Task<int> CreateTownship(TownshipVM townshipVM)
        {
            int result = 0;
            try
            {
                var _township = new Township()
                {
                    TownshipCode = townshipVM.TownshipCode,
                    TownshipNameEnglish = townshipVM.TownshipNameEnglish,
                    TownshipNameMyanmar = townshipVM.TownshipNameMyanmar,
                    Region = townshipVM.Region
                };
                _applicationDbContext.Townships.Add(_township);
                result = _applicationDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message: " + ex.Message.ToString(), ex);
            }

            return result;
        }

        public async Task<int> UpdateTownship(int id, TownshipVM townshipVM)
        {
            int result = 0;
            try
            {
                var township = await _applicationDbContext.Townships.FindAsync(id);
                if (township != null)
                {
                    township.TownshipCode = townshipVM.TownshipCode;
                    township.TownshipNameEnglish = townshipVM.TownshipNameEnglish;
                    township.TownshipNameMyanmar = townshipVM.TownshipNameMyanmar;
                    township.Region = townshipVM.Region;
                    _applicationDbContext.Update(township);
                    result = await _applicationDbContext.SaveChangesAsync();
                    return result;
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message : " + ex.Message.ToString(), ex);
            }
            return result;
        }

        public async Task<int> DeleteTownship(int id)
        {
            int result = 0;
            try
            {
                Township model = await GetTownshipByID(id);
                if (model != null)
                    _applicationDbContext.Townships.Remove(model);
                result = _applicationDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message : " + ex.Message.ToString(), ex);
            }
            return result;
        }

    }
}