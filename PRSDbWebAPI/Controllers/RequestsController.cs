using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSDbWebAPI.Models;

namespace PRSDbWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
          if (_context.Requests == null)
          {
              return NotFound();
          }
            return await _context.Requests.ToListAsync();
        }

        // GET: Reviews by User
        [HttpGet("reviews/{UserId}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsReviewByUser(int UserId) {
            if (_context.Requests == null) {
                return NotFound();
            }
            var requests = await _context.Requests.Where(x => x.UserId != UserId && x.Status == "REVIEW").ToListAsync();
            return requests;
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
          if (_context.Requests == null)
          {
              return NotFound();
          }
            var request = await _context.Requests.Include(x => x.User).Include(x => x.RequestLines)!.ThenInclude(x => x.Product)
                                                                                          .SingleOrDefaultAsync(x => x.Id == id);             
                                    

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: Review Method
        [HttpPut("review/{id}")]
        public async Task<IActionResult> PutRequestReview(int id) {
            var request = await _context.Requests.FindAsync(id);
            request!.Status = (request.Total <= 50 ? "APPROVED" : "REVIEW");
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: Approve Method
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> PutRequestApprove(int id) {
            var request = await _context.Requests.FindAsync(id);
            request!.Status = "APPROVED";
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: Reject Method
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> PutRequestReject(int id) {
            var request = await _context.Requests.FindAsync(id);
            request!.Status = "REJECTED";
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
          if (_context.Requests == null)
          {
              return Problem("Entity set 'AppDbContext.Requests'  is null.");
          }
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return (_context.Requests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
