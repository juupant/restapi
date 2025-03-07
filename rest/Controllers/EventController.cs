using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTapi.Model;

namespace RESTapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly DataContext _context;

        public EventsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Event>> GetEvents()
        {
            return _context.Events.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Event> GetEvent(int id)
        {
            var @event = _context.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }
            return @event;
        }

        [HttpPost]
        public ActionResult<Event> PostEvent(Event @event)
        {
            var speaker = _context.Speakers.Find(@event.SpeakerId);
            if (speaker == null)
            {
                return NotFound("Speaker n0t found");
            }

            _context.Events.Add(@event);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEvent), new { id = @event.Id }, @event);
        }

        [HttpPut("{id}")]
        public IActionResult PutEvent(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            var speaker = _context.Speakers.Find(@event.SpeakerId);
            if (speaker == null)
            {
                return NotFound("Speaker not foundd");
            }

            var dbEvent = _context.Events.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (dbEvent == null)
            {
                return NotFound();
            }

            _context.Update(@event);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var @event = _context.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
