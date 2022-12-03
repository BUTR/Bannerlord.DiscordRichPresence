using Bannerlord.ButterLib.DynamicAPI;

using DiscordRPC;

using System;
using System.Diagnostics.CodeAnalysis;

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
    }
}