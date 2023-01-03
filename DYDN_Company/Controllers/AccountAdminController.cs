using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DYDN_Company.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginModel = DYDN_Company.Models.LoginModel;

namespace DYDN_Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AddCors")]
    public class AccountAdminController : ControllerBase
    {
        private readonly AppDBContext _context;

        public AccountAdminController(AppDBContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            var account = _context.AccountAdmins.FirstOrDefault(x => x.Email == user.Email);
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }
            user.AccountName = account.Code;
            user.Role = account.Role;
            if (user.Email == account.Email && user.Password == account.Password)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new AuthenticatedResponse { Token = tokenString, User = user });
            }
            return Unauthorized();
        }
        // GET: api/AccountAdmin
        [HttpGet]
        public IEnumerable<AccountAdmin> GetAccountAdmins()
        {
            return _context.AccountAdmins;
        }

        // GET: api/AccountAdmin/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountAdmin([FromRoute] int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountAdmin = await _context.AccountAdmins.FindAsync(id);

            if (accountAdmin == null)
            {
                return NotFound();
            }

            return Ok(accountAdmin);
        }

        // PUT: api/AccountAdmin/5
        [HttpPost]
        [Route("PutAccountAdmin")]
        public async Task<IActionResult> PutAccountAdmin( [FromBody] AccountAdmin accountAdmin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           

            _context.Entry(accountAdmin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return NoContent();
        }

        // POST: api/AccountAdmin
        [HttpPost]
        public async Task<IActionResult> PostAccountAdmin([FromBody] AccountAdmin accountAdmin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AccountAdmins.Add(accountAdmin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountAdmin", new { id = accountAdmin.Id }, accountAdmin);
        }

        // DELETE: api/AccountAdmin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountAdmin([FromRoute] int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountAdmin = await _context.AccountAdmins.FindAsync(id);
            if (accountAdmin == null)
            {
                return NotFound();
            }

            _context.AccountAdmins.Remove(accountAdmin);
            await _context.SaveChangesAsync();

            return Ok(accountAdmin);
        }

        private bool AccountAdminExists(int? id)
        {
            return _context.AccountAdmins.Any(e => e.Id == id);
        }
    }
}