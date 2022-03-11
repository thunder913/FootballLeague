using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService matchService;

        public MatchController(IMatchService matchService)
        {
            this.matchService = matchService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var matches = this.matchService.GetAllMatches();
                return Ok(matches);
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
                var match = this.matchService.GetMatchById(id);
                return Ok(match);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        
        [HttpGet("/teamMatches/{id}")]
        public IActionResult GetTeamMatches(int id)
        {
            try
            {
                var match = this.matchService.GetTeamMatches(id);
                return Ok(match);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(int homeTeamId, int awayTeamId, int homeGoals, int awayGoals, string date)
        {
            try
            {
                var match = await this.matchService.AddMatchAsync(homeTeamId, awayTeamId, homeGoals, awayGoals, date);
                return Ok(match);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, int homeTeamId, int awayTeamId, int homeGoals, int awayGoals, string date)
        {
            try
            {
                await this.matchService.UpdateMatchAsync(id, homeTeamId, awayTeamId, homeGoals, awayGoals, date);
                return Ok(id);
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.matchService.DeleteMatchAsync(id);
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
