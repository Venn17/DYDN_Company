﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DYDN_Company.Models;
using System.Net.Mail;
using System.Net;

namespace DYDN_Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountUsersController : ControllerBase
    {
        private static Random random = new Random();
        private readonly AppDBContext _context;

        public AccountUsersController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/AccountUsers
        [HttpGet]
        public IEnumerable<AccountUser> GetAccountUsers()
        {
            return _context.AccountUsers.Where(b=>b.Status == true);
        }
        [HttpGet]
        [Route("getAll")]
        public IEnumerable<AccountUser> GetAllAccountUsers()
        {
            return _context.AccountUsers.ToList();
        }
        [HttpGet]
        [Route("TrashAccountUsers")]    
        public IEnumerable<AccountUser> GetTrashAccountUsers()
        {
            return _context.AccountUsers.Where(b => b.Status == false);
        }
        // GET: api/AccountUsers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountUser([FromRoute] int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountUser = await _context.AccountUsers.FindAsync(id);

            if (accountUser == null)
            {
                return NotFound();
            }

            return Ok(accountUser);
        }
        [HttpPost]
        [Route("RepeatAccountUsers")]
        public async Task<IActionResult> RepeatAccountUsers([FromBody] AccountUser accountUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(accountUser).State = EntityState.Modified;

            try
            {
                accountUser.Status = true;
                accountUser.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return NoContent();
        }
        [HttpPost]
        [Route("TemporaryDelete")]
        public async Task<IActionResult> TemporaryDelete([FromBody] AccountUser accountUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(accountUser).State = EntityState.Modified;

            try
            {
                accountUser.Status = false;
                //categoryProduct.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return NoContent();
        }
        // PUT: api/AccountUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountUser([FromRoute] int? id, [FromBody] AccountUser accountUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accountUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(accountUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountUserExists(id))
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

        // POST: api/AccountUsers
        [HttpPost]
        public async Task<IActionResult> PostAccountUser([FromBody] AccountUser accountUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AccountUsers.Add(accountUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountUser", new { id = accountUser.Id }, accountUser);
        }

        // DELETE: api/AccountUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountUser([FromRoute] int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountUser = await _context.AccountUsers.FindAsync(id);
            if (accountUser == null)
            {
                return NotFound();
            }

            _context.AccountUsers.Remove(accountUser);
            await _context.SaveChangesAsync();

            return Ok(accountUser);
        }

        private bool AccountUserExists(int? id)
        {
            return _context.AccountUsers.Any(e => e.Id == id);
        }

        // POST: api/AccountUsers/Login
        [HttpPost]
        [Route("Login")]
        public AccountUser Login([FromBody] AccountUserLogin login)
        {
            AccountUser result;
            if (login.emailorphone.Contains('@') == true) {
                result =  _context.AccountUsers.Where(acc => acc.Email == login.emailorphone & acc.Password == login.password).FirstOrDefault();
            }
            else
            {
                result =  _context.AccountUsers.Where(acc => acc.Phone == login.emailorphone & acc.Password == login.password).FirstOrDefault();
            }
            
            if(result != null)
            {
                return result;
            }

            return null;
        }

        // POST: api/AccountUsers/ChangePassword/1
        [HttpPost("id")]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromRoute] int id,[FromBody] string password)
        {
            AccountUser result = _context.AccountUsers.Find(id);
            if(result != null)
            {
                result.Password = password;
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        [HttpPost]
        [Route("SendEmail")]
        public ActionResult SendEmail([FromBody] string email)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string new_pass = new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            try
            {
                if (ModelState.IsValid)
                {
                    var account = _context.AccountUsers.Where(p => p.Email == email).FirstOrDefault();
                    if (account == null)
                    {
                        return NotFound("Email not exist");
                    }
                    else
                    {
                        var senderEmail = new MailAddress("vienavtb@gmail.com", "CongVien");
                        var receiverEmail = new MailAddress(email, "Receiver");
                        var password = "rwpezipvxnciwrij";
                        var subject = "Here's the link to reset your password";
                        var body = "<p>Hello,</p>" + "<p>You have requested to reset your password.</p>"
                        + "<p> below to change your password:</p>"
                        + "<h4>Your new password is : <b>" + new_pass + "</b></h4>"
                        + "<h3><p>Please don't share this email for everyone !</p></h3>"
                        + "<br><p>This link will expire within the next hour . "
                        + "<b>(If this is a spam message, please click  it is not spam)<b>";
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true
                        })
                        {
                            smtp.Send(mess);
                        }
                        account.Password = new_pass;
                        _context.Update(account);
                        _context.SaveChangesAsync();
                    }
                    return NoContent();
                }
            }
            catch (Exception)
            {

            }
            return NoContent();
        }
    }
}