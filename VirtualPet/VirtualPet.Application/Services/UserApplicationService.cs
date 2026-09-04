using VirtualPet.Application.DTOs;
using VirtualPet.Application.Mappings;
using VirtualPet.Application.Repositories;
using VirtualPet.Domain.Entities;

namespace VirtualPet.Application.Services
{
    public sealed class UserApplicationService
    {
        private readonly IUserRepository _userRepository;

        public UserApplicationService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserDto> CreateUserAsync(string name, CancellationToken cancellationToken = default)
        {
            var user = new User(name);

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return UserMapper.ToDto(user);
        }

        public async Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            return user is null ? null : UserMapper.ToDto(user);
        }
    }
}
