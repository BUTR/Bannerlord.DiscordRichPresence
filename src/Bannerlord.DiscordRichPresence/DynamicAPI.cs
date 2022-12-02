using Bannerlord.ButterLib.DynamicAPI;

using DiscordRPC;

using System;
using System.Diagnostics.CodeAnalysis;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.DiscordRichPresence
{
    [DynamicAPIClass("Bannerlord.DiscordRichPresence")]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal sealed class DynamicAPI
    {
        [DynamicAPIMethod("SetPresence")]
        public static void SetPresence(string details, DateTime? start, DateTime? end, bool saveInHistory)
        {
            DiscordSubModule.Instance.SetPresence(new RichPresence
            {
                Details = details,
                Timestamps = start is null ? Timestamps.Now : end is null ? new Timestamps(start.Value) : new Timestamps(start.Value, end.Value),
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = details
                }
            }, saveInHistory);
        }

        [DynamicAPIMethod("SetPreviousPresence")]
        public static void SetPreviousPresence(bool saveInHistory) => DiscordSubModule.Instance.SetPreviousPresence(saveInHistory);


        [DynamicAPIMethod("SetPresenceLoading")]
        public static void SetPresenceLoading() => DiscordSubModule.Instance.SetPresence(PresenceStates.Loading(), true);

        [DynamicAPIMethod("SetPresenceInMainMenu")]
        public static void SetPresenceInMainMenu() => DiscordSubModule.Instance.SetPresence(PresenceStates.InMainMenu(), true);

        [DynamicAPIMethod("SetPresenceInCustomBattle")]
        public static void SetPresenceInCustomBattle() => DiscordSubModule.Instance.SetPresence(PresenceStates.InCustomBattle(), true);

        [DynamicAPIMethod("SetPresenceInMenu")]
        public static void SetPresenceInMenu() => DiscordSubModule.Instance.SetPresence(PresenceStates.InMenu(), true);
    }
}