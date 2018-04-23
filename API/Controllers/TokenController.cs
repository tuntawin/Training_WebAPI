using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        // GET: api/Token
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Token/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Token
        [HttpPost]
        public IActionResult Post([FromBody]LoginInputModel inputModel)
        {
            if (inputModel.Username != "admin" && inputModel.Password != "1234")
                return Unauthorized();

            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("aaaaaaaaaaaaaaaa"))
                                .AddSubject("james bond")
                                .AddIssuer("Fiver.Security.Bearer")
                                .AddAudience("Fiver.Security.Bearer")
                                .AddClaim("MembershipId", "111")
                                .AddExpiry(1)
                                .Build();

            return Ok(token.Value);
        }
        
        // PUT: api/Token/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
