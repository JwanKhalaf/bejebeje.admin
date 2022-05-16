using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;

namespace bejebeje.admin.Application.TodoLists.Queries.ExportTodos;

public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
