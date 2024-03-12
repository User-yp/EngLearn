using MediatR;

namespace MediaEncoder.Domain.Event;

public record EncodingItemStartedEvent(Guid Id, string SourceSystem) : INotification;