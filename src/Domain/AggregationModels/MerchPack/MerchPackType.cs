using Domain.BaseModels;

namespace Domain.AggregationModels.MerchPack
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
            "welcome_pack" => WelcomePack,
            "conference_listener_pack" => ConferenceListenerPack,
            "conference_speaker_pack" => ConferenceListenerPack,
            "probation_period_ending_pack" => ConferenceListenerPack,
            "veteran_pack" => ConferenceListenerPack,
            _ => throw new DomainException("Unknown merch pack type")
        };
        
        private MerchPackType(int id, string name) : base(id, name)
        {
        }
    }
}