using Microsoft.AspNetCore.Mvc;

namespace DashyBoard.API.Controllers;

public class DepartureController : ControllerBase
{
    public IActionResult Index()
    {
        return Ok();
    }
}
