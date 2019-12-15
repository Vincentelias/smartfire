﻿using System;
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

        // GET: api/Measurements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measurements>>> GetMeasurements()
        {
            return await _context.Measurements.ToListAsync();
        }

        // GET: api/Measurements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Measurements>> GetMeasurements(int id)
        {
            var measurements = await _context.Measurements.FindAsync(id);

            if (measurements == null)
            {
                return NotFound();
            }

            return measurements;
        }

        [HttpGet("histroy/{id}")]
        public async Task<IEnumerable<Measurements>> GetHistoryMeasurements(int id, string startDate, string endDate, int limit) 
        {

            DateTime startDateTime = DateTime.ParseExact(startDate, "yyyyMMddhhmm", null);
            DateTime endDateTime = DateTime.ParseExact(endDate, "yyyyMMddhhmm", null);

            var measurement = from m in _db.Measurements
                              select m;

            measurement = measurement.Where(m => m.DeviceId == id).Take(limit);

            measurement = measurement.Where(m => m.MeasuredOn >= startDateTime || m.MeasuredOn < endDateTime);




            return measurement.ToList();
        }

        // GET: api/Measurements/mac
        [HttpGet("history/{id}")]
        public async Task<IEnumerable<Measurements>> GetMacMeasurements(int id)
        {
            var measurements = from m in _db.Measurements
                              select m;

            measurements = measurements.Where(m => m.DeviceId == id);

            return measurements.ToList();
        }

        


        // PUT: api/Measurements/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeasurements(int id, Measurements measurements)
        {
            if (id != measurements.Id)
            {
                return BadRequest();
            }

            _context.Entry(measurements).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasurementsExists(id))
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

        // POST: api/Measurements
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Measurements>> PostMeasurements(Measurements measurements)
        {
            _context.Measurements.Add(measurements);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeasurements", new { id = measurements.Id }, measurements);
        }

        // DELETE: api/Measurements/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Measurements>> DeleteMeasurements(int id)
        {
            var measurements = await _context.Measurements.FindAsync(id);
            if (measurements == null)
            {
                return NotFound();
            }

            _context.Measurements.Remove(measurements);
            await _context.SaveChangesAsync();

            return measurements;
        }

        private bool MeasurementsExists(int id)
        {
            return _context.Measurements.Any(e => e.Id == id);
        }
    }
}