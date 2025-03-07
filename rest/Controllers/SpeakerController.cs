using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTapi.Model;

namespace RESTapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpeakersController : ControllerBase
    {
        private readonly DataContext _context;

        public SpeakersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Speaker>> GetSpeakers()
        {
            return _context.Speakers.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Speaker> GetSpeaker(int id)
        {
            var speaker = _context.Speakers.Find(id);
            if (speaker == null)
            {
                return NotFound();
            }
            return speaker;
        }

        [HttpPost]
        public ActionResult<Speaker> PostSpeaker(Speaker speaker)
        {
            if (!speaker.Email.Contains("@"))
            {
                return BadRequest("Email must contain @ symbol");
            }

            _context.Speakers.Add(speaker);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetSpeaker), new { id = speaker.Id }, speaker);
        }

        [HttpPut("{id}")]
        public IActionResult PutSpeaker(int id, Speaker speaker)
        {
            if (id != speaker.Id)
            {
                return BadRequest();
            }

            if (!speaker.Email.Contains("@"))
            {
                return BadRequest("Email must contain @ symbol");
            }

            var dbSpeaker = _context.Speakers.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (dbSpeaker == null)
            {
                return NotFound();
            }

            _context.Update(speaker);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSpeaker(int id)
        {
            var speaker = _context.Speakers.Find(id);
            if (speaker == null)
            {
                return NotFound();
            }

            _context.Speakers.Remove(speaker);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
