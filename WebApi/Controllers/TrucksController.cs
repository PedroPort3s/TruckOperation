using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Net;
using WebApi.Handlers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrucksController : ControllerBase
    {
        private readonly TruckDbContext _context;

        public TrucksController(TruckDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Truck>>> GetTrucks()
        {
            return await _context.Trucks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Truck>> GetTruck(Guid id)
        {
            var truck = await _context.Trucks.FindAsync(id);

            if (truck == null)
            {
                return NotFound();
            }

            return truck;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTruck(Guid id, Truck truck)
        {
            if (id != truck.Id || !TruckExists(id))
            {
                return BadRequest();
            }

            _context.Entry(truck).State = EntityState.Modified;

            await ValidateTruckUpsert(truck);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TruckExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(truck);
        }

        [HttpPost]
        public async Task<ActionResult<Truck>> PostTruck(Truck truck)
        {
            await ValidateTruckUpsert(truck);

            _context.Trucks.Add(truck);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTruck", new { id = truck.Id }, truck);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruck(Guid id)
        {
            var truck = await _context.Trucks.FindAsync(id);
            if (truck == null)
            {
                return NotFound();
            }

            _context.Trucks.Remove(truck);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TruckExists(Guid id)
        {
            return _context.Trucks.Any(e => e.Id == id);
        }

        private async Task ValidateTruckUpsert(Truck truck)
        {
            if (truck.ModelId == 0)
            {
                throw new BadHttpRequestException("A model should be informed.", (int)HttpStatusCode.BadRequest);
            }

            if (truck.ModelId == 0)
            {
                throw new BadHttpRequestException("A plant should be informed.", (int)HttpStatusCode.BadRequest);
            }

            if (truck.YearOfManufacture <= 1928 || truck.YearOfManufacture > 2999)
            {
                throw new BadHttpRequestException($"{nameof(truck.YearOfManufacture)} must be between 1928 and 2999", (int)HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(truck.ChassisCode))
            {
                throw new BadHttpRequestException($"{nameof(truck.ChassisCode)} must be informed", (int)HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(truck.Color))
            {
                throw new BadHttpRequestException($"{nameof(truck.Color)} must be informed", (int)HttpStatusCode.BadRequest);
            }

            var model = await _context.Models.FindAsync(truck.PlantId);

            if (model == null)
            {
                throw new BadHttpRequestException("An existing model should be informed.", (int)HttpStatusCode.BadRequest);
            }

            var plant = await _context.Plants.FindAsync(truck.PlantId);

            if (plant == null)
            {
                throw new BadHttpRequestException("An existing plant should be informed.", (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
