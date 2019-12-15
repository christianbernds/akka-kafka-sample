namespace ProcessEngine.IntermediateMessageThrow.Kafka
{
    using System;
    using System.Linq;

    using Akka;
    using Akka.Actor;
    using Akka.Configuration;
    using Akka.Streams;
    using Akka.Streams.Dsl;
    using Akka.Streams.Kafka.Dsl;
    using Akka.Streams.Kafka.Messages;
    using Akka.Streams.Kafka.Settings;

    using Confluent.Kafka;

    using ProcessEngine.IntermediateMessageThrow.Kafka.Messages;

    using Config = Akka.Configuration.Config;

    public static class Program
    {
        static void Main(string[] args)
        {
            Config fallbackConfig = ConfigurationFactory.ParseString(@"
                    akka.suppress-json-serializer-warning=true
                    akka.loglevel = DEBUG
                ").WithFallback(ConfigurationFactory.FromResource<ConsumerSettings<object, object>>("Akka.Streams.Kafka.reference.conf"));

            var system = ActorSystem.Create("TestKafka", fallbackConfig);

            var throwEventActor = system.ActorOf(Props.Create<IntermediateMessageThrowEventActor>());

            var sequenceFlowInput = new SequenceFlowInput
            {
                ProcessInstanceId = Guid.NewGuid(),
                Payload = new InputPayload
                {
                    Status = "Exported",
                    EventId = Guid.NewGuid().ToString(),
                }
            };

            throwEventActor.Tell(sequenceFlowInput);

            Console.ReadLine();
        }
    }
}
