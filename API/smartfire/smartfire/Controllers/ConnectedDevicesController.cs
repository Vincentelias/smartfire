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
    public class ConnectedDevicesController : ControllerBase
    {
        private readonly smartfireContext _context;

        smartfireContext _db;


        public ConnectedDevicesController(smartfireContext context, smartfireContext db)
        {
            _context = context;
            _db = db;
        }

        // GET: api/ConnectedDevices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConnectedDevices>>> GetConnectedDevices()
        {
            return await _context.ConnectedDevices.ToListAsync();
        }

        // GET: api/ConnectedDevices/5
        [HttpGet("connected-alarms/{id}")]
        public async Task<IEnumerable<ConnectedDevices>> GetConnectedDevices(int id)
        {



            var connectedDevice = from m in _db.ConnectedDevices
                                  select m;

            connectedDevice = connectedDevice.Where(m => m.DeviceId == id);



            return connectedDevice.ToList();
        }

        // PUT: api/ConnectedDevices/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConnectedDevices(int id, ConnectedDevices connectedDevices)
        {
            if (id != connectedDevices.Id)
            {
                return BadRequest();
            }

            _context.Entry(connectedDevices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConnectedDevicesExists(id))
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

        // POST: api/ConnectedDevices
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("connected-alarms")]
        public async Task<ActionResult<ConnectedDevices>> PostConnectedDevices(ConnectedDevices connectedDevices)
        {
            _context.ConnectedDevices.Add(connectedDevices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConnectedDevices", new { id = connectedDevices.Id }, connectedDevices);
        }

        // DELETE: api/ConnectedDevices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ConnectedDevices>> DeleteConnectedDevices(int id)
        {
            var connectedDevices = await _context.ConnectedDevices.FindAsync(id);
            
            
            
            if (connectedDevices == null)
            {
                return NotFound();
            }

            _context.ConnectedDevices.Remove(connectedDevices);
            await _context.SaveChangesAsync();

            return connectedDevices;
        }

        private bool ConnectedDevicesExists(int id)
        {
            return _context.ConnectedDevices.Any(e => e.Id == id);
        }
    }
}
