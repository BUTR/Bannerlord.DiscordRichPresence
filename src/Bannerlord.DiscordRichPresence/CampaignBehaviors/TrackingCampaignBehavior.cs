using System;

using DiscordRPC;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;

namespace Bannerlord.DiscordRichPresence.CampaignBehaviors
{
    internal sealed class TrackingCampaignBehavior : CampaignBehaviorBase
    {
        private readonly Action<RichPresence> _setPresence;
        private readonly Action _setPreviousPresence;

        public TrackingCampaignBehavior(Action<RichPresence> setPresence, Action setPreviousPresence)
        {
            _setPresence = setPresence;
            _setPreviousPresence = setPreviousPresence;
        }

        public override void SyncData(IDataStore dataStore) { }

        public override void RegisterEvents()
        {
            CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, OnPlayerCharacterChanged);
        }

        private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
        {
            _setPresence(PresenceStates.InCampaign(newPlayer));
        }
    }
}