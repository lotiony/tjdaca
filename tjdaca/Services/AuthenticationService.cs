using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using tjdaca.Class;
using tjdaca.Data;

namespace tjdaca.Services
{
    public class AuthenticationService : IAuthenticationService
	{
        private readonly tjdacaContext _dbContext;

        public AuthenticationService(tjdacaContext dbContext)
        {
            this._dbContext = dbContext;
        }

		public async Task<bool> Login(string username, string password)
        {
            var user = await _dbContext.AcaMembers.SingleOrDefaultAsync(u => u.UserId.Equals(username));

            if (user != null)
            {
                return Encryptor.ValidatePassword(password, user.UserPw);
            }

            return false; // Failed login
        }
    }

}
