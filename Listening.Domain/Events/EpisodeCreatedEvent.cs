using Listening.Domain.Entity;
using MediatR;

namespace Listening.Domain.Events;

public record EpisodeCreatedEvent(Episode Value) : INotification;