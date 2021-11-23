using Domain.BaseModels;

namespace Domain.AggregationModels.MerchandiseRequest
{
    public class MerchPackType : Enumeration
    {
        public static MerchPackType WelcomePack = new(1, nameof(WelcomePack));
        public static MerchPackType StarterPack = new(2, nameof(StarterPack));
        public static MerchPackType ConferenceListenerPack = new(3, nameof(ConferenceListenerPack));
        public static MerchPackType ConferenceSpeakerPack = new(4, nameof(ConferenceSpeakerPack));
        public static MerchPackType VeteranPack = new(5, nameof(VeteranPack));

        public static MerchPackType Parse(string size) => size?.ToUpper() switch
        {
            nameof(WelcomePack) => WelcomePack,
            nameof(StarterPack) => ConferenceListenerPack,
            nameof(ConferenceListenerPack) => ConferenceListenerPack,
            nameof(ConferenceSpeakerPack) => ConferenceListenerPack,
            nameof(VeteranPack) => ConferenceListenerPack,
            _ => throw new DomainException("Unknown merch pack type")
        };
        
        private MerchPackType(int id, string name) : base(id, name)
        {
        }
    }
}