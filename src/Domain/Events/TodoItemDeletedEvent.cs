namespace bejebeje.admin.Domain.Events;

public class TodoItemDeletedEvent : DomainEvent
{
    public TodoItemDeletedEvent(Lyric item)
    {
        Item = item;
    }

    public Lyric Item { get; }
}
