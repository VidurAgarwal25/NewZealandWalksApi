using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        // dto's are used to present only a limited data infront of client rather than displaying whole data
        private readonly NZWalksDBContext dbContext;
        public RegionsController(NZWalksDBContext dBContext)
        {
            this.dbContext = dBContext;
        }
        // Get All Regions
        [HttpGet]
        public IActionResult GetAll()
        {
            // get data from database - domain models
            var regionsDomain = dbContext.Regions.ToList();

            // map domain models to dtos
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageURL = regionDomain.RegionImageURL
                });
            }
            return Ok(regionsDto);
        }

        //  Get Regions By ID

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageURL = regionDomain.RegionImageURL
            };

            return Ok(regionDto);
        }


        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert dto to domain model
            var regionDomain = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageURL = addRegionRequestDto.RegionImageURL
            };

            // use domain model to create region
            dbContext.Regions.Add(regionDomain);
            dbContext.SaveChanges();

            //map domain model back to dto

            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageURL = regionDomain.RegionImageURL
            };
            // return 201 response
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromBody] UpdateRegionRequestDto updateRegionDto, [FromRoute] Guid id)
        {
            
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
                return NotFound();

            regionDomain.Code = updateRegionDto.Code;
            regionDomain.Name = updateRegionDto.Name;
            regionDomain.RegionImageURL = updateRegionDto.RegionImageURL;

            dbContext.SaveChanges();

            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageURL = regionDomain.RegionImageURL
            };
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDomain == null)
                return NotFound();

            dbContext.Regions.Remove(regionDomain);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
