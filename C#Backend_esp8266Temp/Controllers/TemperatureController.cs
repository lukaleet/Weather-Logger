using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using esp8266Temp.Data;
using esp8266Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace esp8266Temp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemperatureController : ControllerBase
    {
        private readonly DataContext _context;
        public TemperatureController(DataContext context)
        {
            _context = context;
        }

        // get all values url/api/temperature
        [HttpGet]
        public async Task<IActionResult> GetTemperatures()
        {
            var temperatures = await _context.Temperatures.ToListAsync();

            return Ok(temperatures);
        }

        // get value by id url/api/temperature/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTemperature(int id)
        {
            var temperature = await _context.Temperatures.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(temperature);
        }

        // get all values by given location url/api/temperature/location/{location}
        [HttpGet("{location}")]
        public async Task<IActionResult> GetTemperaturesByLocation(string location)
        {
            var temperatures = await _context.Temperatures.Where(x => x.SensorLocation == location).ToListAsync();

            return Ok(temperatures);
        }

        // Get latest record by location
        [HttpGet("{location}/last")]
        public async Task<IActionResult> GetLastTemperatureByLocation(string location)
        {
            var temperature = await _context.Temperatures.Where(x => x.SensorLocation == location).OrderByDescending(x => x.MeasuredAt).FirstOrDefaultAsync();

            return Ok(temperature);
        }

        // get average temperature in a location from last x hours
        [HttpGet("{location}/average/last/hours/{hours}")]
        public async Task<IActionResult> GetAverageTemperatureByLocationFromLastHours(string location, int hours)
        {

            var temperatures = await _context.Temperatures.Where(x => x.SensorLocation == location && x.MeasuredAt > DateTime.Now.AddHours(-hours)).ToListAsync();

            var temperature = temperatures.Select(x => x.TemperatureValue).DefaultIfEmpty(0).Average();

            var temperatureToReturn = String.Format("{0:F2}", temperature);

            var temperatureObj = new { temp = temperatureToReturn, message = $"Unit is Celsius. It's average temperature from last {hours} hours" };

            string json = JsonConvert.SerializeObject(temperatureObj);

            return Ok(json);
        }

        // get average temperature in a location from last x months
        [HttpGet("{location}/average/last/months/{months}")]
        public async Task<IActionResult> GetAverageTemperatureByLocationFromLastMonths(string location, int months)
        {

            var temperatures = await _context.Temperatures.Where(x => x.SensorLocation == location && x.MeasuredAt > DateTime.Now.AddMonths(-months)).ToListAsync();

            var temperature = temperatures.Select(x => x.TemperatureValue).DefaultIfEmpty(0).Average();

            var temperatureToReturn = String.Format("{0:F2}", temperature);

            var temperatureObj = new { temp = temperatureToReturn, message = "Celsius deegrees" };

            string json = JsonConvert.SerializeObject(temperatureObj);

            return Ok(json);

        }

        // Get all values by given device address url/api/temperature/device/{deviceAddress}
        [HttpGet("device/{deviceAddress}")]
        public async Task<IActionResult> GetTemperaturesByDeviceAddress(string deviceAddress)
        {
            var temperatures = await _context.Temperatures.Where(x => x.Address == deviceAddress).ToListAsync();

            return Ok(temperatures);
        }

        // upload values as json object (one by one for now) url/api/temperature/upload?Token={your token from down below}
        [HttpPost("upload")]
        public async Task<IActionResult> UploadTemperatures([FromQuery] string token, Temperature temperature)
        {
            if (token != "sampletoken")
                return Unauthorized("Wrong token");

            var temperatureToLog = new Temperature
            {
                MeasuredAt = DateTime.Now,
                Address = temperature.Address,
                TemperatureValue = temperature.TemperatureValue,
                SensorLocation = temperature.SensorLocation
            };

            var post = await _context.Temperatures.AddAsync(temperatureToLog);
            var save = await _context.SaveChangesAsync();

            return Ok();
        }

    }

}