using VirtualPet.Application.DTOs;
using VirtualPet.Domain.Entities;

namespace VirtualPet.Application.Mappings
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                CreateAt = user.CreateAt
            };
        }
    }
}
