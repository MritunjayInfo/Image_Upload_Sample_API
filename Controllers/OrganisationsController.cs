using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApp.Models;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisationsController : ControllerBase
    {
        private readonly Context _context;

        public OrganisationsController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organisation>>> GetOrganisations()
        {
            var organisations = new List<Organisation>();
            organisations = await _context.Organisation.ToListAsync();
            foreach(var organisation in organisations)
            {
                if(organisation.OrganisationByteImage != null)
                {
                    string imageBase64Data = Convert.ToBase64String(organisation.OrganisationByteImage);
                    organisation.OrganisationImageBase64 = string.Format("data:image/png;base64,{0}", imageBase64Data);
                }
            }
            return organisations;
        }

        [HttpGet("{organisationId}")]
        public async Task<ActionResult<Organisation>> GetOrganisation(int organisationId)
        {
            var organisation = await _context.Organisation.FindAsync(organisationId);
            
            if (organisation == null)
            {
                return NotFound();
            }
            else
            {
                if (organisation.OrganisationByteImage != null)
                {
                    string imageBase64Data = Convert.ToBase64String(organisation.OrganisationByteImage);
                    organisation.OrganisationImageBase64 = string.Format("data:image/png;base64,{0}", imageBase64Data);
                }
            }
            return organisation;
        }

        [HttpPost]
        public async Task<ActionResult<Organisation>> CreateOrganisation([FromForm] Organisation organisation)
        {
            var org = organisation;
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                org.OrganisationByteImage = ms.ToArray();
            }
            _context.Organisation.Add(org);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!OrganisationExists(organisation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrganisation", organisation);
        }

        [HttpPut("{organisationId}")]
        public async Task<IActionResult> UpdateOrganisation(int organisationId, [FromForm] Organisation organisation)
        {
            if(organisationId != organisation.Id)
            {
                return BadRequest();
            }

            var org = organisation;
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                org.OrganisationByteImage = ms.ToArray();
            }

            _context.Entry(org).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganisationExists(organisationId))
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

        [HttpDelete("{organisationId}")]
        public async Task<IActionResult> DeleteOrganisation([FromForm] int organisationId)
        {
            var organisation = await _context.Organisation.FindAsync(organisationId);
            if(organisation == null)
            {
                return NotFound();
            }

            _context.Organisation.Remove(organisation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrganisationExists(int organisationId)
        {
            return _context.Organisation.Any(o => o.Id == organisationId);
        }
    }
}
