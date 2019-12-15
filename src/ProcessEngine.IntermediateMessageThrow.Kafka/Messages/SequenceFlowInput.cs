namespace ProcessEngine.IntermediateMessageThrow.Kafka.Messages
{
    public class SequenceFlowInput : SequenceFlow
    {
        public InputPayload Payload { get; set; }
    }

    public class InputPayload
    {
        public string EventId { get; set; }

        public string Status { get; set; }

        public override string ToString()
        {
            return $"EventId: {EventId} - Status: {Status}";
        }
    }
}
