using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ArduinoLightswitcherGateway;
using Serilog;
using LightSwitcherWeb.Model;
using Microsoft.AspNetCore.Cors;

namespace LightSwitcherWeb.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class SwitchesController : ControllerBase
    {
        public SwitchesController(ILightSwitcherGateway lightSwitcherGateway)
        {
            if (lightSwitcherGateway == null)
            {
                throw new ArgumentNullException(nameof(lightSwitcherGateway));
            }

            _lightSwitcherGateway = lightSwitcherGateway;
        }

        [HttpGet]
        public IEnumerable<Switch> Get()
        {
            var status = _lightSwitcherGateway.GetStatus();
            for (var i = 0; i < status.Length; i++)
            {
                yield return new Switch
                {
                    SwitchId = i,
                    State = (status[i] == ArduinoLightswitcherGateway.SwitchState.On 
                        ? LightSwitcherWeb.Model.SwitchState.On 
                        : LightSwitcherWeb.Model.SwitchState.Off)
                };
            }
        }

        [HttpPost("{switchId}")]
        public IActionResult ChangeSwitchState(byte switchId, [FromQuery]string mode)
        {
            var result = _lightSwitcherGateway.ChangeSwitchState(switchId, mode == "on");
            return new JsonResult(
                new {
                    Result = true,
                    Message = result
                }
            );
        }

        [HttpPost("/all")]
        public IActionResult All(string mode)
        {
            mode = mode.ToLower();

            if (mode == "on")
            {
                _lightSwitcherGateway.SwitchAllOn();
            }
            else if (mode == "off")
            {
                _lightSwitcherGateway.SwitchAllOff();
            }

            return Ok();
        }
 
        private readonly ILightSwitcherGateway _lightSwitcherGateway;
        private readonly ILogger  _logger = Log.ForContext<SwitchesController>();
    }
}