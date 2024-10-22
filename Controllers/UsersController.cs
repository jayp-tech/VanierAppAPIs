using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VanierAppAPIs.Data;

namespace VanierAppAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly VanierDBContext _context;

        public UsersController(VanierDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetUsers()
        {
            // Fetch all the usernames
            return await _context.Users
                                 .Select(u => u.Username)
                                 .ToListAsync();
        }
       
    }
}
