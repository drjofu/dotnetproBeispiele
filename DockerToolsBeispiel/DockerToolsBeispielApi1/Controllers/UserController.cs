using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerToolsBeispielApi1.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IConfiguration configuration;
    private readonly ILogger<UserController> logger;

    public UserController(IConfiguration configuration, ILogger<UserController> logger)
    {
      this.configuration = configuration;
      this.logger = logger;
    }

    [HttpGet]
    public UserInfo Get()
    {
      logger.LogInformation("Benutzerdaten abgerufen");

      return new UserInfo(configuration["user"], configuration["password"]);
    }
  }

  public record UserInfo(string User,string Password);
}
