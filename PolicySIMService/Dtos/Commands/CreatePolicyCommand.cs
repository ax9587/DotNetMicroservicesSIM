

namespace PolicySIMService.Dtos.Commands
{
    public class CreatePolicyCommand 
    {
        public string OfferNumber { get; set; }
        public PersonDto PolicyHolder { get; set; }
        public AddressDto PolicyHolderAddress { get; set; }
    }
}
