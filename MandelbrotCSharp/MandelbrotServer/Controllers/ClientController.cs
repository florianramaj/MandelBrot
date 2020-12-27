using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MandelbrotLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WPFClient.Model;

namespace MandelbrotServer.Controllers
{
   
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpPost]
        [Route("api/client")]
        public ActionResult<TripleResult> CalculationRequest([FromBody] CalculationRequest calculationRequest)
        {
            var calculator = new Calculator();

            var result = calculator.Calculate(calculationRequest.Height, calculationRequest.Width);
            
            var resultTripkeResult = result.Select(item => new TripleResult()
            {
                X = item.Item1,
                Y = item.Item2,
                Iteration = item.Item3
            }).ToList();
            
            return Ok(resultTripkeResult);

        }

    }
}
