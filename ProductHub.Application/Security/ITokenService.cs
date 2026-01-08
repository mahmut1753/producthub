using ProductHub.Domain.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Security;

public interface ITokenService
{
    string GenerateToken(User user);
}
