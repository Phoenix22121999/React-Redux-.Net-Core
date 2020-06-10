using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TodoTasksController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoTasksController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoTasks
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoTask>>> GetTodoItems()
        {
            return await _context.TodoTask.ToListAsync();
            
        }

        // GET: api/TodoTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTask>> GetTodoTask(int id)
        {
            var todoTask = await _context.TodoTask.FindAsync(id);

            if (todoTask == null)
            {
                return NotFound();
            }

            return todoTask;
        }

        // PUT: api/TodoTasks/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoTask(int id, TodoTask todoTask)
        {
            if (!ModelState.IsValid)
            {
                
                // var errorList = ModelState.Values.SelectMany(m => m.Errors)
                //                .Select(e => e.ErrorMessage)
                //                .ToList();
                //todoTask.Result = new BadRequestObjectResult(todoTask.ModelState);
                //ModelState.AddModelError(nameof(GetValuesQueryParameters.To), "The date range for the query can be maximum of 31 days.");
                    return new BadRequestObjectResult(ModelState);


            }   
            if (id != todoTask.Id)
            {
                return BadRequest(ModelState    );
            }

            _context.Entry(todoTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
          

            return Ok();
        }

        // POST: api/TodoTasks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TodoTask>> PostTodoTask(TodoTask todoTask)
        {
            
                _context.TodoTask.Add(todoTask);
                await _context.SaveChangesAsync();

                //return CreatedAtAction("GetTodoTask", new { id = todoTask.Id }, todoTask);
                return CreatedAtAction(nameof(GetTodoTask), new { id = todoTask.Id }, todoTask);
            

           
        }

        // DELETE: api/TodoTasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoTask>> DeleteTodoTask(int id)
        {
            var todoTask = await _context.TodoTask.FindAsync(id);
            if (todoTask == null)
            {
                return NotFound();
            }

            _context.TodoTask.Remove(todoTask);
            await _context.SaveChangesAsync();

            return todoTask;
        }

        private bool TodoTaskExists(long id)
        {
            return _context.TodoTask.Any(e => e.Id == id);
        }
    }
}
