using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.models;

namespace Repositories.Interfaces
{
    public interface IUserInterface
    {
        Task<int> Register(t_User register);
        Task<t_User> Login(Login user);

        Task<t_User> GetUser(string user);

        // Task<>
    }
}