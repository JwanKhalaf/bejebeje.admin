using bejebeje.admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
