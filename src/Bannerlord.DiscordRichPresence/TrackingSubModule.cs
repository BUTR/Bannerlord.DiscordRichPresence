using System;

using DiscordRPC;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.DiscordRichPresence
{
    internal sealed class TrackingSubModule : MBSubModuleBase
    {
        private readonly Action<RichPresence> _setPresence;
        private readonly Action _setPreviousPresence;

        public TrackingSubModule()
        {
            _setPresence = DiscordSubModule.Instance.SetPresence;
            _setPreviousPresence = DiscordSubModule.Instance.SetPreviousPresence;
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            _setPresence(PresenceStates.Loading());
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            _setPresence(PresenceStates.InMainMenu());
        }

        private bool _inMenu;
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            if (GameStateManager.Current is { ActiveState.IsMenuState: true })
            {
                if (!_inMenu)
                {
                    _inMenu = true;
                    _setPresence(PresenceStates.InMenu());
                }
            }
            else
            {
                if (_inMenu)
                {
                    _inMenu = false;
                    _setPreviousPresence();
                }
            }
        }

        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            base.InitializeGameStarter(game, starterObject);

            _setPresence(PresenceStates.Loading());
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            if (game.GameType is Campaign campaign)
            {
                _setPresence(PresenceStates.InCampaign(campaign.MainParty.LeaderHero));
            }

            if (game.GameType.GetType().FullName == "TaleWorlds.MountAndBlade.CustomBattle.CustomGame")
            {
                _setPresence(PresenceStates.InCustomBattle());
            }
        }
    }
}