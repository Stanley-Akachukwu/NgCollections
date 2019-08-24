using NgCollections.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Concrete
{
    public class FormsAuthenticationProvider : IAuthentication
    {
        private readonly EFDbContext context = new EFDbContext();
        public bool Authenticate(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
                return false;
            return true;
        }

        public bool IsAdmin(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user.UserLevel == 1)
                return true;
            return false; ;
        }

        public bool Logout()
        {
            return true;
        }
    }
}
