using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheIntegratorLib.Services;

namespace TheIntegratorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IUserSalesService _userSalesService;
        public SalesController(IUserSalesService userSalesService)
        {
            _userSalesService = userSalesService;
        }

        [HttpGet]
        [Route("row")]
        public IActionResult Record([FromRoute] string row)
        {

            return Ok();
        }
    }
}
