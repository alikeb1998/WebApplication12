using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.DataAccess;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Iz.Online.Services.Services
{
    public class CacheService : ICacheService
    {
        private readonly IUserRepository _userRepository;
        private readonly IInstrumentsRepository _instrumentsRepository;
        public CacheService(IUserRepository userRepository, IInstrumentsRepository instrumentsRepository)
        {
            _userRepository = userRepository;
            _instrumentsRepository = instrumentsRepository;
        }



        public InstrumentList InstrumentData(int instrumentId)
        {
            return _instrumentsRepository.InstrumentData(instrumentId);
        }
        
        public List<InstrumentList> InstrumentData()
        {
            return _instrumentsRepository.InstrumentData();
        }

        public List<Izi.Online.ViewModels.AppConfigs> ConfigData()
        {
            return _userRepository.ConfigData();

        }

        public int GetLocalInstrumentIdFromOmsId(int omsId)
        {
            return _instrumentsRepository.GetLocalInstrumentIdFromOmsId(omsId);
        }

        public Izi.Online.ViewModels.AppConfigs ConfigData(string key)
        {
            return _userRepository.ConfigData(key);

        }

       
    }
}
