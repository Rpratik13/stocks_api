using System;
using api.Models;

namespace api.Interface;

public interface ITokenService
{
    public string CreateToken(AppUser user);
}
