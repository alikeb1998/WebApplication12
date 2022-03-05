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
        
        public HubConnationService(IUserRepository userRepository )
        {
            _userRepository = userRepository;
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

        
    }
}
