using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Configuration;
using Configuration.Options;


namespace AppWebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EncryptionController : ControllerBase
    {
            readonly Encryptions _encryptions = null;
            readonly ILogger<EncryptionController> _logger;
            readonly MySettingsOptions _settings;
        // Add your encryption-related endpoints here
        [HttpGet]
        [ActionName("Welcome")]
        public IActionResult Welcome()
        {
            
            return Ok(new { Message = "Welcome to the Encryption API!" });
        }
    
    [HttpGet]
    [ActionName("GetSettings")]
        public IActionResult GetSettings()
        {
            return Ok(_settings);
        }
    [HttpGet]
    [ActionName("Encrypted")]
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400)]
        public IActionResult Encrypted()
        {
            try
            {
                string encryptedBase64 = _encryptions.AesEncryptToBase64<MySettingsOptions>(_settings);
                return Ok(encryptedBase64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while encrypting the settings.");
                return BadRequest(ex.Message);
            }
        }

    [HttpGet]
    [ActionName("Decrypted")]
    [ProducesResponseType(200, Type = typeof(MySettingsOptions))]
    [ProducesResponseType(400)]
        public IActionResult Decrypted([FromQuery] string encryptedBase64)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedBase64))
                {
                    return BadRequest("The 'encryptedBase64' query parameter is required.");
                }
                var decrypted = _encryptions.AesDecryptFromBase64<MySettingsOptions>(encryptedBase64);
                return Ok(decrypted);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while decrypting the settings.");
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet]
        [ActionName("EncryptedPassword")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult EncryptedPassword([FromQuery] string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return BadRequest("The 'password' query parameter is required.");
                }
                string encryptedPassword = _encryptions.AesEncryptToBase64(password);
                return Ok(encryptedPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while encrypting the password.");
                return BadRequest(ex.Message);
            }
        }
        
    public EncryptionController(Encryptions encryptions, ILogger<EncryptionController> logger, IOptions<MySettingsOptions> mySettingsOptions)
    {
    _encryptions = encryptions;
    _logger = logger;
    _settings = mySettingsOptions.Value;
    }
    }
}