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


        // GET: api/alarm/connected-alarms/5
        [HttpGet("connected-alarms/{id}")]
        public async Task<IEnumerable<ConnectedDevices>> GetConnectedDevices(int id)
        {

            var connectedDevice = from m in _db.ConnectedDevices
                                  select m;

            connectedDevice = connectedDevice.Where(m => m.DeviceId == id);
            return connectedDevice.ToList();
        }


        // POST: api/alarm/connected/alarms
        [HttpPost]
        [Route("connected-alarms")]
        public async Task<ActionResult<ConnectedDevices>> PostConnectedDevices(ConnectedDevices connectedDevices)
        {
            _context.ConnectedDevices.Add(connectedDevices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConnectedDevices", new { id = connectedDevices.Id }, connectedDevices);
        }


        // DELETE: api/alarm/connected-alarms/id
        [HttpDelete("connected-alarms/{id}")]
        public async Task<ActionResult<ConnectedDevices>> DeleteConnectedDevices(int id, int connectedDeviceId)
        {

            var connectedDevice = from m in _db.ConnectedDevices
                                  select m;

            connectedDevice = connectedDevice.Where(m => m.DeviceId == id && m.ConnectedDeviceId == connectedDeviceId);
            var removeConnectedDevice = connectedDevice.FirstOrDefault();
            _context.ConnectedDevices.Remove(removeConnectedDevice);
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }

        private bool ConnectedDevicesExists(int id)
        {
            return _context.ConnectedDevices.Any(e => e.Id == id);
        }
    }
}
