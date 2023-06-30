using Bannerlord.DiscordRichPresence.Utils;

using DiscordRPC;

using System;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.View.Tableaus;

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
            CampaignEvents.MissionTickEvent.AddNonSerializedListener(this, OnMissionTick);
            CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, OnMissionEnded);

            CampaignEvents.SetupPreConversationEvent.AddNonSerializedListener(this, OnSetupPreConversation);
#if v100 || v101 || v102 || v103
            CampaignEvents.ConversationEnded.AddNonSerializedListener(this, OnConversationEnded);
#elif v110 || v111 || v112 || v113
            CampaignEvents.ConversationEnded.AddNonSerializedListener(this, OnConversationEnded);
#endif

            if (GetCampaignBehavior<CampaignEventsEx>() is { } campaignEventsEx)
            {
                campaignEventsEx.BattleSimulationStarted.AddNonSerializedListener(this, OnBattleSimulationStarted);
                campaignEventsEx.BattleSimulationEnding.AddNonSerializedListener(this, OnBattleSimulationEnding);
            }
        }

        private void OnSetupPreConversation()
        {
            if (PlayerEncounter.EncounteredParty is { } party)
            {
                _setPresence(PresenceStates.CampaignConversation(Hero.MainHero, party), true);
            }
        }

#if v100 || v101 || v102 || v103
        private void OnConversationEnded(CharacterObject obj)
#elif v110 || v111 || v112 || v113 || v114 || v115 || v120
        private void OnConversationEnded(System.Collections.Generic.IEnumerable<CharacterObject> objs)
#else
#error SET VERSION
#endif
        {
            CheckCurrentState();
        }

        private void OnGameLoaded(CampaignGameStarter gameStarter)
        {
            TableauCacheManager.Current.BeginCreateCharacterTexture(CharacterCode.CreateFrom(Hero.MainHero.CharacterObject), AvatarUploader.UploadCharacterTexture, isBig: true);
            //TableauCacheManager.Current.BeginCreateBannerTexture(BannerCode.CreateFrom(Hero.MainHero.ClanBanner), AvatarUploader.UploadBannerTexture);

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
            TableauCacheManager.Current.BeginCreateCharacterTexture(CharacterCode.CreateFrom(newPlayer.CharacterObject), AvatarUploader.UploadCharacterTexture, isBig: true);

            CheckCurrentState();
        }

        private void OnPlayerEleminatedFromTournament(int round, Town town)
        {
            CheckCurrentState();
        }

        private bool? _isBattleSimulated;
        private float _missionTickCounter = 0;
        private void OnMissionStarted(IMission baseMission)
        {
            _isBattleSimulated = true;
            _missionTickCounter = 0f;

            if (MapEvent.PlayerMapEvent is { } mapEvent)
            {
                switch (mapEvent.PlayerSide)
                {
                    case BattleSideEnum.Attacker:
                        _setPresence(PresenceStates.CampaignAttacking(mapEvent, false), true);
                        break;
                    case BattleSideEnum.Defender:
                        _setPresence(PresenceStates.CampaignDefending(mapEvent, false), true);
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
        private void OnMissionTick(float dt)
        {
            _missionTickCounter += dt;
            if (_missionTickCounter > 3f)
            {
                _missionTickCounter = 0f;
                if (MapEvent.PlayerMapEvent is { } mapEvent)
                {
                    switch (mapEvent.PlayerSide)
                    {
                        case BattleSideEnum.Attacker:
                            _setPresence(PresenceStates.CampaignAttacking(mapEvent, _isBattleSimulated!.Value), true);
                            break;
                        case BattleSideEnum.Defender:
                            _setPresence(PresenceStates.CampaignDefending(mapEvent, _isBattleSimulated!.Value), true);
                            break;
                    }
                }
            }
        }
        private void OnMissionEnded(IMission baseMission)
        {
            _isBattleSimulated = null;
            _missionTickCounter = 0f;
            CheckCurrentState();
        }

        private void OnBattleSimulationStarted(BattleSimulation battleSimulation)
        {
            _isBattleSimulated = true;
            if (battleSimulation.MapEvent is { } mapEvent)
            {
                switch (mapEvent.PlayerSide)
                {
                    case BattleSideEnum.Attacker:
                        _setPresence(PresenceStates.CampaignAttacking(mapEvent, _isBattleSimulated.Value), true);
                        break;
                    case BattleSideEnum.Defender:
                        _setPresence(PresenceStates.CampaignDefending(mapEvent, _isBattleSimulated.Value), true);
                        break;
                }
            }
            //_setPresence(PresenceStates.CampaignAttacking(mapEvent), true);

        }
        private void OnBattleSimulationEnding(BattleSimulation battleSimulation)
        {
            _isBattleSimulated = null;
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

            if (hero.CurrentSettlement is {IsHideout: false} heroSettlement)
                return heroSettlement;

            if (!hero.IsActive && !hero.IsWanderer)
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