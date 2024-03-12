using MediatR;

namespace MediaEncoder.Domain.Event;

public record EncodingItemCompletedEvent(Guid Id, string SourceSystem, Uri OutputUrl) : INotification;