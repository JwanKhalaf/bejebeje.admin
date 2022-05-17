namespace bejebeje.admin.Domain.Events;

public class TodoItemCreatedEvent : DomainEvent
{
    public TodoItemCreatedEvent(Lyric item)
    {
        Item = item;
    }

    public Lyric Item { get; }
}
