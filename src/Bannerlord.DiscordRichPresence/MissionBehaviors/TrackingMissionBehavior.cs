using DiscordRPC;

using System;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.DiscordRichPresence.MissionBehaviors
{
    internal sealed class TrackingMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        private readonly Action<RichPresence, bool> _setPresence;
        private readonly Action<bool> _setPreviousPresence;

        public TrackingMissionBehavior(Action<RichPresence, bool> setPresence, Action<bool> setPreviousPresence)
        {
            _setPresence = setPresence;
            _setPreviousPresence = setPreviousPresence;
        }
    }
}