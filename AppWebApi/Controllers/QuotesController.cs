using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Seido.Utilities.SeedGenerator;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]   
    public class QuotesController : Controller
    {
        readonly ILogger<QuotesController> _logger;
        readonly IWebHostEnvironment _environment;
        readonly SeedGenerator _seeder = new SeedGenerator();

        //GET: api/admin/helloworld
        
        
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


        public QuotesController(ILogger<QuotesController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }
    }
}

