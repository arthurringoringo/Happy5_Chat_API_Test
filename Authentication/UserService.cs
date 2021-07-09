using Happy5ChatTest.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Happy5ChatTest.Models;
using Microsoft.EntityFrameworkCore;
namespace Happy5ChatTest.Authentication
{
    public class UserService : IUserService
    {
        private readonly APIDbContext _context;
        public UserService(APIDbContext Context)
        {
            _context = Context ?? throw new ArgumentNullException(nameof(Context));

        }
        public bool ValidateUser(string username, string password)
        {
            var user = _context.Users.Where(x => x.userName == username).FirstOrDefault();

            if (user != null)
            {
                if (user.password == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }

        }
        public bool RegisterUser(string username, string password)
        {
            var user = _context.Users.Where(x => x.userName == username).FirstOrDefault();

            if (user == null)
            {
                user = new User();
                user.userName = username;
                user.password = password;

                _context.Users.Add(user);

                _context.SaveChanges();

                _context.Entry(user).State = EntityState.Detached; 

                    return true;
            }
            else 
            {
                return false;
            }
        }

        public string GetUserId(string username)
        {
            var user = _context.Users.Where(x => x.userName == username).FirstOrDefault();

            return user.userId.ToString();
        }
    }

    public interface IUserService
    {
        bool ValidateUser(string username, string password);
        bool RegisterUser(string username, string password);
        public string GetUserId(string username);


    }
}
