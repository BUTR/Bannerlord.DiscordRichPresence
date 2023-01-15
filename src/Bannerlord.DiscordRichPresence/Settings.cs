using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

using TaleWorlds.Localization;

namespace Bannerlord.DiscordRichPresence
{
    internal sealed class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "DiscordRichPresence_v1";
        public override string FolderName => "DiscordRichPresence";
        public override string FormatType => "json2";
        public override string DisplayName => new TextObject("{=1fQo3dBh}Discord Rich Presence {VERSION}", new()
        {
            { "VERSION", typeof(Settings).Assembly.GetName().Version?.ToString(3) ?? "ERROR" }
        }).ToString();

        [SettingPropertyBool("{=Id6YRNIF}Show Elapsed Time", HintText = "{=GPx0FHT7}Shows how much time has elapsed since teh state changed.", RequireRestart = false)]
        public bool ShowElapsedTime { get; set; } = false;

        [SettingPropertyBool("{=2HU8nftA}Share Mod List", HintText = "{=jceurQK9}Add a Button that will contain a link to the Mod List.", RequireRestart = false)]
        public bool ShareModList { get; set; } = false;
    }
}
