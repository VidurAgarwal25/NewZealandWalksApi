﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQlWalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext dBContext;

        public SQlWalkRepository(NZWalksDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dBContext.Walks.AddAsync(walk);
            await dBContext.SaveChangesAsync();
            return walk;
        }


        public async Task<List<Walk>> GetAll(string? filterOn = null, string? filterQuery = null)
        {
            /*return await dBContext.Walks.Include("Difficulty").Include("Region").ToListAsync();*/
            var walks = dBContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            // Filtering
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false ) {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            return await walks.ToListAsync();
        }
          
        public async Task<Walk?> GetById(Guid id)
        {
            return await dBContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> Update(Guid id, Walk walk)
        {
            var existingWalk = await dBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingWalk == null)
            {
                return null;
            }
            existingWalk.Name= walk.Name;
            existingWalk.Description= walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageURL= walk.WalkImageURL;
            existingWalk.DifficultyId= walk.DifficultyId;
            existingWalk.RegionId= walk.RegionId;
            await dBContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk?> DeleteById(Guid id)
        {
            var walkDomainModel = await dBContext.Walks.FirstOrDefaultAsync(x=>x.Id == id);
            if(walkDomainModel == null)
            {
                return null;
            }
            dBContext.Walks.Remove(walkDomainModel);
            await dBContext.SaveChangesAsync();
            return walkDomainModel;
        }
    }
}
