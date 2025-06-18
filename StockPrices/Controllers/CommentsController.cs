using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockPrices.Core.Entities;
using StockPrices.Infrastructure;
using System.Security.Claims;

namespace StockPrices.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly StockPricesDbContext _context;

        public CommentsController(StockPricesDbContext context)
        {
            _context = context;
        }

        // GET: /api/comments/{symbol}
        [HttpGet("{symbol}")]
        public ActionResult<IEnumerable<Comment>> GetUserCommentsForSymbol(string symbol)
        {
            var userId = GetUserId();

            var comments = _context.Comments
                .Where(c => c.UserId == userId && c.Symbol == symbol)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return Ok(comments);
        }

        // POST: /api/comments
        [HttpPost]
        public IActionResult AddComment([FromBody] CreateCommentDto dto)
        {
            var userId = GetUserId();

            var comment = new Comment
            {
                UserId = userId,
                Symbol = dto.Symbol,
                Text = dto.Text,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok(comment);
        }

        // DELETE: /api/comments/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            var userId = GetUserId();

            var comment = _context.Comments.FirstOrDefault(c => c.Id == id && c.UserId == userId);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return NoContent();
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim.Value);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateComment(int id, [FromBody] string newText)
        {
            var userId = GetUserId();

            var comment = _context.Comments.FirstOrDefault(c => c.Id == id && c.UserId == userId);
            if (comment == null)
            {
                return NotFound();
            }

            comment.Text = newText;
            _context.SaveChanges();

            return Ok(comment);
        }

    }
}
