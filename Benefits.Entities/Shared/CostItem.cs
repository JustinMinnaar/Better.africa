using Benefits.Shared;
using System;

namespace Benefits.Entities
{
    public class TransactionItem : BaseRow
    {
        public Guid TransactionId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public TransactionItem(string name, decimal cost)
        {
            this.Name = name;
            this.Amount = cost;
        }
    }
}