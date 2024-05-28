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
    public class AccountsController : ControllerBase
    {
        private readonly BankDbContext _context;

        public AccountsController(BankDbContext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
        {
          if (_context.Accounts == null)
          {
              return NotFound();
          }
            var accounts = await _context.Accounts
                .Include(a => a.Client) 
            .Select(a => new AccountDto
            {
                Id = a.Id,
                Number = a.Number,
                OpenDate = a.OpenDate,
                OwnerId = a.OwnerId,
                Balance = a.Balance,
                ClientFullName = a.Client.ClientFullName 
            })
            .ToListAsync();
            return accounts;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(int id)
        {
          if (_context.Accounts == null)
          {
              return NotFound();
          }
            var account = await _context.Accounts
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }
            var accountDto = new AccountDto
            {
                Id = account.Id,
                Number = account.Number,
                OpenDate = account.OpenDate,
                OwnerId = account.OwnerId,
                Balance = account.Balance,
                ClientFullName = account.Client.ClientFullName
            };
            return accountDto;
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, CreateAccountDto accountDto)
        {
            if (id != accountDto.Id)
            {
                return BadRequest();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            // Обновляем данные счета
            account.Number = accountDto.Number;
            account.OpenDate = accountDto.OpenDate;
            account.OwnerId = accountDto.OwnerId;
            account.Balance = accountDto.Balance;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAccount(int id, JsonPatchDocument<AccountDto> patchDocument)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            var accountDto = new AccountDto
            {
                Id = account.Id,
                Number = account.Number,
                OpenDate = account.OpenDate,
                OwnerId = account.OwnerId,
                Balance = account.Balance
            };

            patchDocument.ApplyTo(accountDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            account.Number = accountDto.Number;
            account.OpenDate = accountDto.OpenDate;
            account.OwnerId = accountDto.OwnerId;
            account.Balance = accountDto.Balance;

            try
            {
                await _context.SaveChangesAsync(); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound(); // Если счет был удален, возвращаем 404 Not Found
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Возвращаем 204 No Content
        }

        [HttpPost]
        public async Task<ActionResult<object>> PostAccount(CreateAccountDto createAccountDto)
        {
          if (_context.Accounts == null)
          {
              return Problem("Entity set 'BankDbContext.Accounts'  is null.");
          }
           var client = await _context.Clients.FindAsync(createAccountDto.OwnerId);
           if (client == null)
            {
                return NotFound("Client not found.");
            }
            var account = new Account
            {
                Id = createAccountDto.Id,
                Number = createAccountDto.Number,
                OpenDate = createAccountDto.OpenDate,
                OwnerId = createAccountDto.OwnerId,
                Balance = createAccountDto.Balance,
                Client = client
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            var responseData = new
            {
                status = true,
                data = new { id = account.Id }
            };
            return responseData;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if (_context.Accounts == null)
            {
                return NotFound();
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
