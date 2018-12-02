namespace Benefits.Demo
{
    internal class AppPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string EmailAddress { get; internal set; }
        public string EmployedAt { get; internal set; }
        public string EmployedAtPhone { get; internal set; }
    }
}