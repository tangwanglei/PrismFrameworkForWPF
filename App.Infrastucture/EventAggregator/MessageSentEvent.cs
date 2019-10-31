using Prism.Events;

namespace App.Infrastucture.EventAggregator
{
    public class MessageSentEvent : PubSubEvent<string>
    {
    }
}
