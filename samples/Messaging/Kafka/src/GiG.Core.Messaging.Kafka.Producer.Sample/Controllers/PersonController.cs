using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Producer.Sample.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    public class PersonController : ControllerBase
    {
        private readonly IKafkaProducer<string, Person> _kafkaProducer;
        private readonly IDictionary<string, Person> _dataStore = new Dictionary<string, Person>();
        
        public PersonController(IKafkaProducer<string, Person> kafkaProducer)
        {
            _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
        }

        [HttpGet("{id}")]
        public ActionResult<Person> GetPersonById(Guid id)
        {
            var person = _dataStore[id.ToString()];
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson()
        {
            var person = Person.Default;
            var messageId = Guid.NewGuid().ToString();

            // Insert in data store
            _dataStore.Add(person.Id.ToString(), person);
            
            // Result message
            var resultMessage = new KafkaMessage<string, Person>
            {
                Key = "person",
                Value = person,
                MessageId = messageId,
                MessageType = "Person.Created"
            };

            await _kafkaProducer.ProduceAsync(resultMessage);

            return CreatedAtAction("GetPersonById", new { id = person.Id }, person);
        }
    }
}