using Iz.Online.Services.IServices;

namespace CashHelper
{
    public class Helper
    {
        private readonly ICacheService _cacheService;
        public Helper(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public string GetId(int id)
        {
           var res =  _cacheService.InstrumentData(id);
            return res.NscCode;
        }

    }
}