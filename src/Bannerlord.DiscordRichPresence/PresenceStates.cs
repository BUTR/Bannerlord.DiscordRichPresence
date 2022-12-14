using DiscordRPC;

using TaleWorlds.CampaignSystem;
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
                Details = Strings.Singleplayer.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence InMainMenu()
        {
            var detailsString = Strings.InMainMenu.ToString();

            return new()
            {
                Details = Strings.Singleplayer.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence InCustomBattle()
        {
            var detailsString = Strings.InCustomBattle.ToString();

            return new()
            {
                Details = Strings.SingleplayerCustomBattle.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence InMenu()
        {
            var detailsString = Strings.InMenu.ToString();

            return new()
            {
                Details = Strings.SingleplayerBannerlordCampaign.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignTravelling(Hero hero, Settlement neareSettlement)
        {
            var detailsString = Strings.Traveling.CopyTextObject()
                .SetCharacterProperties("HERO", hero.CharacterObject)
                .SetTextVariable("SETTLEMENT", neareSettlement.Name).ToString();

            return new()
            {
                Details = Strings.SingleplayerBannerlordCampaign.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignInSettlement(Settlement settlement)
        {
            var detailsString = Strings.CampaignInSettlement.CopyTextObject()
                .SetTextVariable("SETTLEMENT", settlement.Name).ToString();

            return new()
            {
                Details = Strings.SingleplayerBannerlordCampaign.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignInSettlementMission(Settlement settlement, Location location)
        {
            var detailsString = Strings.CampaignInSettlementMission.CopyTextObject()
                .SetTextVariable("LOCATION", location.Name)
                .SetTextVariable("SETTLEMENT", settlement.Name).ToString();

            return new()
            {
                Details = Strings.SingleplayerBannerlordCampaign.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignAttacking(MapEvent mapEvent, bool isSimulation)
        {
            // TODO: Signal about it being a player simulation
            var detailsString = (mapEvent.EventType switch
            {
                MapEvent.BattleTypes.FieldBattle or MapEvent.BattleTypes.Hideout or MapEvent.BattleTypes.AlleyFight => Strings.CampaignAttackingGeneral.CopyTextObject(),
                MapEvent.BattleTypes.Raid => Strings.CampaignAttackingRaid.CopyTextObject(),
                MapEvent.BattleTypes.IsForcingVolunteers => Strings.CampaignAttackingForcingVolunteers.CopyTextObject(),
                MapEvent.BattleTypes.IsForcingSupplies => Strings.CampaignAttackingForcingSupplies.CopyTextObject(),
                MapEvent.BattleTypes.Siege or MapEvent.BattleTypes.SiegeOutside or MapEvent.BattleTypes.SallyOut => Strings.CampaignAttackingSieging.CopyTextObject(),
                _ => new TextObject("ERROR")
            })
                .SetConditional(() => isSimulation, "ISSIMULATION", Strings.CampaignIsSimulation)
                .SetMapEventSideProperties("ATTACKER", mapEvent.AttackerSide)
                .SetMapEventSideProperties("DEFENDER", mapEvent.DefenderSide).ToString();

            return new()
            {
                Details = Strings.SingleplayerBannerlordCampaign.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignDefending(MapEvent mapEvent, bool isSimulation)
        {
            // TODO: Signal about it being a player simulation
            var detailsString = (mapEvent.EventType switch
            {
                MapEvent.BattleTypes.FieldBattle or MapEvent.BattleTypes.Hideout or MapEvent.BattleTypes.AlleyFight => Strings.CampaignDefendingGeneral.CopyTextObject(),
                MapEvent.BattleTypes.Raid => Strings.CampaignDefendingRaid.CopyTextObject(),
                MapEvent.BattleTypes.IsForcingVolunteers => Strings.CampaignDefendingForcingVolunteers.CopyTextObject(),
                MapEvent.BattleTypes.IsForcingSupplies => Strings.CampaignDefendingForcingSupplies.CopyTextObject(),
                MapEvent.BattleTypes.Siege or MapEvent.BattleTypes.SiegeOutside or MapEvent.BattleTypes.SallyOut => Strings.CampaignDefendingSiege.CopyTextObject(),
                _ => new TextObject("ERROR")
            })
                .SetConditional(() => isSimulation, "ISSIMULATION", Strings.CampaignIsSimulation)
                .SetMapEventSideProperties("DEFENDER", mapEvent.DefenderSide)
                .SetMapEventSideProperties("ATTACKER", mapEvent.AttackerSide)
                .ToString();

            return new()
            {
                Details = Strings.SingleplayerBannerlordCampaign.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignConversation(Hero mainHero, PartyBase conversationParty)
        {
            var details = Strings.CampaignConversation.CopyTextObject()
                .SetTextVariable("PARTY", conversationParty.Name)
                .SetCharacterProperties("HERO", mainHero.CharacterObject);

            var detailsString = details.ToString();
            return new()
            {
                Details = Strings.SingleplayerBannerlordCampaign.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }

        public static RichPresence CampaignCustom(TextObject details)
        {
            var detailsString = details.ToString();

            return new()
            {
                Details = Strings.SingleplayerBannerlordCampaign.ToString(),
                State = detailsString,
                Timestamps = Settings.Instance?.ShowElapsedTime == true ? Timestamps.Now : null,
                Assets = new Assets
                {
                    LargeImageKey = AssetKeys.Bannerlord,
                    LargeImageText = detailsString,
                },
                Buttons = ModListUrl is not null ? new[] { new Button { Label = Strings.GetModList.ToString(), Url = ModListUrl } } : null,
            };
        }
    }
}