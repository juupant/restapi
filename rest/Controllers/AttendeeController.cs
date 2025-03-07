using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTapi.Model;

namespace RESTapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendeesController : ControllerBase
    {
        private readonly DataContext _context;

        public AttendeesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Attendee>> GetAttendees(int? daysBeforeEvent = null)
        {
            var query = _context.Attendees.AsQueryable();
            if (daysBeforeEvent.HasValue)
            {
                query = query
                    .Join(_context.Events,
                        a => a.EventId,
                        e => e.Id,
                        (a, e) => new { Attendee = a, Event = e })
                    .Where(x => (x.Event.Date - x.Attendee.RegistrationTime).Days >= daysBeforeEvent.Value)
                    .Select(x => x.Attendee);
            }


            return query.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Attendee> GetAttendee(int id)
        {
            var attendee = _context.Attendees.Find(id);
            if (attendee == null)
            {
                return NotFound();
            }
            return attendee;
        }

        [HttpPost]
        public ActionResult<Attendee> PostAttendee(Attendee attendee)
        {

            if (!attendee.Email.Contains("@"))
            {
                return BadRequest("Email must contain @ symbol");
            }


            var evt = _context.Events.Find(attendee.EventId);
            if (evt == null)
            {
                return NotFound("Event not found");
            }

 
            if (attendee.RegistrationTime > evt.Date)
            {
                return BadRequest("Registration time cannot be after the event date");
            }


            var existingAttendee = _context.Attendees
                .FirstOrDefault(a => a.EventId == attendee.EventId && a.Email == attendee.Email);
            if (existingAttendee != null)
            {
                return BadRequest("An attendee with this email is already registered for this event");
            }


            var speaker = _context.Speakers.Find(evt.SpeakerId);
            if (speaker != null && speaker.Email == attendee.Email)
            {
                return BadRequest("The speaker cannot register as an attendee");
            }

            _context.Attendees.Add(attendee);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAttendee), new { id = attendee.Id }, attendee);
        }

        [HttpPut("{id}")]
        public IActionResult PutAttendee(int id, Attendee attendee)
        {
            if (id != attendee.Id)
            {
                return BadRequest();
            }


            if (!attendee.Email.Contains("@"))
            {
                return BadRequest("Email must contain @ sümbol");
            }


            var evt = _context.Events.Find(attendee.EventId);
            if (evt == null)
            {
                return NotFound("Event not found");
            }

            if (attendee.RegistrationTime > evt.Date)
            {
                return BadRequest("Registration time cannot be after the event date");
            }

            var existingAttendee = _context.Attendees
                .FirstOrDefault(a => a.EventId == attendee.EventId &&
                                   a.Email == attendee.Email &&
                                   a.Id != attendee.Id);
            if (existingAttendee != null)
            {
                return BadRequest("Another attendee with this email is already registered for this event");
            }

            var speaker = _context.Speakers.Find(evt.SpeakerId);
            if (speaker != null && speaker.Email == attendee.Email)
            {
                return BadRequest("The speaker cannot register as an attendee");
            }

            var dbAttendee = _context.Attendees.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (dbAttendee == null)
            {
                return NotFound();
            }

            _context.Update(attendee);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAttendee(int id)
        {
            var attendee = _context.Attendees.Find(id);
            if (attendee == null)
            {
                return NotFound();
            }

            _context.Attendees.Remove(attendee);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
