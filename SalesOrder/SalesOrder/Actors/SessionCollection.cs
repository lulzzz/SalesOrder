using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;

using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class SessionCollectionActor : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Context.GetLogger();

        public SessionCollectionActor()
        {
            Receive<CreateSession>(message => CreateSession(message));
            Receive<FindSession>(message => FindSession(message));
        }

        private void CreateSession(CreateSession createSession)
        {
            logger.Info("Create session (ID: {0}, UserID: {1})", createSession.Id, createSession.UserId);

            // IActorRef sessionActor = Context.ActorOf(Context.DI().Props<SessionActor>(), $"session-{ createSession.Id }");
            IActorRef sessionActor = Context.ActorOf(Props.Create<SessionActor>(), $"session-{ createSession.Id }");

            sessionActor.Forward(createSession);
        }

        private void FindSession(FindSession findSession)
        {
            logger.Info("Find session (ID: {0})", findSession.Id);

            IActorRef sessionActor = Context.Child($"session-{ findSession.Id }");

            SessionFound sessionFound = new SessionFound(findSession.Id, sessionActor);

            Sender.Tell(sessionFound);
        }
    }
}
