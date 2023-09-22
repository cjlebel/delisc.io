namespace Deliscio.Modules.QueuedLinks.Common.Models;

public record NewQueuedLink(string Url, string SubmittedByUserId, string UsersTitle = "", string UsersDescription = "", string[]? Tags = default);