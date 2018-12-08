namespace BetterAfrica.Benefits.Entities
{
    public enum EPersonRelated
    {
        Father,
        Mother,
        Brother,
        Sister,
        Uncle,
        Aunt,
        Grandfather,
        Grandmother,
        Greatgrandfather,
        Greatgrandmother,
        Cousin,
    }

    public enum EPersonGenders
    {
        Male,
        Female
    }

    public enum ECommunicateVia
    {
        Agent,
        Email,
    }

    public enum EBeneficiariesType
    {
        Spouse,
        Children,
        Specified,
    }

    public enum EFormAction
    {
        Add,
        Update,
        Delete,
    }

    public enum ELanguage
    {
        English,
        Afrikaans,
    }
}