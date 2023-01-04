﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DYDN_Company.Models;
using Microsoft.AspNetCore.Cors;

namespace DYDN_Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AddCors")]
    public class BannerController : ControllerBase
    {
        private readonly AppDBContext _context;

        public BannerController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Banner
        [HttpGet]
        public IEnumerable<Banner> GetBanners()
        {
            return _context.Banners;
        }

        // GET: api/Banner/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBanner([FromRoute] int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var banner = await _context.Banners.FindAsync(id);

            if (banner == null)
            {
                return NotFound();
            }

            return Ok(banner);
        }

        // PUT: api/Banner/5
        [HttpPost]
        [Route("PutBanner")]
        public async Task<IActionResult> PutBanner( [FromBody] Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           

            _context.Entry(banner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               
            }

            return NoContent();
        }

        // POST: api/Banner
        [HttpPost]
        public async Task<IActionResult> PostBanner([FromBody] Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBanner", new { id = banner.Id }, banner);
        }

        // DELETE: api/Banner/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner([FromRoute] int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();

            return Ok(banner);
        }

        private bool BannerExists(int? id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
    }
}