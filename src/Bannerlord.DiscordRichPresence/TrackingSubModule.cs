using DiscordRPC;

using System;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.DiscordRichPresence
{
    internal sealed class TrackingSubModule : MBSubModuleBase
    {
        private readonly Action<RichPresence, bool> _setPresence;
        private readonly Action<bool> _setPreviousPresence;

        public TrackingSubModule()
        {
            _setPresence = DiscordSubModule.Instance.SetPresence;
            _setPreviousPresence = DiscordSubModule.Instance.SetPreviousPresence;
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            _setPresence(PresenceStates.Loading(), true);
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            _setPresence(PresenceStates.InMainMenu(), true);
        }

        /*
        private bool _inMenu;
        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            if (GameStateManager.Current is { ActiveState.IsMenuState: true })
            {
                if (!_inMenu)
                {
                    _inMenu = true;
                    _setPresence(PresenceStates.InMenu(), true);
                }
            }
            else
            {
                if (_inMenu)
                {
                    _inMenu = false;
                    _setPreviousPresence(true);
                }
            }
        }
        */

        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            base.InitializeGameStarter(game, starterObject);

            _setPresence(PresenceStates.Loading(), true);
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);

            if (game.GameType.GetType().FullName == "TaleWorlds.MountAndBlade.CustomBattle.CustomGame")
            {
                _setPresence(PresenceStates.InCustomBattle(), true);
            }
        }
    }
}