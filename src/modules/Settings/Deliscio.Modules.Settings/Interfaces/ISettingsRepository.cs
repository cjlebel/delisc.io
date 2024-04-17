using Deliscio.Core.Data.Interfaces;
using Deliscio.Modules.Settings.Data.Entities;
using MongoDB.Bson;

namespace Deliscio.Modules.Settings.Interfaces;

public interface ISettingsRepository : IRepository<SettingsEntity, ObjectId>
{
    //Task<Settings<TValues>?> GetByIdAsync<TValues>(string id, CancellationToken token = default);

    //Task<Settings<TValues>?> GetByNameAsync<TValues>(string name, CancellationToken token = default);

    //Task<IEnumerable<Settings<TValues>?>> GetAllByNameAsync<TValues>(string name, CancellationToken token = default);

    //Task<string> Save<TValue>(Settings<TValue> settings, string createdByUserId, CancellationToken token = default);

    //Task<bool> Update<TValue>(string id, Settings<TValue> settings, string userId, CancellationToken token = default);
}