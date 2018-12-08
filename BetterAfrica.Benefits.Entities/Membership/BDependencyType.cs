namespace BetterAfrica.Benefits.Entities
{
    // TODO: Check each month if any member is going to soon be too old to be covered
    // TODO: A child becomes extended member when to old to be a child, or scholar.
    public enum BDependencyType
    {
        /// <summary>The principal member is responsible for payment and receives payouts unless he/she dies.</summary>
        Principal = 1,

        /// <summary></summary>
        Spouse = 2,

        /// <summary></summary>
        Child = 3,

        /// <summary></summary>
        Family = 5,
    }
}