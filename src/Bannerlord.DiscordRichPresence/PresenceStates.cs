using DiscordRPC;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Localization;

namespace Bannerlord.DiscordRichPresence
{
    internal static class PresenceStates
    {
        public static string? ModListUrl => DiscordSubModule.Instance.ModListUrl;

        public static RichPresence Loading()
        {
            var detailsString = Strings.Loading.ToString();
            return new()
            {
                //State = Strings.Sing leplayer.ToString(),
                Details = "Singleplayer",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence InMainMenu()
        {
            var detailsString = Strings.InMainMenu.ToString();
            return new()
            {
                Details = "Singleplayer",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence InCustomBattle()
        {
            var detailsString = Strings.InCustomBattle.ToString();
            return new()
            {
                Details = "Singleplayer: Custom Battle",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence InMenu()
        {
            var detailsString = Strings.InMenu.ToString();
            return new()
            {
                Details = "Singleplayer: Bannerlord Campaign",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignTravelling(Hero hero, Settlement neareSettlement)
        {
            var details = Strings.Travelling.CopyTextObject();
            details.SetCharacterProperties("HERO", hero.CharacterObject);
            details.SetTextVariable("SETTLEMENT", neareSettlement.Name);
            var detailsString = details.ToString();
            return new()
            {
                Details = "Singleplayer: Bannerlord Campaign",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                    //SmallImageKey = AssetKeys.Bannerlord,
                    //SmallImageText =
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignInSettlement(Settlement settlement)
        {
            var detailsString = new TextObject("In {SETTLEMENT}")
                .SetTextVariable("SETTLEMENT", settlement.Name).ToString();
            return new()
            {
                Details = "Singleplayer: Bannerlord Campaign",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignInSettlementMission(Settlement settlement, Location location)
        {
            var detailsString = new TextObject("In {LOCATION} at {SETTLEMENT}")
                .SetTextVariable("LOCATION", location.Name)
                .SetTextVariable("SETTLEMENT", settlement.Name).ToString();
            return new()
            {
                Details = "Singleplayer: Bannerlord Campaign",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignAttacking(MapEvent mapEvent)
        {
            var detailsString = new TextObject("Attacking {DEFENDERPARTY} as {ATTACKERPARTY}")
                .SetTextVariable("ATTACKERPARTY", mapEvent.AttackerSide.LeaderParty.Name)
                .SetTextVariable("DEFENDERPARTY", mapEvent.DefenderSide.LeaderParty.Name).ToString();
            return new()
            {
                Details = "Singleplayer: Bannerlord Campaign",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignDefending(MapEvent mapEvent)
        {
            var detailsString = new TextObject("Defending from {ATTACKERPARTY} as {DEFENDERPARTY}")
                .SetTextVariable("DEFENDERPARTY", mapEvent.DefenderSide.LeaderParty.Name)
                .SetTextVariable("ATTACKERPARTY", mapEvent.AttackerSide.LeaderParty.Name).ToString();
            return new()
            {
                Details = "Singleplayer: Bannerlord Campaign",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignConversation(Hero mainHero, PartyBase conversationParty)
        {
            var details = new TextObject("Talking with {PARTY} as {HERO.NAME}");
            details.SetTextVariable("PARTY", conversationParty.Name);
            details.SetCharacterProperties("HERO", mainHero.CharacterObject);

            var detailsString = details.ToString();
            return new()
            {
                Details = "Singleplayer: Bannerlord Campaign",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignCustom(TextObject details)
        {
            var detailsString = details.ToString();
            return new()
            {
                Details = "Singleplayer: Bannerlord Campaign",
                State = detailsString,
                Timestamps = Timestamps.Now,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = "Get Mod List", Url = ModListUrl } } : null,

            };
        }
    }
}