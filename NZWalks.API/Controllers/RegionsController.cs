using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    // https://localhost:port/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
                this.dbContext = dbContext;
        }

        // Get all regions
        // https://localhost:port/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            /*var regions = new List<Region>
            {
                new Region
                {
                    Id=Guid.NewGuid(),
                    Name = "Auckland Region",
                    Code="AKL",
                    RegionImageUrl="drdfg"
                },
                new Region
                {
                    Id=Guid.NewGuid(),
                    Name = "Wellington Region",
                    Code="WLG",
                    RegionImageUrl="dsgfhfg"
                }
            };*/

            // Get data from database - Domain models
            var regionsDomain = dbContext.Regions.ToList();

            // Map Domain Models to DTOs
            var regionDto = new List<RegionDto>();
            foreach (var region in regionsDomain) {
                regionDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }

            return Ok(regionDto);
        }

        // Get region by id
        // https://localhost:port/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById(Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            // Get Region Domain Model from database
            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            // Map Domain Models to DTOs
            var regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionDto);
        }

        // POST to create New Region
        // POST: https://localhost:port/api/regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or cpnvert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // Use Domain Model to create Region
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            // Map Domain model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // Update region
        // PUT: https://localhost:port/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) 
        {
            // Check if region exists
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Map DTO to Domain Model
            regionDomainModel.Code=updateRegionRequestDto.Code;
            regionDomainModel.Name=updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl=updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

            // Convert Domain Model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        // Delete Region
        // DELETE: https://localhost:port/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x=> x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete region
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

            // return deleted Region back
            // Map Domain Model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
