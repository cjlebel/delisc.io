using Deliscio.Common.Interfaces;
using FluentResults;

namespace Deliscio.Modules.Settings.Interfaces;

public interface ISettingsService
{
    //Task<Result<TSettings?>> GetByIdAsync<TSettings>(string id, CancellationToken token = default) where TSettings : ISettings;

   // ValueTask<Result<Settings<TSettings>?>> GetByNameAsync<TSettings>(string name, CancellationToken token = default) where TSettings : ISettings;

    Task<string> GetAllGroups();
}