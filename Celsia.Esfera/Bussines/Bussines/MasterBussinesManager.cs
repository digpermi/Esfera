namespace Bussines.Bussines
{
    public class MasterBussinesManager : IMasterBussinesManager
    {
        private readonly EsferaContext context;

        public MasterBussinesManager(EsferaContext context)
        {
            this.context = context;
        }


        private IIdentificationTypeBussines _identificationTypeBussines;
        private IExternalSystemBussines _externalSystemBussines;
        private IInterestBussines _interestBussines;
        private IRelationshipBussines _relationshipBussines;

        public IIdentificationTypeBussines IdentificationTypeBussines
        {
            get
            {
                return this._identificationTypeBussines ??= new IdentificationTypeBussines(this.context);
            }
        }

        public IExternalSystemBussines ExternalSystemBussines
        {
            get
            {
                return this._externalSystemBussines ??= new ExternalSystemBussines(this.context);
            }
        }

        public IInterestBussines InterestBussines
        {
            get
            {
                return this._interestBussines ??= new InterestBussines(this.context);
            }
        }

        public IRelationshipBussines RelationshipBussines
        {
            get
            {
                return this._relationshipBussines ??= new RelationshipBussines(this.context);
            }
        }
    }
}
