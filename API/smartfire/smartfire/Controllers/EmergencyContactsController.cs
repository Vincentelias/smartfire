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


        // GET: api/alarm/emergency-contacts/id
        [HttpGet("emergency-contacts/{id}")]
        public async Task<IEnumerable<EmergencyContacts>> GetEmergencyContactsWithDeviceId(int id)
        {

            var emergencyContact = from m in _db.EmergencyContacts
                                   select m;


            emergencyContact = emergencyContact.Where(m => m.DeviceId == id);
            return emergencyContact.ToList();
        }


        // POST: api/alarm/emergency-contacts
        [HttpPost]
        [Route("emergency-contacts")]
        public async Task<ActionResult<EmergencyContacts>> PostEmergencyContacts(EmergencyContacts emergencyContacts)
        {
            _context.EmergencyContacts.Add(emergencyContacts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmergencyContacts", new { id = emergencyContacts.Id }, emergencyContacts);
        }


        // DELETE: api/alarm/emergency-contacts/id
        [HttpDelete("emergency-contacts/{id}")]
        public async Task<ActionResult<EmergencyContacts>> DeleteEmergencyContacts(int id, string name)
        {
            var emergencyContacts = from m in _db.EmergencyContacts
                                    select m;

            emergencyContacts = emergencyContacts.Where(m => m.DeviceId == id && m.Name == name);
            var removeEmergencyContacts = emergencyContacts.FirstOrDefault();
            _context.EmergencyContacts.Remove(removeEmergencyContacts);
            await _context.SaveChangesAsync();

            return StatusCode(200);

        }

        private bool EmergencyContactsExists(int id)
        {
            return _context.EmergencyContacts.Any(e => e.Id == id);
        }
    }
}
