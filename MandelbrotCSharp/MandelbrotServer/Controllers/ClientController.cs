using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MandelbrotServer.Controllers
{
   
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpPost]
        [Route("api/client")]
        public ActionResult CalculationRequest()
        {
            return Ok();
        }

    }
}
