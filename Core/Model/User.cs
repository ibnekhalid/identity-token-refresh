
using Microsoft.AspNetCore.Identity;
using System;

namespace Core.Model
{
    public class User : IdentityUser
    {
        #region Properties
        public override string Id { get; set; } = Guid.NewGuid().ToString();

        #endregion

       
        #region Construtors
        protected User()
        {

        }
        public User(string email,string username)
        {
            UserName = username;
            Email = email;
        }
        #endregion
    }

    public class Role : IdentityRole
    {

    }
    public class UserRole : IdentityUserRole<int>
    {

    }
    public class RoleClaim : IdentityRoleClaim<int>
    {

    }
}
