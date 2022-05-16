using System.Globalization;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.TodoLists.Queries.ExportTodos;
using bejebeje.admin.Infrastructure.Files.Maps;
using CsvHelper;

namespace bejebeje.admin.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }
}
