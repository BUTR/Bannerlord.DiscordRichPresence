using DiscordRPC;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;

namespace Bannerlord.DiscordRichPresence
{
    internal static class PresenceStates
    {
        public static RichPresence Loading()
        {
            var detailsString = Strings.Loading.ToString();
            return new()
            {
                //State = Strings.Singleplayer.ToString(),
                Details = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString
                }
            };
        }

        public static RichPresence InMainMenu()
        {
            var detailsString = Strings.InMainMenu.ToString();
            return new()
            {
                Details = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                }
            };
        }

        public static RichPresence InCustomBattle()
        {
            var detailsString = Strings.InCustomBattle.ToString();
            return new()
            {
                Details = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                }
            };
        }

        public static RichPresence InCampaign(Hero hero)
        {
            var details = Strings.InCampaign.CopyTextObject();
            details.SetCharacterProperties("HERO", hero.CharacterObject);
            var detailsString = details.ToString();
            return new()
            {
                Details = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                    //SmallImageKey = AssetKeys.Bannerlord,
                    //SmallImageText =
                }
            };
        }

        public static RichPresence InMenu()
        {
            var detailsString = Strings.InMenu.ToString();
            return new()
            {
                Details = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                }
            };
        }
    }
}