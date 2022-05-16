namespace bejebeje.admin.Domain.Events;

public class TodoItemCompletedEvent : DomainEvent
{
    public TodoItemCompletedEvent(Lyric item)
    {
        Item = item;
    }

    public Lyric Item { get; }
}
