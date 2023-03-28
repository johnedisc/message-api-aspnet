using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageBoardApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
// using Newtonsoft.Json;

namespace MessageBoardApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MessagesController : ControllerBase
  {
    private readonly MessageBoardApiContext _db;

    public MessagesController(MessageBoardApiContext db)
    {
      _db = db;
    }

    // GET api/Messages
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> Get( [FromQuery] string messageString, string name, int pageNumber = 1, int resultsPerPage = 10) // , int createdDate) // update params to handle query// & add'l q = name // [Range]) // : int createdDate
    {
      // add search parameters to our Get() controller action so that we can request and retrieve filtered data.
      // http://localhost:5000/api/messages?messageString=dinosaur
      IQueryable<Message> query = _db.Messages.AsQueryable();
      if ( messageString != null)
      {
        query = query.Where(entry => entry.MessageString == messageString);
      }
      // param: string name && q = name
      if ( name != null)
      {
        query = query.Where(entry => entry.Name == name); // http://localhost:5000/api/messages?messageString=dinosaur&name=matilda
      }
      // [Range=
      // ie. http://localhost:5000/api/messages?createdDate=5
      //   if (createdDate > 0)
      //   {
      //     query =  query.Where(entry => entry.Age >= createdDate);
      //   }

      var totalResultCount = query.Count();
      var items = query.Skip((pageNumber - 1) * resultsPerPage).Take(resultsPerPage).ToList();
      var totalPages = (int)Math.Ceiling((double)totalResultCount / resultsPerPage);
      // $ dotnet add package Newtonsoft.Json --version 13.0.1 (v.2021)
      var metadata = new
      {
          totalResultCount,
          resultsPerPage,
          currentPage = pageNumber,
          totalPages
      };
      // Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
      string headerText = JsonSerializer.Serialize(metadata);
      Response.Headers.Add("X-Pagination", headerText);
      Console.WriteLine("HeaderText: ", headerText);
      return items.ToList();


      // return await query.ToListAsync(); //  _db.Messages.ToListAsync();
    }

    // ie. GET: api/Messages/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Message>> GetMessage(int id)
    {
      Message message = await _db.Messages.FindAsync(id);

      if (message == null)
      {
        return NotFound();
      }

      return message;
    }

    // (new seed/input.cshtml) ie. POST api/messages
    [HttpPost]
    public async Task<ActionResult<Message>> Post(Message message)
    { 
      _db.Messages.Add(message);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetMessage), new { id = message.MessageId }, message);
    }    

    //  PUT action requires an entire object with all of its properties in order to make an update to it in the database. An alternative to thissupport partial updates: use PATCH instead:
    // (edit existing info): PUT requires a body with the entire updated message object (including the MessageId
    // PUT http://localhost:5000/api/messages/{id}
    // ie. PUT: api/Messages/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Message message)
    {
      if (id != message.MessageId)
      {
        return BadRequest();
      }
      _db.Messages.Update(message);

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!MessageExists(id))
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

    private bool MessageExists(int id)
    {
      return _db.Messages.Any(e => e.MessageId == id);
    }

    // DELETE requests to the following endpoint, where {id} is the variable for the MessageId of the message that we want to remove from out database:
    // DELETE http://localhost:5000/api/messages/{id}
    // ie. DELETE: api/Messages/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
      Message message = await _db.Messages.FindAsync(id);
      if (message == null)
      {
        return NotFound();
      }

      _db.Messages.Remove(message);
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}