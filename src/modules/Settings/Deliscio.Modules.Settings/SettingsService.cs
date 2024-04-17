using Deliscio.Common.Interfaces;
using Deliscio.Modules.Settings.Interfaces;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.Settings;

public class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;
    private readonly ISettingsRepository _settingsRepository;

    public SettingsService(ISettingsRepository settingsRepository, ILogger<SettingsService> logger)
    {
        _logger = logger;
        _settingsRepository = settingsRepository;
    }

    //public async Task<Result<TSettings?>> GetByIdAsync<TSettings>(string id, CancellationToken token = default) where TSettings : ISettings
    //{
    //    if (string.IsNullOrWhiteSpace(id))
    //        return Result.Fail("The id is null or empty.");

    //    var rslt = await _settingsRepository.GetByIdAsync<TSettings>(id, token);

    //    if (rslt is null)
    //    {
    //        _logger.LogWarning("Settings with id {id} not found.", id);

    //        return Result.Fail($"Settings with the id {id} not found.");
    //    }

    //    var settings = (TSettings)(object)rslt.Values;

    //    return Result.Ok(settings);
    //}

    //public async ValueTask<Result<Settings<ISettings>?>> GetByNameAsync<TSettings>(string name, CancellationToken token = default)
    //{
    //    var settings = await _settingsRepository.GetByNameAsync<TSettings>(name, token);

    //    if (settings is null)
    //    {
    //        _logger.LogWarning("Settings with the name '{name}' not found.", name);

    //        return Result.Fail($"Settings with the name '{name}' not found.");
    //    }

    //    return settings;
    //}

    public Task<string> GetAllGroups()
    {
        throw new NotImplementedException();
    }

    //public async ValueTask<ObjectId> Save<TSettings>(Settings<TSettings> settings, CancellationToken token = default)
    //{
    //    var entity = Mappers.Mapper.Map(settings);

    //    var rslt = await _settingsRepository.AddAsync(entity, token);

    //    return rslt;
    //}
}
