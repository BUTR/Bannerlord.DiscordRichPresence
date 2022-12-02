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
        public override string DisplayName => new TextObject("{=h8I4j7H2YU}Discord Rich Presence {VERSION}", new()
        {
            { "VERSION", typeof(Settings).Assembly.GetName().Version?.ToString(3) ?? "ERROR" }
        }).ToString();

        [SettingPropertyBool("Share Mod List", HintText = "Add a Button that will contain a link to the Mod List", RequireRestart = false)]
        public bool ShareModList { get; set; } = false;
    }
}