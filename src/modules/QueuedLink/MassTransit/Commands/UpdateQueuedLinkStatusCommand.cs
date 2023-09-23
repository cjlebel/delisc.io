using Deliscio.Modules.QueuedLinks.Common.Models;

namespace Deliscio.Modules.QueuedLinks.MassTransit.Commands;

public sealed record UpdateQueuedLinkStatusCommand(QueuedLink Link);