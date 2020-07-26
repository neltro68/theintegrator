using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheIntegratorLib.Models;
using TheIntegratorLib.Services;

namespace TheIntegratorAPI.Controllers
{
    [ApiController]
    [Route("[controller]/v1/api/")]
    public class SalesController : ControllerBase
    {
        private readonly IUserSalesService _userSalesService;
        public SalesController(IUserSalesService userSalesService)
        {
            _userSalesService = userSalesService;
        }

        [HttpGet]
        [Route("recoard/{row}")]
        public IActionResult Record([FromRoute] string row)
        {
            _userSalesService.Record(row);
            return Ok();
        }

        [HttpGet]
        [Route("header/{row}")]
        public IActionResult Header([FromRoute] string row)
        {
            _userSalesService.SetHeader(row);
            return Ok();
        }

        [HttpGet]
        [Route("report")]
        public IActionResult Report([FromQuery] string fromDate, string toDate)
        {
            DateTime? dFromDate = null;
            DateTime? dToDate = null;
            if (DateTime.TryParse(fromDate, out DateTime parseFromDate))
            {
                dFromDate = parseFromDate;
            }
            if (DateTime.TryParse(fromDate, out DateTime parseToDate))
            {
                dToDate = parseToDate;
            }
            List<UserSalesModel> list = _userSalesService.GetSales(dFromDate, dToDate);
            return Ok(list);
        }
    }
}
