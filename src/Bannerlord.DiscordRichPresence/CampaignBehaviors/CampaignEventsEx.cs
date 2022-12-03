using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;

namespace Bannerlord.DiscordRichPresence.CampaignBehaviors
{
    internal class CampaignEventsEx : CampaignBehaviorBase
    {
        private static void StartBattleSimulationPostfix(BattleSimulation ____battleSimulation)
        {
            if (Campaign.Current is { } campaign && campaign.GetCampaignBehavior<CampaignEventsEx>() is { } campaignEventsEx)
                campaignEventsEx._battleSimulationStarted.Invoke(____battleSimulation);
        }

        private static void EndBattleSimulationPrefix(BattleSimulation ____battleSimulation)
        {
            if (Campaign.Current is { } campaign && campaign.GetCampaignBehavior<CampaignEventsEx>() is { } campaignEventsEx)
                campaignEventsEx._battleSimulationEnding.Invoke(____battleSimulation);
        }

        private readonly Harmony _harmony = new("Bannerlord.DiscordRichPresence.CampaignEventsEx");

        private readonly MbEvent<BattleSimulation> _battleSimulationStarted = new();
        public IMbEvent<BattleSimulation> BattleSimulationStarted => _battleSimulationStarted;

        private readonly MbEvent<BattleSimulation> _battleSimulationEnding = new();
        public IMbEvent<BattleSimulation> BattleSimulationEnding => _battleSimulationEnding;

        public CampaignEventsEx()
        {
            _harmony.Patch(
                original: AccessTools2.Method(typeof(MapState), "StartBattleSimulation"),
                postfix: new HarmonyMethod(typeof(CampaignEventsEx), nameof(StartBattleSimulationPostfix)));
            _harmony.Patch(
                original: AccessTools2.Method(typeof(MapState), "EndBattleSimulation"),
                prefix: new HarmonyMethod(typeof(CampaignEventsEx), nameof(EndBattleSimulationPrefix)));
        }

        public override void SyncData(IDataStore dataStore) { }

        public override void RegisterEvents()
        {
            CampaignEvents.OnGameOverEvent.AddNonSerializedListener(this, Dispose);
        }

        public void Dispose()
        {
            _harmony.UnpatchAll(_harmony.Id);
        }
    }
}