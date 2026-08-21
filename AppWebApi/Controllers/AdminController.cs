using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Seido.Utilities.SeedGenerator;
using Configuration.Options;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]   
    public class AdminController : Controller
    {
        readonly ILogger<AdminController> _logger;
        readonly IConfiguration _configuration;
        readonly IWebHostEnvironment _environment;
        readonly MySecret _mySecret;
        readonly VersionOptions _versionOptions;
        readonly AesEncryptionOptions _aesOptions;
        readonly SeedGenerator _seeder = new SeedGenerator();

        //GET: api/admin/helloworld
        [HttpGet()]
        [ActionName("HelloWorld")]
        [ProducesResponseType(200)]
        public IActionResult HelloWorld()
        {
            try
            {
                var helloWorldOptions = new
                {
                    greeting = "Hello, World!",
                    from = "a friend",
                    time = DateTime.UtcNow
                };
                _logger.LogInformation("HelloWorld endpoint called at {Time}", helloWorldOptions.time);
                return Ok(helloWorldOptions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/admin/version
        [HttpGet()]
        [ActionName("Version")]
        [ProducesResponseType(typeof(VersionOptions), 200)]
        public IActionResult Version()
        {
            try
            {
                
                _logger.LogInformation("Version endpoint called at {Time}", DateTime.UtcNow);
                return Ok(_versionOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving version information");
                return BadRequest(ex.Message);
            }
        }
        [HttpGet()]
        [ActionName("Quotes")]
        public IActionResult AllQuotes()
        {
            try
            {
                var allQuotes = _seeder.AllQuotes;
                _logger.LogInformation("AllQuotes endpoint called at, returning {Count} quotes", allQuotes.Count);
                return Ok(allQuotes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all quotes");
                return BadRequest(ex.Message);
            }


        }
        [HttpGet()]
        [ActionName("RandomQuote")]
        public IActionResult RandomQuote()
        {
            try
            {
                var randomQuote = _seeder.Quote;
                _logger.LogInformation("andomQuote endpoint called");
                return Ok(randomQuote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving random quote");
                return BadRequest(ex.Message);
            }
        }
        [HttpGet()]
        [ActionName("Key")]
        public IActionResult Key()
        {
            try
            {
                var keyOptions =new
                {
                    MyName = _configuration["MySettings:MyName"],
                    MyNumber = _configuration["MySettings:MyNumber"]
                };
                return Ok(keyOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving key");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("Options")]
        public IActionResult Options()
        {
            try
            {
               
                return Ok(_mySecret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving options");
                return BadRequest(ex.Message);
            }
        }
        [HttpGet()]
        [ActionName("Option2")]
        [ProducesResponseType( 200, Type = typeof(AesEncryptionOptions))]
        public IActionResult Option2()
        {
            try
            {
                
                return Ok(_aesOptions);
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }

        public AdminController(ILogger<AdminController> logger,IConfiguration configuration, IOptions<MySecret> mySecret,IOptions<VersionOptions> versionOptions,IOptions<AesEncryptionOptions> aesOptions, IWebHostEnvironment environment)
        {
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
           _mySecret = mySecret.Value;
           _aesOptions = aesOptions.Value;
           _versionOptions = versionOptions.Value;
        }
    }
}

