using DigitalLibrary.DAL.Entities;

namespace DigitalLibrary.BLL.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}