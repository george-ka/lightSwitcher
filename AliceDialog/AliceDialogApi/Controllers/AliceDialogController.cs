using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AliceDialogApi.Models;

namespace AliceDialogApi.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class AliceDialogController : ControllerBase
    {
        public AliceDialogController()
        {
        }

        [HttpPost]
        public IActionResult Post([FromBody] AliceDialogRequest request)
        {
            
            return new JsonResult(new { Result = "Ok"});
        }
    }
}