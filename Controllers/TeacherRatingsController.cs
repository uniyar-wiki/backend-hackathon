using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UniYarWiki.Data;
using UniYarWiki.Models;

namespace UniYarWiki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherRatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeacherRatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("rate")]
        public async Task<IActionResult> RateTeacher(TeacherRatings rating)
        {
            var existingRating = await _context.TeacherRatings
                .FirstOrDefaultAsync(r => r.TeacherId == rating.TeacherId && r.UserId == rating.UserId);

            if (existingRating != null)
            {
                existingRating.KnowledgeRating = rating.KnowledgeRating;
                existingRating.TeachingSkillRating = rating.TeachingSkillRating;
                existingRating.CommunicationRating = rating.CommunicationRating;
                existingRating.EasinessRating = rating.EasinessRating;
            }
            else
            {
                _context.TeacherRatings.Add(rating);
            }

            await _context.SaveChangesAsync();

            var averageRatings = await _context.TeacherRatings
                .Where(r => r.TeacherId == rating.TeacherId)
                .GroupBy(r => r.TeacherId)
                .Select(g => new
                {
                    KnowledgeRating = g.Average(r => r.KnowledgeRating),
                    TeachingSkillRating = g.Average(r => r.TeachingSkillRating),
                    CommunicationRating = g.Average(r => r.CommunicationRating),
                    EasinessRating = g.Average(r => r.EasinessRating)
                })
                .FirstOrDefaultAsync();

            var teacher = await _context.Teachers.FindAsync(rating.TeacherId);
            if (teacher != null)
            {
                teacher.KnowledgeRating = averageRatings.KnowledgeRating;
                teacher.TeachingSkillRating = averageRatings.TeachingSkillRating;
                teacher.CommunicationRating = averageRatings.CommunicationRating;
                teacher.EasinessRating = averageRatings.EasinessRating;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
