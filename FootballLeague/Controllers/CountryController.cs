using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService countryService;

        public CountryController(ICountryService countryService)
        {
            this.countryService = countryService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var countries = this.countryService.GetCountries();
                return Ok(countries);
            }
            catch (Exception)
            {
                return StatusCode(500, "There was an error!");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var country = this.countryService.GetCountryById(id);
                if (country == null)
                {
                    return BadRequest("No country found!");
                }
                return Ok(country);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong, try again!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(string countryName)
        {
            try
            {
                var countryId = await this.countryService
                            .AddCountryAsync(countryName);
                return Ok(countryId);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "There was an error, saving the data.");
            }


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, string countryName)
        {
            try
            {
                await this.countryService.UpdateCountryAsync(id, countryName);
                return Ok();
            }
            catch (ArgumentException ae)
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
                await this.countryService.DeleteCountryAsync(id);
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
