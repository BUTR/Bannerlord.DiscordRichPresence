using TaleWorlds.Localization;

namespace Bannerlord.DiscordRichPresence
{
    internal static class Strings
    {
        public static readonly TextObject GetModList = new("{=meYs0E4y}Get Mod List");


        public static readonly TextObject Singleplayer = new("{=JbpAgMpG}Singleplayer");
        public static readonly TextObject SingleplayerCustomBattle = new("{=a7NGHGQ6}Singleplayer: Custom Battle");
        public static readonly TextObject SingleplayerBannerlordCampaign = new("{=e7hmqot8}Singleplayer: Bannerlord Campaign");


        public static readonly TextObject Loading = new("{=5Fd4qhYA}Loading...");
        public static readonly TextObject InMainMenu = new("{=aZboLkmT}In Main Menu");
        public static readonly TextObject InCustomBattle = new("{=UrcQjlQ9}In a Custom Battle");
        public static readonly TextObject InMenu = new("{=AW9YgXO3}In a Menu");
        public static readonly TextObject Traveling = new("{=EN0dNwKz}Traveling as {HERO.NAME} near {SETTLEMENT}");


        public static readonly TextObject CampaignInSettlement = new("{=BcvAuXsH}In {SETTLEMENT}");

        public static readonly TextObject CampaignInSettlementMission = new("{=YjyPeo1g}In {LOCATION} at {SETTLEMENT}");

        public static readonly TextObject CampaignIsSimulation = new("{=O4x837Ak}(Simulation) ");
        public static readonly TextObject CampaignAttackingGeneral = new("{=vbS40wdE}{ISSIMULATION}Attacking {DEFENDER.NAME} ({DEFENDER.COUNT}) as {ATTACKER.NAME} ({ATTACKER.COUNT})");
        public static readonly TextObject CampaignAttackingRaid = new("{=lfAU30F0}{ISSIMULATION}Raiding {DEFENDER.NAME} ({DEFENDER.COUNT}) as {ATTACKER.NAME} ({ATTACKER.COUNT})");
        public static readonly TextObject CampaignAttackingForcingVolunteers = new("{=houvlYqH}{ISSIMULATION}Forcing volunteers {DEFENDER.NAME} ({DEFENDER.COUNT}) as {ATTACKER.NAME} ({ATTACKER.COUNT})");
        public static readonly TextObject CampaignAttackingForcingSupplies = new("{=Bmb5QRHX}{ISSIMULATION}Forcing supplies {DEFENDER.NAME} ({DEFENDER.COUNT}) as {ATTACKER.NAME} ({ATTACKER.COUNT})");
        public static readonly TextObject CampaignAttackingSieging = new("{=uAVJBLHS}{ISSIMULATION}Sieging {DEFENDER.NAME} ({DEFENDER.COUNT}) as {ATTACKER.NAME} ({ATTACKER.COUNT})");

        public static readonly TextObject CampaignDefendingGeneral = new("{=f7hYjU4D}{ISSIMULATION}Defending against {ATTACKER.NAME} ({ATTACKER.COUNT}) as {DEFENDER.NAME} ({DEFENDER.COUNT})");
        public static readonly TextObject CampaignDefendingRaid = new("{=5TwT4kNC}{ISSIMULATION}Defending against a raid by {ATTACKER.NAME} ({ATTACKER.COUNT}) as {DEFENDER.NAME} ({DEFENDER.COUNT})");
        public static readonly TextObject CampaignDefendingForcingVolunteers = new("{=GHfQXd9t}{ISSIMULATION}Defending against forcing volunteers by {ATTACKER.NAME} ({ATTACKER.COUNT}) as {DEFENDER.NAME} ({DEFENDER.COUNT})");
        public static readonly TextObject CampaignDefendingForcingSupplies = new("{=fJdWDciU}{ISSIMULATION}Defending against forcing supplies by {ATTACKER.NAME} ({ATTACKER.COUNT}) as {DEFENDER.NAME} ({DEFENDER.COUNT})");
        public static readonly TextObject CampaignDefendingSiege = new("{=FMVm4Ipd}{ISSIMULATION}Defending against a siege by {ATTACKER.NAME} ({ATTACKER.COUNT}) as {DEFENDER.NAME} ({DEFENDER.COUNT})");

        public static readonly TextObject CampaignConversation = new("{=v2cwuO3J}Talking with {PARTY} as {HERO.NAME}");
    }
}