using DiscordRPC;

using SandBox.Tournaments.MissionLogics;

using System;

using TaleWorlds.MountAndBlade;

namespace Bannerlord.DiscordRichPresence.MissionBehaviors
{
    internal sealed class TrackingMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        private readonly Action<RichPresence, bool> _setPresence;
        private readonly Action<bool> _setPreviousPresence;

        private float _counter = 3f;
        private TournamentBehavior? _tournamentBehavior;

        public TrackingMissionBehavior(Action<RichPresence, bool> setPresence, Action<bool> setPreviousPresence)
        {
            _setPresence = setPresence;
            _setPreviousPresence = setPreviousPresence;
        }

        /// <inheritdoc />
        public override void OnCreated()
        {
            base.OnCreated();

            _tournamentBehavior = Mission.GetMissionBehavior<TournamentBehavior>();
        }

        /// <inheritdoc />
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            _counter += dt;
            if (_counter < 3f) return;
            _counter = 0;

            if (_tournamentBehavior is not null)
            {
                _setPresence(PresenceStates.CampaignTournament(_tournamentBehavior), true);
            }
        }
    }
}