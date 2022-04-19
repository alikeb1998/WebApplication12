using Confluent.Kafka;
using Iz.Online.HubConnectionHandler.IServices;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.Users;

namespace Iz.Online.HubConnectionHandler.Services
{
    public class HubConnationService :  IHubConnationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ConsumerConfig _consumerConfig;
        private readonly IInstrumentsRepository _instrumentsRepository;
        
        public HubConnationService(IUserRepository userRepository , IInstrumentsRepository instrumentsRepository)
        {
            _userRepository = userRepository;
            _instrumentsRepository = instrumentsRepository;
            _consumerConfig = new ConsumerConfig
            {

                BootstrapServers = "192.168.72.222:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }


        public CustomerInfo UserHubsList(string userId)
        {
            userId = "KafkaUserId_" + userId;
            return _userRepository.GetUserHubs(userId);

        }

       
        public void DeleteConnectionId(string connectionId)
        {
            _userRepository.DeleteConnectionId( connectionId);
        }

        public async Task<List<string>> GetInstrumentHubs(string NscCode)
        {
          return await _instrumentsRepository.GetInstrumentHubs(NscCode);
        }

        public Task<List<string>> GetHubsByCustomer(string CustomerId)
        {
            throw new NotImplementedException();
        }
    }
}
