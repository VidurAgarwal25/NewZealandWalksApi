using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAll(string? filterOn = null, string? filterQuery = null);
        Task<Walk?> GetById(Guid id);

        Task<Walk?> Update(Guid id, Walk walk);
        Task<Walk?> DeleteById(Guid id);
    }
}
