using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TheIntegratorLib.Models;
using TheIntegratorLib.Services;
using TheIntegratorLib.Utilities;

namespace TheIntegratorAPI.Controllers
{
    [ApiController]
    [Route("[controller]/v1/api/")]
    public class SalesController : ControllerBase
    {
        private readonly IUserSalesService _userSalesService;
        private readonly IFileService _fileService;
        private readonly IDataCache _userSalesCache;
        public SalesController(IUserSalesService userSalesService, IDataCache userSalesCache, IMemoryCache memoryCache, IFileService fileService)
        {
            _userSalesService = userSalesService;
            _userSalesCache = userSalesCache;
            _userSalesCache.SetCache(memoryCache);
            _userSalesService.UseCache(_userSalesCache);
            _fileService = fileService;
            _fileService.UseCache(_userSalesCache);
        }

        [HttpPost("record")]
        public async Task<IActionResult> Record([FromForm(Name = "files")] List<IFormFile> files)
        {
            try
            {
                List<Stream> streamFiles = new List<Stream>();
                foreach (IFormFile formFile in files)
                {
                    streamFiles.Add(formFile.OpenReadStream()); 
                }                    
                await _fileService.ProcessAsync(streamFiles, "temp", _userSalesService);

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
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
