﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionsFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        public readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository) {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        //Create walk
        // /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
                // map dto to domain model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                await walkRepository.CreateAsync(walkDomainModel);
                //map domain model to dto

                return Ok(mapper.Map<WalkDto>(walkDomainModel));
           
        }
        //filtering, sorting and pagination happens only in get method
        //get walks
        //GET: /api/walks?filterOn=Name&filterQuery=BeachWalk&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        //user wants to filter on the basis of name columns that have track in it
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string ? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber =1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walkRepository.GetAll(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        //Get walk by id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetById(id);
            if(walkDomainModel == null)
                return NotFound();
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id,  UpdateWalkRequestDto updateWalkRequestDto)
        {

            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

           walkDomainModel = await walkRepository.Update(id, walkDomainModel);
            if(walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //delete walk by id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteWalkDomainModel = await walkRepository.DeleteById(id);
            if(deleteWalkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(deleteWalkDomainModel));
        }
    }
}
