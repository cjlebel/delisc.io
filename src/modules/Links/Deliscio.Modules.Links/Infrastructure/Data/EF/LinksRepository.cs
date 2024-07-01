using System.Linq.Expressions;
using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.Links.Domain.LinkTags;
using Deliscio.Modules.Links.Infrastructure.Data.Entities;
using FluentResults;
using MongoDB.Bson;

namespace Deliscio.Modules.Links.Infrastructure.Data.EF;

public sealed class LinksRepository : ILinksRepository
{
    public Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> FindAsync(Expression<Func<LinkEntity, bool>> predicate, int pageNo = 1, int pageSize = 25, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public LinkEntity? Get(ObjectId id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<LinkEntity> Get(IEnumerable<ObjectId> ids)
    {
        throw new NotImplementedException();
    }

    public Task<LinkEntity?> GetAsync(ObjectId id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LinkEntity>> GetAsync(IEnumerable<ObjectId> ids, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> GetAsync(int pageNo, int pageSize, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<LinkEntity?> FirstOrDefaultAsync(Expression<Func<LinkEntity, bool>> predicate, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Add(LinkEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(LinkEntity entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void AddRange(IEnumerable<LinkEntity> entities, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IEnumerable<LinkEntity> entities, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(ObjectId id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(LinkEntity entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(ObjectId id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(LinkEntity entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void RemoveRange(IEnumerable<ObjectId> ids, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRangeAsync(IEnumerable<ObjectId> ids, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Update(LinkEntity entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> UpdateAsync(LinkEntity entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<(IReadOnlyList<Domain.Links.Link> Results, int TotalPages, int TotalCount)>> FindLinksAsync(
        string term, string[] tags, string domain, 
        int pageNo, int pageSize, int offset = 0, 
        bool? isActive = default, bool? isFlagged = default, bool? isDeleted = false,
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Link>> GetLinkByIdAsync(string linkId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Link>> GetLinkByUrlAsync(string url, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Link>>> GetLinksByUserAsync(string userId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateLinkAsync(Link link, string updatedByUserId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<LinkTag>>> GetTagsAsync(string linkId, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LinkTagEntity>> GetRelatedTagsAsync(string[] tags, int count, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LinkTagEntity>> GetRelatedTagsByDomainAsync(string domain, int count, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}