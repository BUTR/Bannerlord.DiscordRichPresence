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
        public static void SetPresence(string details, DateTime? start, DateTime? end)
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
            });
        }

        [DynamicAPIMethod("SetPreviousPresence")]
        public static void SetPreviousPresence() => DiscordSubModule.Instance.SetPreviousPresence();


        [DynamicAPIMethod("SetPresenceLoading")]
        public static void SetPresenceLoading() => DiscordSubModule.Instance.SetPresence(PresenceStates.Loading());

        [DynamicAPIMethod("SetPresenceInMainMenu")]
        public static void SetPresenceInMainMenu() => DiscordSubModule.Instance.SetPresence(PresenceStates.InMainMenu());

        [DynamicAPIMethod("SetPresenceInCustomBattle")]
        public static void SetPresenceInCustomBattle() => DiscordSubModule.Instance.SetPresence(PresenceStates.InCustomBattle());

        [DynamicAPIMethod("SetPresenceInCampaign")]
        public static void SetPresenceInCampaign(Hero hero) => DiscordSubModule.Instance.SetPresence(PresenceStates.InCampaign(hero));

        [DynamicAPIMethod("SetPresenceInMenu")]
        public static void SetPresenceInMenu() => DiscordSubModule.Instance.SetPresence(PresenceStates.InMenu());
    }
}