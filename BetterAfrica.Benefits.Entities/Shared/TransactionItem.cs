using BetterAfrica.Shared;
using System;

namespace BetterAfrica.Benefits.Entities
{
    public class TransactionItem : BaseRow
    {
        public int TransactionId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public TransactionItem(string name, decimal cost)
        {
            this.Name = name;
            this.Amount = cost;
        }
    }
}