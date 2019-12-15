namespace ProcessEngine.IntermediateMessageThrow.Kafka.Messages
{
    using System;

    public abstract class SequenceFlow
    {
        public Guid ProcessInstanceId { get; set; }
    }
}
