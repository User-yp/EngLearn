using Listening.Domain.Entity;
using MediatR;

namespace Listening.Domain.Events;

public record EpisodeUpdatedEvent(Episode Value) : INotification;
