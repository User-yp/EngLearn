using MediaEncoder.Domain.Entity;
using MediatR;

namespace MediaEncoder.Domain.Event;

public record EncodingItemCreatedEvent(EncodingItem Value) : INotification;