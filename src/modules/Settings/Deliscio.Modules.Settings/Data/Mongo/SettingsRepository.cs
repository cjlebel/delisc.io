using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.Settings.Data.Entities;
using Deliscio.Modules.Settings.Interfaces;
using Deliscio.Modules.Settings.Mappers;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace Deliscio.Modules.Settings.Data.Mongo;

public class SettingsRepository : MongoRepository<SettingsEntity>, ISettingsRepository
{
    #region - Constructors -

    public SettingsRepository(IOptions<MongoDbOptions> options) : base(options) { }

    #endregion

    //#region - Public Methods -

    //public async Task<Deliscio.Common.Settings.Settings?> GetByIdAsync<TValues>(string id, CancellationToken token = default)
    //{
    //    var rslt = await GetAsync(new ObjectId(id), token);

    //    if (rslt is null)
    //        return default;

    //    return Mapper.Map(rslt);
    //}

    //public async Task<Settings<TValues>?> GetByNameAsync<TValues>(string name, CancellationToken token = default)
    //{
    //    var rslt = await FirstOrDefaultAsync(x => x.Name == name && x.IsPublished == true, token);

    //    if (rslt is null)
    //        return default;

    //    return Mapper.Map<TValues>(rslt);
    //}

    //public Task<IEnumerable<Settings<TValues>?>> GetAllByNameAsync<TValues>(string name, CancellationToken token = default)
    //{
    //    throw new NotImplementedException();
    //}

    //public async Task<string> Save<TValue>(Settings<TValue> settings, string createdByUserId, CancellationToken token = default)
    //{
    //    if (settings is null)
    //        throw new ArgumentNullException(nameof(settings));

    //    if (!string.IsNullOrWhiteSpace(settings.Id) && !ObjectId.Parse(settings.Id).Equals(ObjectId.Empty))
    //        throw new ArgumentException("Settings Id must be empty or null.", nameof(settings.Id));

    //    var entity = SettingsEntity.Create(settings.Name, settings.Environment, settings.Values, createdByUserId);

    //    var rslt = await AddAsync(entity, token);

    //    return rslt.ToString();
    //}

    //public async Task<bool> Update<TValue>(string id, Settings<TValue> settings, string userId, CancellationToken token = default)
    //{
    //    var objectId = ObjectId.Parse(id);

    //    var existing = await base.GetAsync(objectId, token);

    //    if (existing is null)
    //        throw new KeyNotFoundException($"Settings with id {id} not found.");

    //    existing.IsPublished = false;
    //    existing.DateUpdated = DateTimeOffset.UtcNow;
    //    existing.UpdatedById = ObjectId.Parse(userId);


    //    var newSettings = SettingsEntity.Create(settings.Name, settings.Environment, settings.Values, userId);
    //    newSettings.Version = existing.Version + 1;
    //    newSettings.IsPublished = true;

    //    var rslt = await UpdateAsync(newSettings, token);

    //    return rslt;
    //}

    //#endregion
}