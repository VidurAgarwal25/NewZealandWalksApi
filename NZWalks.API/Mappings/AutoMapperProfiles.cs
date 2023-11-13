using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using System.Threading.Tasks;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            // region to regiondto
            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
        }
    }
}