using bejebeje.admin.Application.TodoLists.Queries.ExportTodos;

namespace bejebeje.admin.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
