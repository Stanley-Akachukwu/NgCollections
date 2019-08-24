using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Abstract
{
    public interface IAuthentication
    {
        bool Authenticate(string email, string password);
        bool Logout();
        bool IsAdmin(string email, string password);
    }
}
