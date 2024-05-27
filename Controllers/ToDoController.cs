using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Context;
using ToDoList.Entities;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ListContext _context;
        public ToDoController(ListContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult PostToDo(ToDo toDo)
        {
            _context.ToDos.Add(toDo);
            _context.SaveChanges();
            // return Ok(toDo);
            return CreatedAtAction(nameof(GetToDoId), new { id = toDo.Id }, toDo);
        }

        [HttpGet("/toDoList")]
        public IActionResult GetToDo()
        {
            var toDo = _context.ToDos.ToList();
            return Ok(toDo);
        }

        [HttpGet("{id}")]
        public IActionResult GetToDoId(int id)
        {
            var toDo = _context.ToDos.Find(id);
            if (toDo == null) return NotFound();
            return Ok(toDo);
        }

        [HttpGet("title/{title}")]
        public IActionResult GetToDoTitle(string title)
        {
            var toDo = _context.ToDos.Where(toDo => toDo.Titulo.Contains(title));
            return Ok(toDo);
        }

        [HttpGet("date/{date}")]
        public IActionResult GetToDoDate(DateTime date)
        {
            // Ex 2024-05-27T14:29:08.913
            var toDo = _context.ToDos.Where(toDo => toDo.Data == date).ToList();
            return Ok(toDo);
        }

        [HttpGet("status/{status}")]
        public IActionResult GetToDoStatus(EnumStatusToDo status)
        {
            var toDo = _context.ToDos.Where(toDo => toDo.Status == status);
            if (toDo == null) return NotFound();
            return Ok(toDo);
        }

        [HttpPut("/edit/{id}")]
        public IActionResult PutToDoId(int id, ToDo toDo)
        {
            var newToDo = _context.ToDos.Find(id);
            if (newToDo == null) return NotFound();

            newToDo.Titulo = toDo.Titulo;
            newToDo.Descricao = toDo.Descricao;
            newToDo.Data = toDo.Data;
            newToDo.Status = toDo.Status;

            _context.ToDos.Update(newToDo);
            _context.SaveChanges();
            return Ok(newToDo);
        }

        [HttpDelete("/delete/{id}")]
        public IActionResult DeleteToDoId(int id)
        {
            var toDo = _context.ToDos.Find(id);
            if (toDo == null) return NotFound();

            _context.ToDos.Remove(toDo);
            _context.SaveChanges();
            return Ok();
        }
    }
}