using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;

namespace bejebeje.admin.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class TodoItemBriefDto : IMapFrom<TodoItem>
{
    public int Id { get; set; }

    public int ListId { get; set; }

    public string? Title { get; set; }

    public bool Done { get; set; }
}
