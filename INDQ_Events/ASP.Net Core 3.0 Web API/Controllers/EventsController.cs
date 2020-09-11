using ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities;
using ASP.Net_Core_3._0_Web_API.ApplicationCore.Exceptions;
using ASP.Net_Core_3._0_Web_API.ApplicationCore.Specifications;
using ASP.Net_Core_3._0_Web_API.Infraestructure.Data;
using ASP.Net_Core_3._0_Web_API.ViewModels.Attendance;
using ASP.Net_Core_3._0_Web_API.ViewModels.Event;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EventsController : IndqControllerBase
    {
        private readonly EventRepository _repository;
        private readonly AttendanceRepository _attendanceRepository;

        public EventsController(EventRepository repository, AttendanceRepository attendanceRepository)
        {
            _repository = repository;
            _attendanceRepository = attendanceRepository;
        }

        [HttpPost]
        [Route("/events")]
        public async Task<ActionResult> CreateAsync(ViewModels.Event.CreateViewModel model)
        {
            try
            {
                if (model.Date < DateTime.Now)
                    return BadRequest("La fecha del evento no debe ser menor al día de hoy.");

                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                Event newEvent = new Event
                {
                    Title = model.Title,
                    Description = model.Description,
                    Date = model.Date,
                    Image = model.Image,
                    Attendances = model.Attendances,
                    WillYouAttend = model.WillYouAttend,
                    Location = geometryFactory.CreatePoint(new Coordinate(double.Parse(model.Location[1].ToString()), double.Parse(model.Location[0].ToString())))
                };

                await _repository.Add(newEvent);

                return Ok(newEvent.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("/events")]
        public async Task<ActionResult> GetEvents(int? page, int? limit, decimal? lat, decimal? lng, string title)
        {
            try
            {
                IEnumerable<EventViewModel> _items = null;

                int _page = page ?? 0;
                int _limit = limit ?? 0;

                int _totalpages = 0;

                _limit = _limit == 0 || _limit > 10 ? 10 : _limit;

                if (_page > 0 && _limit > 0)
                {
                    Expression<Func<Event, bool>> criteria = null;

                    if (string.IsNullOrEmpty(title) && lat == null && lng == null)
                        criteria = e => true;
                    else if (!string.IsNullOrEmpty(title))
                        criteria = e => e.Title == title;

                    IReadOnlyList<Event> eventsTask = null;
                    EventFilterPaginatedSpecification filterPaginatedSpecification = null;

                    if (criteria != null)
                    {
                        filterPaginatedSpecification =
                            new EventFilterPaginatedSpecification(_limit * (_page - 1), _limit, criteria);

                        eventsTask = await _repository.List(filterPaginatedSpecification);
                    }
                    else if (lat != null && lng != null)
                    {
                        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                        var myLocation =
                            geometryFactory.CreatePoint(new Coordinate(double.Parse(lng.ToString()), double.Parse(lat.ToString())));

                        criteria = e => e.Location.IsWithinDistance(myLocation, 3000);

                        var events = await _repository.GetWhere(criteria);
                        events = events.OrderBy(o => o.Location.Distance(myLocation));

                        int _skip = _limit * (_page - 1);

                        eventsTask = events.Skip(_skip).Take(_limit).ToList();
                    }

                    if (eventsTask != null)
                    {
                        _items = eventsTask.Select(s => new EventViewModel()
                        {
                            Id = s.EventId,
                            Title = s.Title,
                            Description = s.Description,
                            Date = s.Date,
                            Image = s.Image,
                            Attendances = s.Attendances,
                            WillYouAttend = s.WillYouAttend
                        });

                        int totalItems = await _repository.CountWhere(criteria);
                        _totalpages = int.Parse(Math.Ceiling(((decimal)totalItems / _limit)).ToString());
                    }
                }
                else
                {
                    var eventsTask = await _repository.GetAll();

                    if (eventsTask.Any())
                    {
                        _items = eventsTask.Select(s => new EventViewModel()
                        {
                            Id = s.EventId,
                            Title = s.Title,
                            Description = s.Description,
                            Date = s.Date,
                            Image = s.Image,
                            Attendances = s.Attendances,
                            WillYouAttend = s.WillYouAttend
                        });

                        int totalItems = await _repository.CountAll();

                        _items = _limit > 0 ? _items.Take(_limit) : _items;
                    }

                }

                if (_page == 0) _page = 1;
                if (_totalpages == 0) _totalpages = 1;

                return Ok(new EventViewModelResponse()
                {
                    Page = _page,
                    Pages = _totalpages,
                    Items = _items
                });
            }
            catch (Exception ex)
            {
                LogException(ex);
                return StatusCode(500, "Error al obtener los eventos");
            }
        }

        [HttpPost]
        [HttpDelete]
        [Route("/events/[action]/{eventId}")]
        public async Task<ActionResult<AttendanceResponseViewModel>> Attendance(int eventId)
        {
            AttendanceResponseViewModel ret = null;

            try
            {
                var _event = await _repository.GetById(eventId);
                if (_event == null)
                    return StatusCode(404, "Evento no encontrado");

                var attendance = new Attendance() { EventId = eventId, UserName = User.Identity.Name };

                if (HttpContext.Request.Method == HttpMethods.Post)
                    await _repository.ConfirmAttendace(attendance, _attendanceRepository);
                else
                    await _repository.CancelAttendace(attendance, _attendanceRepository);

                int count = await _attendanceRepository.CountWhere(p => p.EventId == eventId);

                ret = new AttendanceResponseViewModel() { Id = eventId.ToString(), Attendances = count };
            }
            catch (AttendanceExistsException ex)
            {
                LogException(ex);
                return StatusCode(403, "Evento registrado para asistencia actualmente");
            }
            catch (AttendenceNotExistsException ex)
            {
                LogException(ex);
                return StatusCode(403, "Asistencia no registrada anteriormente");
            }
            catch (Exception ex)
            {
                LogException(ex);
                string _action = HttpContext.Request.Method == HttpMethods.Delete ? "cancelar" : "registrar";
                return StatusCode(400, $"Error al {_action} la asistencia al evento");
            }

            return Ok(ret);

        }
    }
}
