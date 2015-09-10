using System;
using Akka;
using Akka.Actor;

namespace AkkaExperiments
{
    public class Transfer
    {
        /// <summary>
        /// Transfer message to send to a BankAccountActor
        /// </summary>
        /// <param name="amount">amount to send</param>
        /// <param name="dest">destination BankAccountActor</param>
        public Transfer(int amount, IActorRef dest)
        {
            this.amount = amount;
            this.dest = dest;
        }
        public int amount { get; private set; }
        public IActorRef dest { get; private set; }
    }
    /// <summary>
    /// Balance message to send to a BankAccountActor
    /// </summary>
    public class Balance  {}

    public class Deposit
    {
        /// <summary>
        /// Deposit message to send to a BankAccountActor
        /// </summary>
        /// <param name="amount">amount to sent</param>
        public Deposit(int amount)
        {
            this.amount = amount;
        }
        public int amount { get; private set; }
    }
    
    /// <summary>
    /// Actor representing a bankaccount
    /// </summary>
    public class BankAccountActor : ReceiveActor
    {
        private int balance;

        /// <summary>
        /// Initializes a bankaccount with a balance of 1000
        /// Registers behavior when receiving a message
        /// </summary>
        public BankAccountActor() {
            balance = 1000;
            
            Receive<Deposit>(msg => balance += msg.amount);
            Receive<Balance>(msg => Sender.Tell(balance));
            Receive<Transfer>(msg => transfer(msg));
        }

        private void transfer(Transfer msg)
        {
            balance -= msg.amount;
            msg.dest.Tell(new Deposit(msg.amount));
        }
    }
}
