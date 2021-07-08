using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Happy5ChatTest.Models;
using Happy5ChatTest.Authentication;

namespace Happy5ChatTest.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthHandler _registrationHandler;

        public UserController(IAuthHandler RegistrationHandler)
        {
            _registrationHandler = RegistrationHandler ?? throw new ArgumentNullException(nameof(RegistrationHandler));
        }
        [HttpPost]
        public IActionResult Post([FromBody]RegistrationDTO model)
        {
            var response = _registrationHandler.HandleRegistration(model.Username, model.Password);

            return Ok(response.Result);
            
        }
    }
}
