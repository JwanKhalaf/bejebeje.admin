using bejebeje.admin.Application.Common.Interfaces;

namespace bejebeje.admin.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}