using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TO_DO_API.Models;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Is always [Route("api/Name of the Controller")]
    /// </summary>
    [Route("api/TodoItemDTO")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }
        /// <summary>
        /// First implementation of the GET_Methods --> return value is ActionResult<T> - Type
        /// </summary>
        /// <returns></returns>
        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }
        /// <summary>
        /// Second implementation of the Get-Method --> return value is ActionResult<T> - Type
        /// If there is no existing element with the requested Id then the Method returns NotFound-Code 404 
        /// otherwise the Method returns the Code 200
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }
        /// <summary>
        /// If successful the Method returns the Code 204: NoContent
        /// otherwise it can return Code 400: Badrequest
        /// or the Method returns the Code 404: NotFound
        /// If there is an Error make sure to call the GET-Mthod before the PUT-Method to make sure the the Database has an input
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todoItemDTO"></param>
        /// <returns></returns>
        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        /// <summary>
        /// If successfulthe Method return value is status Code 201: CreatedAtAction
        /// </summary>
        /// <param name="todoItemDTO"></param>
        /// <returns></returns>
        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));
        }
        /// <summary>
        /// If successful the return value of this Method is Code 204: NoContent
        /// else the Method returns the ErrorCode 404: NotFound
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}