using MediatR;

namespace MediaEncoder.Domain.Event;

public record EncodingItemFailedEvent(Guid Id, string SourceSystem, string ErrorMessage) : INotification;