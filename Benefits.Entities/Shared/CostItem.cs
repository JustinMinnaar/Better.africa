namespace Benefits.Entities
{
    public class CostItem
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }

        public CostItem(string name, decimal cost)
        {
            this.Name = name;
            this.Cost = cost;
        }
    }
}