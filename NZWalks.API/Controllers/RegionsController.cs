using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        // dto's are used to present only a limited data infront of client rather than displaying whole data
        private readonly NZWalksDBContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(NZWalksDBContext dBContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dBContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        // Get All Regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // get data from database - domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            // map domain models to dtos
            /*var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageURL = regionDomain.RegionImageURL
                });
            }*/

            //from domain model to dto
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            return Ok(regionsDto);
        }

        //  Get Regions By ID

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var regionDomain = await regionRepository.GetById(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

           /* var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageURL = regionDomain.RegionImageURL
            };*/

            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return Ok(regionDto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert dto to domain model
            /*var regionDomain = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageURL = addRegionRequestDto.RegionImageURL
            };*/

            var regionDomain = mapper.Map<Region>(addRegionRequestDto);

            regionDomain = await regionRepository.Create(regionDomain);

            //map domain model back to dto

            /*var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageURL = regionDomain.RegionImageURL
            };*/
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            // return 201 response
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromBody] UpdateRegionRequestDto updateRegionDto, [FromRoute] Guid id)
        {
            /*var regionDomain = new Region()
            {
                Code = updateRegionDto.Code,
                Name = updateRegionDto.Name,
                RegionImageURL = updateRegionDto.RegionImageURL
            };*/

            var regionDomain = mapper.Map<Region>(updateRegionDto);

           regionDomain = await regionRepository.Update(id, regionDomain);

            if (regionDomain == null)
                return NotFound();

            /*var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageURL = regionDomain.RegionImageURL
            };*/
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.Delete(id);
            if(regionDomain == null)
                return NotFound();

            /* var regionDto = new RegionDto
             {
                 Id = regionDomain.Id,
                 Code = regionDomain.Code,
                 Name = regionDomain.Name,
                 RegionImageURL = regionDomain.RegionImageURL
             };*/
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }
    }
}
