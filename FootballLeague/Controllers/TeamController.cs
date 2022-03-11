using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FootballLeague.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService teamService;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var teams = this.teamService.GetAllTeams();
                return Ok(teams);
            }
            catch (Exception)
            {
                return StatusCode(500, "There was an error, try again!");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var team = this.teamService.GetTeamById(id);
                if (team == null)
                {
                    return BadRequest("There is no team with such id!");
                }
               return Ok(JsonConvert.SerializeObject(this.teamService.GetTeamById(id)));
            }
            catch (Exception)
            {
                return StatusCode(500, "There was an error, try again later!");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(string teamName, int cityId, int leagueId)
        {
            try
            {
                var id = await this.teamService.AddTeamAsync(teamName, leagueId, cityId);
                return Ok(id.ToString());
            }
            catch(ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, string teamName, int cityId, int leagueId)
        {
            try
            {
                await this.teamService.UpdateTeamInformationAsync(id, teamName, cityId, leagueId);
                return Ok();
            }catch(ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.teamService.DeleteTeamByIdAsync(id);
                return Ok();
            }catch(ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
