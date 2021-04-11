using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SimpleTask.Models;

namespace SimpleTask.Controllers
{
    [Route("v{version:apiVersion}/[controller]/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbCtx;
        public UserController(AppDbContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpPut]
        [Route("add")]
        public async Task<IActionResult> AddUser(
            [FromQuery][BindRequired] string firstName, 
            [FromQuery][BindRequired] string lastName)
        {
            var user = await _dbCtx.Users.AddAsync(new UserModel
            {
                FirstName = firstName,
                LastName = lastName
            });
            await _dbCtx.SaveChangesAsync();

            return Ok(user.Entity);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteUser([FromQuery][BindRequired] Guid userId)
        {
            var userDelete = await _dbCtx.Users.FindAsync(userId);
            if (userDelete == null)
            {
                return NotFound();
            }
            _dbCtx.Remove(userDelete);
            await _dbCtx.SaveChangesAsync();
            return Ok();
        }
        
        
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetUser([FromQuery][BindRequired] Guid userId)
        {
            var user = await _dbCtx.Users.FindAsync(userId).AsTask();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("listTasks")]
        public IActionResult GetUserTasks([FromQuery][BindRequired] Guid userId)
        {
            var userTasks = _dbCtx.Users.Include(u => u.Tasks)
                .SingleOrDefault(u => u.UserId == userId);

            if (userTasks == null)
            {
                return NotFound();
            }

            return Ok(userTasks.Tasks);
        }

        [HttpGet]
        [Route("list")]
        public List<UserModel> GetUsers()
        {
            return _dbCtx.Users.ToList();
        }
    }
}