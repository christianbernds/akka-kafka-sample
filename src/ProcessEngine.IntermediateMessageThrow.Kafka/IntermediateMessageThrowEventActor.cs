namespace ProcessEngine.IntermediateMessageThrow.Kafka
{
    using System;
    using System.Threading.Tasks;

    using Akka;
    using Akka.Actor;
    using Akka.Streams;
    using Akka.Streams.Dsl;
    using Akka.Streams.Kafka.Dsl;
    using Akka.Streams.Kafka.Messages;
    using Akka.Streams.Kafka.Settings;

    using Confluent.Kafka;

    using ProcessEngine.IntermediateMessageThrow.Kafka.Messages;

    public class IntermediateMessageThrowEventActor : ReceiveActor
    {
        private const string KafkaEndpoint = "localhost:9092";
        private const string TopicName = "test";

        public IntermediateMessageThrowEventActor()
        {
            this.Materializer = Context.System.Materializer();

            this.ReceiveAsync<SequenceFlowInput>(
                async (inputMessage) => {
                    Console.WriteLine($"Entered {nameof(IntermediateMessageThrowEventActor)} for {inputMessage.ProcessInstanceId}");

                    await this.Send(inputMessage);
                    await this.SendWithAkka(inputMessage);

                    Console.WriteLine($"Exited {nameof(IntermediateMessageThrowEventActor)} for {inputMessage.ProcessInstanceId}");
                });
        }

        public ActorMaterializer Materializer { get; set; }

        public async Task SendWithAkka(SequenceFlowInput inputMessage)
        {
            var producerSettings = ProducerSettings<Null, string>.Create(Context.System, null, null)
                                                                 .WithBootstrapServers(KafkaEndpoint);

            await Source
                  .Single("Akka: " + inputMessage.ProcessInstanceId.ToString())
                  .Select(kafkaMessage => ProducerMessage.Single(new ProducerRecord<Null, string>(TopicName, kafkaMessage)))
                  .Via(KafkaProducer.FlexiFlow<Null, string, NotUsed>(producerSettings))
                  .RunWith(Sink.Ignore<IResults<Null, string, NotUsed>>(), Materializer);

        }

        public async Task Send(SequenceFlowInput inputMessage)
        {
            var config = new ProducerConfig { BootstrapServers = KafkaEndpoint };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    await p.ProduceAsync("test", new Message<Null, string>
                    {
                        Value = "PLAIN: " + inputMessage.ProcessInstanceId.ToString()
                    });
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
