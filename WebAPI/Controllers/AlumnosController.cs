using WebAPI.Data; //llamamos a la carpeta Data para instanciar la clase apiDbContext
using WebAPI.Models; //llamamos a la carpeta Models para instanciar la clase Alumnos
using Microsoft.AspNetCore.Mvc; //llamamos a la libreria de ASP.NET Core para crear el controlador y sus funcionalidades
using Microsoft.EntityFrameworkCore; //llamamos a la libreria de Entity Framework Core para trabajar con la base de datos

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")] //ruta de la API
    [ApiController] //controlador de la API
    public class AlumnosController : ControllerBase
    {
        private readonly ApiDbContext _context; //instanciamos la clase ApiDbContext para trabajar con la base de datos

        public AlumnosController(ApiDbContext context)
        {
            _context = context; //inicializamos la clase ApiDbContext
        }

        // GET: api/<AlumnosController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> GetAlumnos()
        {
            return await _context.Alumnos.ToListAsync();
        }

        // GET api/<AlumnosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alumno>> GetAlumno(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();
            return alumno;
        }

        // POST api/<AlumnosController>
        [HttpPost]
        public async Task<ActionResult<Alumno>> Post([FromBody] Alumno alumno)
        {
            _context.Alumnos.Add(alumno); //Prepara los datos para almacenarlos a la tabla
            await _context.SaveChangesAsync(); //lo almacena ya en la BD

            return Ok(alumno); //regresa on 200 y el valor de los datos
            
        }

        // PUT api/<AlumnosController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Alumno alumno)
        {
            if (id != alumno.Id) //deben de coincidir los Id tanto del body como de la URL
                return BadRequest("El ID del alumno no coincide.");

            _context.Entry(alumno).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Alumnos.Any(a => a.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return Ok();//no regresa un objeto
        }

        // DELETE api/<AlumnosController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id); //busca el Id del registro
            if (alumno == null)
                return NotFound();

            _context.Alumnos.Remove(alumno); //prepara el borrado en BD
            await _context.SaveChangesAsync(); //confirma la transaccion

            return NoContent();
        }
    }
}
