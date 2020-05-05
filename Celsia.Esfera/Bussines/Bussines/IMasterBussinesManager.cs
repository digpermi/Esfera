namespace Bussines.Bussines
{
    public interface IMasterBussinesManager
    {
        IIdentificationTypeBussines IdentificationTypeBussines { get; }

        IExternalSystemBussines ExternalSystemBussines { get; }

        IInterestBussines InterestBussines { get; }

        IRelationshipBussines RelationshipBussines { get; }
    }
}
