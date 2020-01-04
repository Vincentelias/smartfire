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


        // GET: api/alarm/info/mac
        [HttpGet("info/{mac}")]
        public async Task<ActionResult<Devices>> GetDeviceInfo(string mac)
        {
            var devices = from m in _db.Devices
                                  select m;

            if (devices == null)
            {
                return NotFound();
            }

            devices = devices.Where(m => m.Mac == mac);

            return devices.FirstOrDefault();
        }


        // GET: api/alarm/info/id
        [HttpGet("fire-status/{id}")]
        public async Task<ActionResult<bool>> GetFireStatus(int id)
        {
            var devices = from m in _db.Devices
                          select m;

            if (devices == null)
            {
                return NotFound();
            }

            bool? isfire = devices.Where(m => m.Id == id).Select(d => d.IsFire).FirstOrDefault();
            return isfire;
        }


        // GET: api/alarm/id/toggle
        [HttpPut("toggle/{id}")]
        public async Task<IActionResult> ToggleDevice(int id)
        {

            bool? newIsFire = null;
            try
            {
                Devices device = _context.Devices.Where(d => d.Id == id).FirstOrDefault();
                if (device != null)
                {
                    device.IsFire = !device.IsFire;
                    newIsFire = device.IsFire;
                    _context.SaveChanges();

                }
            }
            catch (Exception)
            {
                return NotFound();
            }

            if (newIsFire!=null)
            {
                return Ok(newIsFire);
            }

            return NotFound();

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
