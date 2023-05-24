using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using flowPlusExternal.Extensions;
using flowPlusExternal.Models;
//using Data;

namespace flowPlusExternal.EndPoints
{
    [ApiController]
    [Route("api/users")]
    public class UsersEndpoint : ControllerBase
    {
        private readonly TPCoreProductionContext _context;
        private readonly UserManager<Data.ExtranetUsersTemp> _manager;
        private readonly SmartSettings _settings;

        public UsersEndpoint(TPCoreProductionContext context, UserManager<Data.ExtranetUsersTemp> manager, SmartSettings settings)
        {
            _context = context;
            _manager = manager;
            _settings = settings;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Data.ExtranetUsersTemp>>> Get()
        {
            var users = await _manager.Users.AsNoTracking().ToListAsync();

            return Ok(new { data = users, recordsTotal = users.Count, recordsFiltered = users.Count });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Data.ExtranetUsersTemp>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] Data.ExtranetUsersTemp model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.UserName = model.Email;

            var result = await _manager.CreateAsync(model);

            if (result.Succeeded)
            {
                // HACK: This password is just for demonstration purposes!
                // Please do NOT keep it as-is for your own project!
                result = await _manager.AddPasswordAsync(model, "Password123!");

                if (result.Succeeded)
                {
                    return CreatedAtAction("Get", new { id = model.Id }, model);
                }
            }

            return BadRequest(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromForm] Data.ExtranetUsersTemp model)
        {
            var result = await _context.UpdateAsync(model, model.Id);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromForm] Data.ExtranetUsersTemp model)
        {
            // HACK: The code below is just for demonstration purposes!
            // Please use a different method of preventing the currently logged in user from being removed
            if (model.UserName == _settings.Theme.Email)
            {
                return BadRequest(SmartError.Failed("Please do not delete the main user! =)"));
            }

            var result = await _context.DeleteAsync<Data.ExtranetUsersTemp>(model.Id);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result);
        }
    }
}
