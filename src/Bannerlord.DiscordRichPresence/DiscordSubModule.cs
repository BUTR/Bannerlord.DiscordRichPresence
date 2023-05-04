using Bannerlord.BUTR.Shared.Helpers;

using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.DiscordRichPresence.Utils;
using Bannerlord.ModuleManager;

using DiscordRPC;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.FluentBuilder;
using MCM.Common;

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

using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.DiscordRichPresence
{
    internal sealed class DiscordSubModule : MBSubModuleBase
    {
        private static readonly int DefaultChangeSet = typeof(TaleWorlds.Library.ApplicationVersion).GetField("DefaultChangeSet")?.GetValue(null) as int? ?? 0;

        private static string ToString(ApplicationVersion version)
        {
            var str = version.ToString();
            if (version.ChangeSet == DefaultChangeSet)
            {
                var idx = str.LastIndexOf('.');
                return str.Substring(0, idx);
            }
            return str;
        }

        internal static DiscordSubModule Instance { get; private set; } = default!;

        internal static bool ShowElapsedTime { get; private set; }
        internal static bool ShareModList { get; private set; }


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
        
        private string? _avatarUrl;
        public string? AvatarUrl
        {
            get => _avatarUrl;
            set
            {
                if (_avatarUrl != value)
                {
                    _avatarUrl = $"mp:{value}";

                    if (_currentPresence is { } presence)
                    {
                        presence.Assets.LargeImageKey = _avatarUrl;
                        SetPresence(_currentPresence, false);
                    }
                }
            }
        }

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

                var settingsBuilder = BaseSettingsBuilder.Create("DiscordRichPresence_v1", new TextObject("{=1fQo3dBh}Discord Rich Presence {VERSION}", new() { { "VERSION", typeof(DiscordSubModule).Assembly.GetName().Version?.ToString(3) ?? "ERROR" } }).ToString())!
                    .SetFormat("json2")
                    .SetFolderName("DiscordRichPresence")
                    .CreateGroup("Default", group => group
                        .AddBool("show_elapsed_time", "{=Id6YRNIF}Show Elapsed Time", new PropertyRef(SymbolExtensions2.GetPropertyInfo(() => ShowElapsedTime), null), prop => prop
                            .SetHintText("{=GPx0FHT7}Shows how much time has elapsed since teh state changed.")
                            .SetRequireRestart(false))
                        .AddBool("share_mod_list", "{=2HU8nftA}Share Mod List", new PropertyRef(SymbolExtensions2.GetPropertyInfo(() => ShareModList), null), prop => prop
                            .SetHintText("{=jceurQK9}Add a Button that will contain a link to the Mod List.")
                            .SetRequireRestart(false)));
                var settings = settingsBuilder.BuildAsGlobal();
                settings.Register();
                settings.PropertyChanged += InstanceOnPropertyChanged;
                InstanceOnPropertyChanged(settings, new PropertyChangedEventArgs("LOADING_COMPLETE"));
            }
        }

        private async void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ModListUrl = ShareModList
                ? await UploadModListAsync()
                : null;
        }

        private static async Task<string?> UploadModListAsync()
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
                        Version = ToString(x.Version),
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