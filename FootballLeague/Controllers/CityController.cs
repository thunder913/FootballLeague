using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FootballLeague.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService cityService;

        public CityController(ICityService cityService)
        {
            this.cityService = cityService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var cities = this.cityService.GetCities();
                return Ok(cities);
            }catch(Exception)
            {
                return StatusCode(500, "There was an error!");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var city = this.cityService.GetCityDtoById(id);
                if (city == null)
                {
                    return BadRequest("No city found!");
                }
                return Ok(city);
            }catch(Exception)
            {
                return StatusCode(500, "Something went wrong, try again!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(string cityName, int countryId)
        {
            try
            {
                var cityId = await this.cityService
                            .AddCityAsync(cityName, countryId);
                return Ok(cityId);
            }catch(ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch(Exception)
            {
                return StatusCode(500, "There was an error, saving the data.");
            }


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, string cityName, int countryId)
        {
            try
            {
                await this.cityService.UpdateCityAsync(id, cityName, countryId);
                return Ok();
            }
            catch(ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "There was an error, try again!");
                throw;
            }
        }

        // DELETE api/<CityController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.cityService.DeleteCityAsync(id);
                return Ok();
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "There was a server error, try again!");
                throw;
            }
        }
    }
}
