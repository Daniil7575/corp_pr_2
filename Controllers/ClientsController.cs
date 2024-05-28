using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankApplication.Data;
using BankApplication.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BankApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly BankDbContext _context;

        public ClientsController(BankDbContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, CreateClientDTO clientDto)
        {
            if (id != clientDto.Id)
            {
                return BadRequest();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            client.Phone = clientDto.Phone;
            client.Address = clientDto.Address;
            client.ClientFullName = clientDto.ClientFullName;
            client.Sex = clientDto.Sex;
            client.BirthDate = clientDto.BirthDate;
            client.IsDebtor = clientDto.IsDebtor;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // Patch: api/Clients/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchClient(int id, JsonPatchDocument<CreateClientDTO> patchDocument)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var clientDto = new CreateClientDTO
            {
                Id = client.Id,
                Phone = client.Phone,
                Address = client.Address,
                ClientFullName = client.ClientFullName,
                Sex = client.Sex,
                BirthDate = client.BirthDate,
                IsDebtor = client.IsDebtor
            };

            patchDocument.ApplyTo(clientDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            client.Phone = clientDto.Phone;
            client.Address = clientDto.Address;
            client.ClientFullName = clientDto.ClientFullName;
            client.Sex = clientDto.Sex;
            client.BirthDate = clientDto.BirthDate;
            client.IsDebtor = clientDto.IsDebtor;
            try
            {
                await _context.SaveChangesAsync(); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<object>> PostClient(CreateClientDTO client)
        {
          if (_context.Clients == null)
          {
              return Problem("Entity set 'BankDbContext.Clients'  is null.");
          }
            var new_client = new Client { 
                Id = client.Id,
                Phone = client.Phone,
                Address = client.Address,
                ClientFullName= client.ClientFullName,
                Sex = client.Sex,
                BirthDate= client.BirthDate,
                IsDebtor = client.IsDebtor,
            };
            _context.Clients.Add(new_client);
            await _context.SaveChangesAsync();
            var responseData = new
            {
                status = true,
                data = new { id = client.Id }
            };
            return responseData;
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
