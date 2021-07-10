using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Happy5ChatTest.Models;
using Happy5ChatTest.Authentication;
using Microsoft.AspNetCore.Authorization;
using Happy5ChatTest.DataContext;

namespace Happy5ChatTest.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthHandler _registrationHandler;
        private readonly APIDbContext _context; 
        
        public UserController(IAuthHandler RegistrationHandler,APIDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(RegistrationHandler));
            _registrationHandler = RegistrationHandler ?? throw new ArgumentNullException(nameof(RegistrationHandler));
        }
        [HttpPost("/register/")]
        public IActionResult Post([FromBody]RegistrationDTO model)
        {
            var response = _registrationHandler.HandleRegistration(model.Username, model.Password);

            return Ok(response.Result);
            
        }
        [HttpGet("/users/")]
        public IActionResult Get()
        {
            var users = _context.Users.Select(x => x.userName).ToList();

            return Ok(users);
        }
    }
}
