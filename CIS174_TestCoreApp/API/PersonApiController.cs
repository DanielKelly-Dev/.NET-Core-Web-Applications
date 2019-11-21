using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIS174_TestCoreApp.Controllers;
using CIS174_TestCoreApp.Filters;
using CIS174_TestCoreApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CIS174_TestCoreApp.API
{
    [Route("api/person")]
    [FeatureEnabled(IsEnabled = true)]
    [ValidateModel]
    [HandleException]
    [ApiController]

    public class PersonApiController : ControllerBase
    {
        public readonly PersonService _personService;
        public readonly PersonContext _context;
        public ILogger<PeopleController> _log;

        public PersonApiController(PersonService personService, ILogger<PeopleController> log)
        {
            _personService = personService;
            _log = log;
        }

        [AddLastModifiedHeader]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var detail = _personService.GetPersonDetails(id);
            if (detail == null)
            {
                _log.LogWarning("{PersonId} not found.", id);
            }
            return Ok(detail);
        }

        public void UpdatePerson(int id, UpdatePersonCommand command)
        {
            var person = _context.People.Find(id);

            if (person == null)
            {
                _log.LogWarning("{PersonId} failed to update", id);
                throw new Exception("Unable to find person");
            }

            person.FirstName = command.FirstName;
            person.LastName = command.LastName;
            person.BirthDate = command.BirthDate;
            person.City = command.City;
            person.State = command.State;

            _context.SaveChanges();
        }
    }
}