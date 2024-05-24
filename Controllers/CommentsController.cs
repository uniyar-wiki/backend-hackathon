
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniYarWiki.Data;
using UniYarWiki.Models;

namespace UniYarWiki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int teacherId)
        {
            var comments = await (from comment in _context.Comments
                                  join user in _context.Users on comment.UserId equals user.Id
                                  where comment.TeacherId == teacherId
                                  orderby comment.Time descending
                                  select new CommentDto
                                  {
                                      Id = comment.Id,
                                      Content = comment.Content,
                                      Time = comment.Time,
                                      First = user.First,
                                      Last = user.Last
                                  }).ToListAsync();

            return Ok(comments);
        }

        public class CommentDto
        {
            public int Id { get; set; }
            public string Content { get; set; }
            public DateTime Time { get; set; }
            public string First { get; set; }
            public string Last { get; set; }
        }

        [HttpDelete("deleteAll")]
        public async Task<IActionResult> DeleteAllComments()
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Comments");
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            comment.Time = DateTime.UtcNow;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComments), new { teacherId = comment.TeacherId }, comment);
        }
    }
}
