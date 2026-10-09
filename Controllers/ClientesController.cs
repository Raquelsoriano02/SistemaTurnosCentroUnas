
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaTurnosCentroUnas.Data;
using SistemaTurnosCentroUnas.DTOs;
using SistemaTurnosCentroUnas.Models;

namespace SistemaTurnosCentroUnas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Select(c => new ClienteDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Telefono = c.Telefono
                })
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            return Ok(new ClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Telefono = cliente.Telefono
            });
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> CrearCliente(ClienteDTO dto)
        {
            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            dto.Id = cliente.Id;

            return CreatedAtAction(
                nameof(GetCliente),
                new { id = cliente.Id },
                dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, ClienteDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cliente.");

            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            cliente.Nombre = dto.Nombre;
            cliente.Telefono = dto.Telefono;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Turnos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                return NotFound();

            if (cliente.Turnos.Any())
                return Conflict("No se puede eliminar el cliente porque tiene turnos registrados.");

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}