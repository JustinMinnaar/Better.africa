namespace Better.Benefits.Provider
{
    public class IdentityModel
    {
        public RowModel Row { get; internal set; }

        public string CountryCode { get; set; }
        public EIdentityType IdentityType { get; set; }
        public string Number { get; set; }
    }
}