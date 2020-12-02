using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareMeet.Models;

namespace ShareMeet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetUpsController : ControllerBase
    {
        private readonly UsersContext _context;

        public MeetUpsController(UsersContext context)
        {
            _context = context;
        }

        // GET: api/MeetUps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetUp>>> GetMeetUps()
        {
            return await _context.MeetUps.ToListAsync();
        }

        // GET: api/MeetUps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MeetUp>> GetMeetUp(int id)
        {
            var meetUp = await _context.MeetUps.FindAsync(id);

            if (meetUp == null)
            {
                return NotFound();
            }

            return meetUp;
        }

        // PUT: api/MeetUps/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetUp(int id, MeetUp meetUp)
        {
            if (id != meetUp.Id_meetup)
            {
                return BadRequest();
            }

            _context.Entry(meetUp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetUpExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MeetUps
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MeetUp>> PostMeetUp(MeetUp meetUp)
        {
            _context.MeetUps.Add(meetUp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeetUp", new { id = meetUp.Id_meetup }, meetUp);
        }

        // DELETE: api/MeetUps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MeetUp>> DeleteMeetUp(int id)
        {
            var meetUp = await _context.MeetUps.FindAsync(id);
            if (meetUp == null)
            {
                return NotFound();
            }

            _context.MeetUps.Remove(meetUp);
            await _context.SaveChangesAsync();

            return meetUp;
        }

        private bool MeetUpExists(int id)
        {
            return _context.MeetUps.Any(e => e.Id_meetup == id);
        }
    }
}
