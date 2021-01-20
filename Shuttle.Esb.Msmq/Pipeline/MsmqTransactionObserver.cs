using System.Messaging;
using Shuttle.Core.Pipelines;

namespace Shuttle.Esb.Msmq
{
    public class MsmqTransactionObserver :
        IPipelineObserver<OnStart>,
        IPipelineObserver<OnBeginTransaction>,
        IPipelineObserver<OnCommitTransaction>,
        IPipelineObserver<OnDispose>
    {
        public void Execute(OnBeginTransaction pipelineEvent)
        {
            var tx = pipelineEvent.Pipeline.State.Get<MessageQueueTransaction>();

            if (tx == null)
            {
                return;
            }

            tx.Begin();
        }

        public void Execute(OnCommitTransaction pipelineEvent)
        {
            var tx = pipelineEvent.Pipeline.State.Get<MessageQueueTransaction>();

            if (tx == null)
            {
                return;
            }

            tx.Commit();
        }

        public void Execute(OnDispose pipelineEvent)
        {
            var tx = pipelineEvent.Pipeline.State.Get<MessageQueueTransaction>();

            if (tx == null)
            {
                return;
            }

            tx.Dispose();
            pipelineEvent.Pipeline.State.Replace<MessageQueueTransaction>(null);
        }

        public void Execute(OnStart pipelineEvent)
        {
            pipelineEvent.Pipeline.State.Add(new MessageQueueTransaction());
        }
    }
}