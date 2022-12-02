using DiscordRPC;

using System;

using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace Bannerlord.DiscordRichPresence.CampaignBehaviors
{
    internal sealed class TrackingCampaignBehavior : CampaignBehaviorBase
    {
        private readonly Action<RichPresence, bool> _setPresence;
        private readonly Action<bool> _setPreviousPresence;

        public TrackingCampaignBehavior(Action<RichPresence, bool> setPresence, Action<bool> setPreviousPresence)
        {
            _setPresence = setPresence;
            _setPreviousPresence = setPreviousPresence;
        }

        public override void SyncData(IDataStore dataStore) { }

        public override void RegisterEvents()
        {
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);

            CampaignEvents.SettlementEntered.AddNonSerializedListener(this, OnSettlementEntered);
            CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, OnSettlementLeft);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);

            CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, OnPlayerCharacterChanged);

            CampaignEvents.PlayerEliminatedFromTournament.AddNonSerializedListener(this, OnPlayerEleminatedFromTournament);

            CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, OnMissionStarted);
            CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, OnMissionEnded);

            CampaignEvents.SetupPreConversationEvent.AddNonSerializedListener(this, OnSetupPreConversation);
            CampaignEvents.ConversationEnded.AddNonSerializedListener(this, OnConversationEnded);
        }

        private void OnSetupPreConversation()
        {
            if (PlayerEncounter.EncounteredParty is { } party)
            {
                _setPresence(PresenceStates.CampaignConversation(Hero.MainHero, party), true);
            }
        }

        private void OnConversationEnded(CharacterObject obj)
        {
            CheckCurrentState();
        }

        private void OnGameLoaded(CampaignGameStarter gameStarter)
        {
            CheckCurrentState();
        }

        private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
        {
            if (mobileParty != MobileParty.MainParty)
                return;

            _setPresence(PresenceStates.CampaignInSettlement(settlement), true);
        }
        private void OnSettlementLeft(MobileParty mobileParty, Settlement settlement)
        {
            if (mobileParty != MobileParty.MainParty)
                return;

            CheckCurrentState();
        }

        /// <summary>
        /// Fallback
        /// </summary>
        private void OnDailyTick()
        {
            CheckCurrentState();
        }

        private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
        {
            CheckCurrentState();
        }

        private void OnPlayerEleminatedFromTournament(int round, Town town)
        {
            CheckCurrentState();
        }

        private void OnMissionStarted(IMission baseMission)
        {
            if (MapEvent.PlayerMapEvent is { } mapEvent)
            {
                switch (mapEvent.PlayerSide)
                {
                    case BattleSideEnum.Attacker:
                        _setPresence(PresenceStates.CampaignAttacking(mapEvent), true);
                        break;
                    case BattleSideEnum.Defender:
                        _setPresence(PresenceStates.CampaignDefending(mapEvent), true);
                        break;
                }
            }

            if (PlayerEncounter.LocationEncounter is { } locationEncounter && CampaignMission.Current.Location is { } location)
            {
                switch (location.StringId)
                {
                    case CampaignData.LocationLordsHall:
                    case CampaignData.LocationTavern:
                    case CampaignData.LocationArena:
                    case CampaignData.LocationPrison:
                    {
                        _setPresence(PresenceStates.CampaignInSettlementMission(locationEncounter.Settlement, location), true);
                        break;
                    }
                    case CampaignData.LocationCenter: // Town and Castle
                    case CampaignData.LocationVillageCenter:
                    {
                        _setPresence(PresenceStates.CampaignInSettlement(locationEncounter.Settlement), true);
                        break;
                    }
                    case "training_field":
                    {

                        break;
                    }
                }
            }
        }
        private void OnMissionEnded(IMission baseMission)
        {
            CheckCurrentState();
        }

        private void CheckCurrentState()
        {
            if (Hero.MainHero.CurrentSettlement is { } currentSettlement)
            {
                _setPresence(PresenceStates.CampaignInSettlement(currentSettlement), true);
                return;
            }

            if (GetNearestSettltment(Hero.MainHero) is { } settlement)
            {
                _setPresence(PresenceStates.CampaignTravelling(Hero.MainHero, settlement), true);
                return;
            }
        }

        private static Settlement? GetNearestSettltment(Hero hero)
        {
            if (Settlement.All == null || Settlement.All.Count == 0)
                return null;

            if (hero.CurrentSettlement is { IsHideout: false } heroSettlement)
                return heroSettlement;

            if (!hero.IsActive &&!hero.IsWanderer)
                return hero.HomeSettlement;

            var distanceCurrent = float.MaxValue;
            var settemenuCurrent = default(Settlement);
            var asVec = hero.GetPosition().AsVec2;
            foreach (var settlement in Settlement.All.Where(x => !x.IsHideout))
            {
                if (settlement.GatePosition.DistanceSquared(asVec) is var distance && distance < distanceCurrent)
                {
                    distanceCurrent = distance;
                    settemenuCurrent = settlement;
                }
            }

            return settemenuCurrent;
        }
    }
}