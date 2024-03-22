using ConwaysGameofLife.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConwaysGameofLife.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected JsonResult Ok<T>(T value)
        {
            return new JsonResult(new ResponseModel<T>(value))
            {
                StatusCode = 200
            };
        }

        protected JsonResult Created<T>(T value)
        {
            return new JsonResult(new ResponseModel<T>(value))
            {
                StatusCode = 201
            };
        }
    }
}
