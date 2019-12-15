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
    public class DevicesController : ControllerBase
    {
        private readonly smartfireContext _context;

        smartfireContext _db;

        public DevicesController(smartfireContext context, smartfireContext db)
        {
            _context = context;
            _db = db;
        }
        

        // GET: api/Devices
        [HttpGet]
        [Route("info")]
        public async Task<ActionResult<IEnumerable<Devices>>> GetDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Devices>> GetDevices(int id)
        {
            var devices = await _context.Devices.FindAsync(id);

            if (devices == null)
            {
                return NotFound();
            }

            return devices;
        }

        // GET: api/Devices/5
        [HttpGet("info/{mac}")]
        public async Task<ActionResult<Devices>> GetMacDevice(string mac)
        {
            var device = from m in _db.Devices
                                  select m;

            if (device == null)
            {
                return NotFound();
            }

            device = device.Where(m => m.Mac == mac);

            return device.FirstOrDefault();
        }


        [HttpPut("toggle/{MAC}")]
        public async Task<IActionResult> ToggleDevice(string mac, Devices devices)
        {
            if (mac != devices.Mac)
            {
                return BadRequest();
            }

            _context.Entry(devices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MacExists(mac))
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
        // PUT: api/Devices/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevices(int id, Devices devices)
        {
            if (id != devices.Id)
            {
                return BadRequest();
            }

            _context.Entry(devices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DevicesExists(id))
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

        // POST: api/Devices
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Devices>> PostDevices(Devices devices)
        {
            _context.Devices.Add(devices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevices", new { id = devices.Id }, devices);
        }
        


        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Devices>> DeleteDevices(int id)
        {
            var devices = await _context.Devices.FindAsync(id);
            if (devices == null)
            {
                return NotFound();
            }

            _context.Devices.Remove(devices);
            await _context.SaveChangesAsync();

            return devices;
        }

        private bool DevicesExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }

        private bool MacExists(string mac)
        {
            return _context.Devices.Any(e => e.Mac == mac);
        }
    }
}
