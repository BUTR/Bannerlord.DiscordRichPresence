using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.DiscordRichPresence.CampaignBehaviors;
using Bannerlord.DiscordRichPresence.Utils;

using DiscordRPC;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.DiscordRichPresence
{
    internal sealed class DiscordSubModule : MBSubModuleBase
    {
        internal static DiscordSubModule Instance { get; private set; } = default!;

        private bool ServiceRegistrationWasCalled { get; set; }
        private bool DelayedServiceCreation { get; set; }
        private bool OnBeforeInitialModuleScreenSetAsRootWasCalled { get; set; }
        private DiscordRpcClient? Client { get; set; }

        public DiscordSubModule()
        {
            Instance = this;
        }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            IServiceProvider serviceProvider;
            if (!ServiceRegistrationWasCalled)
            {
                OnServiceRegistration();
                DelayedServiceCreation = true;
                serviceProvider = this.GetTempServiceProvider()!;
            }
            else
            {
                serviceProvider = this.GetServiceProvider()!;
            }

            Client = new DiscordRpcClient("", autoEvents: false);
            Client.Logger = new MicrosoftLogger(serviceProvider.GetRequiredService<ILogger<DiscordRpcClient>>()) { Level = DiscordRPC.Logging.LogLevel.Warning };
            Client.Initialize();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            if (!OnBeforeInitialModuleScreenSetAsRootWasCalled)
            {
                OnBeforeInitialModuleScreenSetAsRootWasCalled = true;

                if (DelayedServiceCreation)
                {
                    Client!.Logger = new MicrosoftLogger(this.GetServiceProvider().GetRequiredService<ILogger<DiscordRpcClient>>()) { Level = DiscordRPC.Logging.LogLevel.Warning };
                }
            }
        }

        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            base.InitializeGameStarter(game, starterObject);

            if (starterObject is CampaignGameStarter campaignGameStarter)
            {
                campaignGameStarter.AddBehavior(new TrackingCampaignBehavior(SetPresence, SetPreviousPresence));
            }
        }

        protected override void OnApplicationTick(float dt)
        {
            base.OnApplicationTick(dt);

            if (Client is not { IsInitialized: true }) return;

            Client.Invoke();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

            Client?.Dispose();
        }

        private RichPresence? _currentPresence;
        private RichPresence? _previousPresence;
        internal void SetPresence(RichPresence presence)
        {
            _previousPresence = _currentPresence;
            _currentPresence = presence;

            Client?.SetPresence(_currentPresence);
        }
        internal void SetPreviousPresence()
        {
            if (_previousPresence is not null)
                SetPresence(_previousPresence);
        }
    }
}