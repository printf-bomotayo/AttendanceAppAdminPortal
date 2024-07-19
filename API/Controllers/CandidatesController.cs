using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CandidatesController(AppDbContext context)
        {
            _context = context;            
        }

        [HttpGet]
        public async Task<ActionResult<List<Candidate>>> GetCandidates()
        {
            return await _context.Candidates.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Candidate>> GetCandidate(int id)
        {
            return await _context.Candidates.FindAsync(id);
        }
    }
}