
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaTurnosCentroUnas.Data;
using SistemaTurnosCentroUnas.DTOs;
using SistemaTurnosCentroUnas.Models;

namespace SistemaTurnosCentroUnas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TurnosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TurnoDTO>>> GetTurnos()
        {
            var turnos = await _context.Turnos
                .Select(t => new TurnoDTO
                {
                    Id = t.Id,
                    FechaHora = t.FechaHora,
                    Servicio = t.Servicio,
                    Precio = t.Precio,
                    ClienteId = t.ClienteId
                })
                .ToListAsync();

            return Ok(turnos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TurnoDTO>> GetTurno(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);

            if (turno == null)
                return NotFound();

            return Ok(new TurnoDTO
            {
                Id = turno.Id,
                FechaHora = turno.FechaHora,
                Servicio = turno.Servicio,
                Precio = turno.Precio,
                ClienteId = turno.ClienteId
            });
        }

        [HttpPost]
        public async Task<ActionResult<TurnoDTO>> CrearTurno(TurnoDTO dto)
        {
            var clienteExiste = await _context.Clientes
                .AnyAsync(c => c.Id == dto.ClienteId);

            if (!clienteExiste)
                return BadRequest("El cliente indicado no existe.");

            var turno = new Turno
            {
                FechaHora = dto.FechaHora,
                Servicio = dto.Servicio,
                Precio = dto.Precio,
                ClienteId = dto.ClienteId
            };

            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            dto.Id = turno.Id;

            return CreatedAtAction(
                nameof(GetTurno),
                new { id = turno.Id },
                dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTurno(int id, TurnoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID de la URL no coincide con el ID del turno.");

            var turno = await _context.Turnos.FindAsync(id);

            if (turno == null)
                return NotFound();

            var clienteExiste = await _context.Clientes
                .AnyAsync(c => c.Id == dto.ClienteId);

            if (!clienteExiste)
                return BadRequest("El cliente indicado no existe.");

            turno.FechaHora = dto.FechaHora;
            turno.Servicio = dto.Servicio;
            turno.Precio = dto.Precio;
            turno.ClienteId = dto.ClienteId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTurno(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);

            if (turno == null)
                return NotFound();

            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}