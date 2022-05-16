using bejebeje.admin.Domain.Common;

namespace bejebeje.admin.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
