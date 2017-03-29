using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZenithSociety.Data;
using ZenithSociety.Models.Business;
using Microsoft.AspNetCore.Authorization;
using ZenithSociety.Models;

namespace ZenithSociety.Controllers
{
    [Produces("application/json")]
    [Route("api/EventsAPI")]
    [Authorize]
    public class EventsAPIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IServiceProvider _services;

        public EventsAPIController(ApplicationDbContext context, IServiceProvider services)
        {
            _context = context;
            _services = services;
        }

        // GET: api/EventsAPI
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetEvents()
        {
            // current week
            List<EventJson> MonEvents = new List<EventJson>();
            List<EventJson> TueEvents = new List<EventJson>();
            List<EventJson> WedEvents = new List<EventJson>();
            List<EventJson> ThuEvents = new List<EventJson>();
            List<EventJson> FirEvents = new List<EventJson>();
            List<EventJson> SatEvents = new List<EventJson>();
            List<EventJson> SunEvents = new List<EventJson>();

            DateTime startDate = DateTime.Today.Date.AddDays(-(int)DateTime.Today.DayOfWeek), // prev sunday 00:00
                        endDate = startDate.AddDays(7); // next sunday 00:00 

            var start = startDate.AddHours(24.01);
            var end = endDate.AddHours(24.01);

            // select the current week events
            var eventsCurWeek = from e in _context.Events.Include(a => a.Activity)
                                where e.EventFrom > start && e.EventFrom <= end && e.IsActive
                                select e;


            // filter the data
            foreach (var eveRaw in eventsCurWeek)
            {
                var cc = MonEvents.Count;
                var eve = new EventJson()
                {
                    EventFrom = eveRaw.EventFrom,
                    EventTo = eveRaw.EventTo,
                    Activity = eveRaw.Activity.Description,
                    Date = eveRaw.EventFrom.ToString("dddd MMMM dd, yyyy"),
                    TimeFrom = String.Format("{0:t}", eveRaw.EventFrom),
                    TimeTo = String.Format("{0:t}", eveRaw.EventTo)
                };

                if ((int)eve.EventFrom.DayOfWeek == 1)
                {
                    MonEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 2)
                {
                    TueEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 3)
                {
                    WedEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 4)
                {
                    ThuEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 5)
                {
                    FirEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 6)
                {
                    SatEvents.Add(eve);
                }
                else
                {
                    SunEvents.Add(eve);
                }

            }

            // prepare the ViewModel
            HomeViewModel hvm = new HomeViewModel();
            hvm.Mon = MonEvents;
            hvm.Tue = TueEvents;
            hvm.Wed = WedEvents;
            hvm.Thu = ThuEvents;
            hvm.Fri = FirEvents;
            hvm.Sat = SatEvents;
            hvm.Sun = SunEvents;

            return Json(hvm);
           
        }

        // GET: api/EventsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event @event = await _context.Events.SingleOrDefaultAsync(m => m.EventId == id);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // PUT: api/EventsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent([FromRoute] int id, [FromBody] Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.EventId)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/EventsAPI
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Events.Add(@event);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventExists(@event.EventId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        // DELETE: api/EventsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event @event = await _context.Events.SingleOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return Ok(@event);
        }


        [HttpGet]
        [Route("Client/{id}")]   
        public JsonResult GetEventsByCode(int id)
        {
            // current week
            List<EventJson> MonEvents = new List<EventJson>();
            List<EventJson> TueEvents = new List<EventJson>();
            List<EventJson> WedEvents = new List<EventJson>();
            List<EventJson> ThuEvents = new List<EventJson>();
            List<EventJson> FirEvents = new List<EventJson>();
            List<EventJson> SatEvents = new List<EventJson>();
            List<EventJson> SunEvents = new List<EventJson>();

            int offset = id * 7;

            DateTime startDate = new DateTime(), endDate = new DateTime();

            if(id >= 0)
            {
                startDate = DateTime.Today.Date.AddDays(-(int)(DateTime.Today.DayOfWeek + offset)); // prev sunday 00:00
                endDate = startDate.AddDays(7); // next sunday 00:00 

            } else
            {
                int a = (int)DateTime.Today.Date.DayOfWeek;
                var off = (int)(7 - DateTime.Today.Date.DayOfWeek);
                id = (-id) - 1;
                startDate = DateTime.Today.Date.AddDays((off  +  id * 7)); // this sunday 00:00
                endDate = startDate.AddDays(7);
            }
            

            var start = startDate.AddHours(24.01);
            var end = endDate.AddHours(24.01);

            // select the current week events
            var eventsCurWeek = from e in _context.Events.Include(a => a.Activity)
                                where e.EventFrom > start && e.EventFrom <= end && e.IsActive
                                select e;


            // filter the data
            foreach (var eveRaw in eventsCurWeek)
            {
                var cc = MonEvents.Count;
                var eve = new EventJson()
                {
                    EventFrom = eveRaw.EventFrom,
                    EventTo = eveRaw.EventTo,
                    Activity = eveRaw.Activity.Description,
                    Date = eveRaw.EventFrom.ToString("dddd MMMM dd, yyyy"),
                    TimeFrom = String.Format("{0:t}", eveRaw.EventFrom),
                    TimeTo = String.Format("{0:t}", eveRaw.EventTo)
                };

                if ((int)eve.EventFrom.DayOfWeek == 1)
                {
                    MonEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 2)
                {
                    TueEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 3)
                {
                    WedEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 4)
                {
                    ThuEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 5)
                {
                    FirEvents.Add(eve);
                }
                else if ((int)eve.EventFrom.DayOfWeek == 6)
                {
                    SatEvents.Add(eve);
                }
                else
                {
                    SunEvents.Add(eve);
                }

            }

            // prepare the ViewModel
            HomeViewModel hvm = new HomeViewModel();
            hvm.Mon = MonEvents;
            hvm.Tue = TueEvents;
            hvm.Wed = WedEvents;
            hvm.Thu = ThuEvents;
            hvm.Fri = FirEvents;
            hvm.Sat = SatEvents;
            hvm.Sun = SunEvents;



            return Json(hvm);

        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}