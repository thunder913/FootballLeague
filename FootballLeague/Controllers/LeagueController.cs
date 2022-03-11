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
    public class LeagueController : ControllerBase
    {
        private readonly ILeagueService leagueService;

        public LeagueController(ILeagueService leagueService)
        {
            this.leagueService = leagueService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var cities = this.leagueService.GetLeagues();
                return Ok(cities);
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
                var city = this.leagueService.GetLeagueDtoById(id);
                if (city == null)
                {
                    return BadRequest("No city found!");
                }
                return Ok(city);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong, try again!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(string leagueName, int countryId)
        {
            try
            {
                var cityId = await this.leagueService
                            .AddLeagueAsync(leagueName, countryId);
                return Ok(cityId);
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
        public async Task<IActionResult> Put(int id, string leagueName, int countryId)
        {
            try
            {
                await this.leagueService.UpdateLeagueAsync(id, leagueName, countryId);
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
                await this.leagueService.DeleteCountryAsync(id);
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

        [HttpGet("getRanking/{id}")]
        public IActionResult GetRanking(int id)
        {
            try
            {
                var league = this.leagueService.GetLeagueRanking(id);
                if (league == null)
                {
                    return BadRequest("There is no league with this id!");
                }
                return Ok(league);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong, try again!");
            }
        }
    }
}
