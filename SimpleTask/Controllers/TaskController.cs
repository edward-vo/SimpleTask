using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SimpleTask.Models;

namespace SimpleTask.Controllers
{
    [Route("v{version:apiVersion}/[controller]/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _dbCtx;
        public TaskController(AppDbContext dbCtx)
        {
            _dbCtx = dbCtx;
        }
        
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddTask(
            [FromQuery][BindRequired] string taskName, 
            [FromQuery] Guid assignedUser)
        {
            var task = await _dbCtx.Tasks.AddAsync(new TaskModel()
            {
                TaskName = taskName,
                AssignedUserId = assignedUser
            });
            await _dbCtx.SaveChangesAsync();

            return Ok(task.Entity);
        }

        [HttpPost]
        [Route("assign")]
        public async Task<IActionResult> AssignUserToTask(
            [FromQuery][BindRequired] Guid taskId,
            [FromQuery][BindRequired] Guid userId)
        {
            var taskUpdate = await _dbCtx.Tasks.FindAsync(taskId);
            if (taskUpdate == null)
            {
                return NotFound();
            }

            taskUpdate.AssignedUserId = userId;
            _dbCtx.Tasks.Update(taskUpdate);
            await _dbCtx.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteTask([FromQuery][BindRequired] Guid taskId)
        {
            var taskDelete = await _dbCtx.Tasks.FindAsync(taskId);
            if (taskDelete == null)
            {
                return NotFound();
            }
            _dbCtx.Remove(taskDelete);
            await _dbCtx.SaveChangesAsync();
            return Ok();
        }
        
        
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetTask([FromQuery][BindRequired] Guid taskId)
        {
            var user = await _dbCtx.Tasks.FindAsync(taskId).AsTask();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("list")]
        public List<TaskModel> GetTasks()
        {
            return _dbCtx.Tasks.ToList();
        }
    }
}