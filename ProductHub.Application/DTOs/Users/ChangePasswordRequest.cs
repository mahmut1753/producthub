using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.DTOs.Users;

public class ChangePasswordRequest
{
    public string NewPassword { get; set; }
}
