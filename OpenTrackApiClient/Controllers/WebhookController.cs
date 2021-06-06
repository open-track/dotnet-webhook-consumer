using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace OpenTrackApiClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly string _storedSecret;

        public WebhookController(
            IConfiguration configuration,
            ILogger<WebhookController> logger)
        {
            _logger = logger;
            // Pulls the value from appsettings.json or the environment variable with the same name.
            _storedSecret = configuration.GetValue<string>("WEBHOOK_SECRET") ?? "shhhh";
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("{Uri} was executed from {IpAddress}.",
                Request.GetDisplayUrl(), HttpContext.Connection.RemoteIpAddress);

            return Ok("Hello");
        }

        [HttpPost]
        public IActionResult Post(
            [FromHeader(Name = "X-OpenTrack-Signature")] string signature,
            [FromBody] JsonElement json)
        {
            _logger.LogInformation("Received webhook request: {json}.", json);
            _logger.LogInformation(signature);

            if (!ValidateSignature(json.ToString(), _storedSecret, signature))
                return Unauthorized(new { errorMessage = "Invalid Signature" });

            var @event = json.GetProperty("event").GetString();
            switch (@event)
            {
                case "container.status.updated":
                    _logger.LogInformation("Handler for status update.");
                    break;
                case "container.itinerary.updated":
                    _logger.LogInformation("Handler for container.itinerary.updated.");
                    break;
                case "container.location.updated":
                    _logger.LogInformation("Handler for container.location.updated.");
                    break;
                case "container.holds.updated":
                    _logger.LogInformation("Handler for container.holds.updated.");
                    break;
                case "container.demurrage.updated":
                    _logger.LogInformation("Handler for container.demurrage.updated.");
                    break;
                case "container.tracking.failed":
                    _logger.LogInformation("Handler for container.tracking.failed.");
                    break;
                default:
                    _logger.LogInformation("No handler for type {event}.", @event);
                    break;
            }

            return NoContent();
        }

        private static bool ValidateSignature(string body, string secret, string signature)
        {
            var hash = HashHMACHex(secret, body);

            return signature.Equals(hash, StringComparison.Ordinal);
        }

        private static string HashHMACHex(string keyHex, string message)
        {
            byte[] hash = new HMACSHA256(StringEncode(keyHex))
                .ComputeHash(StringEncode(message));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(text);
        }
    }
}
