﻿using Akka.TestKit.Xunit2;
using Xunit;
using Akka.Actor;
using Akka.Routing;
using SalesOrder.Messages;
using SalesOrder.Actors;
using System.Threading.Tasks;
using System;

namespace SalesOrder.Tests
{
    public class SessionTests : TestKit
    {
        [Fact]
        public async Task SessionShouldBeDistributed()
        {
            IActorRef sessionCollectionActor = ActorOf<SessionCollectionActor>("session-router");
            
            for (int index = 0; index < 1000; index++)
            {
                string id = $"{ index }";

                SessionFound sessionFound = await sessionCollectionActor.Ask<SessionFound>(new FindSession(id), TimeSpan.FromSeconds(2));

                if (sessionFound.SessionActor.IsNobody())
                {
                    SessionCreated sessionCreated = await sessionCollectionActor.Ask<SessionCreated>(new CreateSession(id, string.Empty), TimeSpan.FromSeconds(2));
                }
            }
        }
    }
}
