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
    public class MeasurementsController : ControllerBase
    {
        private readonly smartfireContext _context;

        smartfireContext _db;

        public MeasurementsController(smartfireContext context, smartfireContext db)
        {
            _context = context;
            _db = db;
        }


        // GET: api/alarm/measurements/id
        [HttpGet("measurements/{id}")]
        public async Task<IEnumerable<Measurements>> GetDeviceMeasurements(int id)
        {
            var measurements = from m in _db.Measurements
                              select m;

            measurements = measurements.Where(m => m.DeviceId == 1).OrderByDescending(m => m.MeasuredOn).Take(50);

            return measurements.ToList();
        }


        // GET: api/alarm/measurements/id/latest
        [HttpGet("measurements/{id}/latest")]
        public async Task<Measurements> GetLatestMeasurement(int id)
        {
            var measurements = from m in _db.Measurements
                               select m;

            measurements = measurements.Where(m => m.DeviceId == 1).OrderByDescending(m => m.MeasuredOn).Take(1);

            return measurements.ToList()[0];
        }


        // GET: api/alarm/measurements/averages/id
        [HttpGet("measurements/averages/{id}")]
        public async Task<IQueryable<Measurements>> GetAverageMeasurements(int id)
        {

            var measurements = _db.Measurements
               .Where(m => m.DeviceId == id)
               .GroupBy(i => new { i.DeviceId, i.MeasuredOn })
               .Select(g => new Measurements
               {
                   DeviceId = g.Key.DeviceId,
                   Temperature = g.Average(i => i.Temperature),
                   Humidity = g.Average(i => i.Humidity),
                   MeasuredOn = g.Key.MeasuredOn,
               }).OrderByDescending(m => m.MeasuredOn).Take(7);
            return measurements;
        }


        // POST: api/alarm/measurements
        [HttpPost("measurements")]
        public async Task<ActionResult<Measurements>> PostMeasurements(Measurements measurements)
        {
            _context.Measurements.Add(measurements);
            await _context.SaveChangesAsync();

            return Ok();
        }


        private bool MeasurementsExists(int id)
        {
            return _context.Measurements.Any(e => e.Id == id);
        }
    }
}
