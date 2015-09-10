using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Akka;
using Akka.Actor;
using System.Threading.Tasks;


namespace AkkaExperiments
{
    [TestClass]
    public class AkkaTest
    {
        ActorSystem system;
        IActorRef src;
        IActorRef dst;

        /// <summary>
        /// Setup the actor system and create two actors (supervised by the system)
        /// </summary>
        [TestInitialize]
        public void setup()
        {
            system = ActorSystem.Create("MySystem");
            src = system.ActorOf<BankAccountActor>("src");
            dst = system.ActorOf<BankAccountActor>("dst");

        }

        /// <summary>
        /// Send a transfer message to the src actor for 500 to dst
        /// Concurrently ask the destination its current balance
        /// Both messages are in a race
        /// </summary>
        [TestMethod]
        public void testBankActorNonDeterministic()
        {
            src.Tell(new Transfer(500, dst));

            //Change the delay (1 .. 1000) in order to observe the different behavior (not guaranteed)
            var t = Task.Delay(1000);
            t.Wait();

                      
            var t2 = dst.Ask(new Balance());

            
            Assert.AreEqual(1500, t2.Result);
        }        
    }
}
