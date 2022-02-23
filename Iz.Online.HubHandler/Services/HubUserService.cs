using Iz.Online.HubHandler.IServices;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.Users;

namespace Iz.Online.HubHandler.Services
{
    public class HubUserService : IHubUserService
    {
        public IUserRepository _userRepository { get; set; }
    
        public HubUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UsersHubIds UserHubsList(string userId)
        {

            return  _userRepository.GetUserHubs(userId);

        }

        public void SetUserHub(string userId, string connectionId)
        {
           _userRepository.SetUserHub(userId, connectionId);
        }

        public void DeleteConnectionId(string userId, string connectionId)
        {
            _userRepository.DeleteConnectionId(userId, connectionId);
        }
    }
}
