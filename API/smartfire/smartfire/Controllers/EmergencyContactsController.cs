using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smartfire.Model;

namespace smartfire.Controllers
{
    [Route("alarm")]
    [ApiController]
    public class EmergencyContactsController : ControllerBase
    {
        private readonly smartfireContext _context;

        smartfireContext _db;


        public EmergencyContactsController(smartfireContext context, smartfireContext db)
        {
            _context = context;
            _db = db;
        }

        // GET: api/EmergencyContacts
        [HttpGet]
        [Route("emergency-contacts")]
        public async Task<ActionResult<IEnumerable<EmergencyContacts>>> GetEmergencyContacts()
        {
            return await _context.EmergencyContacts.ToListAsync();
        }

        // GET: api/EmergencyContacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmergencyContacts>> GetEmergencyContacts(int id)
        {
            var emergencyContacts = await _context.EmergencyContacts.FindAsync(id);

            if (emergencyContacts == null)
            {
                return NotFound();
            }

            return emergencyContacts;
        }

        [HttpGet("emergency-contacts/{id}")]
        public async Task<IEnumerable<EmergencyContacts>> GetEmergencyContactsWithDeviceId(int id)
        {

            var emergencyContact = from m in _db.EmergencyContacts
                                   select m;


            emergencyContact = emergencyContact.Where(m => m.DeviceId == id);


            

            return emergencyContact.ToList();
        }

        // PUT: api/EmergencyContacts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmergencyContacts(int id, EmergencyContacts emergencyContacts)
        {
            if (id != emergencyContacts.Id)
            {
                return BadRequest();
            }

            _context.Entry(emergencyContacts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmergencyContactsExists(id))
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

        // POST: api/EmergencyContacts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("emergency-contacts")]
        public async Task<ActionResult<EmergencyContacts>> PostEmergencyContacts(EmergencyContacts emergencyContacts)
        {
            _context.EmergencyContacts.Add(emergencyContacts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmergencyContacts", new { id = emergencyContacts.Id }, emergencyContacts);
        }

        // DELETE: api/EmergencyContacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmergencyContacts>> DeleteEmergencyContacts(int id)
        {
            var emergencyContacts = await _context.EmergencyContacts.FindAsync(id);
            if (emergencyContacts == null)
            {
                return NotFound();
            }

            _context.EmergencyContacts.Remove(emergencyContacts);
            await _context.SaveChangesAsync();

            return emergencyContacts;
        }

        private bool EmergencyContactsExists(int id)
        {
            return _context.EmergencyContacts.Any(e => e.Id == id);
        }
    }
}
