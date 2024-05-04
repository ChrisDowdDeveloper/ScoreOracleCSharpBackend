using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupMemberController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public GroupMemberController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            var members = _context.GroupMembers.ToList();
        
            return Ok(members);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var member = _context.GroupMembers.Find(id);

            if(member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }
    }
}