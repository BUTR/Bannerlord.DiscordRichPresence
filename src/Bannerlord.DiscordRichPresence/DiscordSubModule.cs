using Bannerlord.BUTR.Shared.Helpers;

using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.DiscordRichPresence.CampaignBehaviors;
using Bannerlord.DiscordRichPresence.MissionBehaviors;
using Bannerlord.DiscordRichPresence.Utils;

using DiscordRPC;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.DiscordRichPresence
{
    internal sealed class DiscordSubModule : MBSubModuleBase
    {
        private string? _modListUrl;
        public string? ModListUrl
        {
            get => _modListUrl;
            set
            {
                if (_modListUrl != value)
                {
                    _modListUrl = value;

                    if (_currentPresence is { } presence)
                    {
                        presence.Buttons = _modListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = _modListUrl } } : null;
                        SetPresence(_currentPresence, false);
                    }
                }
            }
        }

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

            Client = new DiscordRpcClient("1045811849314172949", autoEvents: false);
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

                if (Settings.Instance is { } settings)
                {
                    settings.PropertyChanged += InstanceOnPropertyChanged;
                    InstanceOnPropertyChanged(settings, new PropertyChangedEventArgs("LOADING_COMPLETE"));
                }
            }
        }

        private async void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is not Settings settings)
                return;

            ModListUrl = settings.ShareModList
                ? await UploadModListAsync()
                : null;
        }

        private async Task<string?> UploadModListAsync()
        {
            try
            {
                var assembly = typeof(DiscordSubModule).Assembly;
                var uploadUrlAttr = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(a => a.Key == "ModListUploadUrl");
                if (uploadUrlAttr is null)
                    return null;

                var json = JsonConvert.SerializeObject(new
                {
                    Version = ApplicationVersionHelper.GameVersion().ToString(),
                    Modules = ModuleInfoHelper.GetLoadedModules().Select(x => new
                    {
                        Id = x.Id,
                        Version = x.Version.ToString(),
                        Name = x.Name,
                        Url = x.Url
                    }).ToArray()
                });
                var data = Encoding.UTF8.GetBytes(json);

                var httpWebRequest = WebRequest.CreateHttp(uploadUrlAttr.Value);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.ContentLength = data.Length;
                httpWebRequest.UserAgent = $"Bannerlord.DiscordRichPresence v{assembly.GetName().Version}";

                using var requestStream = await httpWebRequest.GetRequestStreamAsync().ConfigureAwait(false);
                await requestStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);

                using var response = await httpWebRequest.GetResponseAsync().ConfigureAwait(false);
                if (response is not HttpWebResponse httpWebResponse)
                    return null;

                if (httpWebResponse.StatusCode is not HttpStatusCode.OK or HttpStatusCode.Created)
                    return null;

                using var stream = response.GetResponseStream();
                if (stream is null)
                    return null;

                using var responseReader = new StreamReader(stream);
                return await responseReader.ReadLineAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            base.InitializeGameStarter(game, starterObject);

            if (starterObject is CampaignGameStarter campaignGameStarter)
            {
                campaignGameStarter.AddBehavior(new CampaignEventsEx());
                campaignGameStarter.AddBehavior(new TrackingCampaignBehavior(SetPresence, SetPreviousPresence));
            }
        }

        /// <inheritdoc />
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            mission.AddMissionBehavior(new TrackingMissionBehavior(SetPresence, SetPreviousPresence));
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
        internal void SetPresence(RichPresence presence, bool saveInHistory)
        {
            if (saveInHistory)
                _previousPresence = _currentPresence;

            _currentPresence = presence;

            Client?.SetPresence(_currentPresence);
        }
        internal void SetPreviousPresence(bool saveInHistory)
        {
            if (_previousPresence is not null)
                SetPresence(_previousPresence, saveInHistory);
        }
    }
}