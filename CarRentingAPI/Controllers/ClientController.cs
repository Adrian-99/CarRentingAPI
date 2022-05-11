using CarRentingAPI.Data.Entities;
using CarRentingAPI.Dtos;
using CarRentingAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingAPI.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private ClientRepository clientRepository;

        public ClientController(ClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ClientDto>>> Get()
        {
            var clients = (await clientRepository.GetAll())
                .Select(client => new ClientDto(client.FirstName, client.LastName, client.PhoneNumber));
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> Get(Guid id)
        {
            var client = await clientRepository.GetById(id);
            if (client == null)
            {
                return BadRequest($"Client with id {id} doesn't exist");
            }
            return Ok(new ClientDto(client.FirstName, client.LastName, client.PhoneNumber));
        }

        [HttpPost]
        public async Task<ActionResult<ClientDto>> Post([FromBody] ClientDto clientDto)
        {
            var validationError = ValidatePhoneNumber(clientDto.PhoneNumber);
            if (validationError != null)
            {
                return BadRequest(validationError);
            }
            var client = new Client()
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                PhoneNumber = clientDto.PhoneNumber 
            };
            var createdClient = await clientRepository.Create(client);
            return Created(
                $"/api/client/{createdClient.Id}",
                new ClientDto(createdClient.FirstName, createdClient.LastName, createdClient.PhoneNumber)
                );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClientDto>> Put(Guid id, [FromBody] ClientDto clientDto)
        {
            var client = await clientRepository.GetById(id);
            if (client != null)
            {
                var validationError = ValidatePhoneNumber(clientDto.PhoneNumber);
                if (validationError != null)
                {
                    return BadRequest(validationError);
                }
                client.FirstName = clientDto.FirstName;
                client.LastName = clientDto.LastName;
                client.PhoneNumber = clientDto.PhoneNumber;
                var updatedClient = await clientRepository.Update(client);
                return Ok(new ClientDto(updatedClient.FirstName, updatedClient.LastName, updatedClient.PhoneNumber));
            }
            else
            {
                return BadRequest($"Client with id {id} doesn't exist");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var client = await clientRepository.GetById(id);
            if (client != null)
            {
                await clientRepository.Delete(id);
                return NoContent();
            }
            else
            {
                return BadRequest($"Client with id {id} doesn't exist");
            }
        }

        private string? ValidatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Any(c => !char.IsDigit(c)))
            {
                return "Phone number must contain only digits";
            }
            if (phoneNumber.Length != 9)
            {
                return "Phone number must be 9 digits long";
            }
            return null;
        }
    }
}
