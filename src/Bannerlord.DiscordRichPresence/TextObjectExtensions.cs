using Helpers;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Localization;

namespace Bannerlord.DiscordRichPresence
{
    public static class Extensions
    {
        public static TextObject SetCharacterProperties(this TextObject to, string tag, CharacterObject character, bool includeDetails = false)
        {
            StringHelpers.SetCharacterProperties(tag, character, to, includeDetails);
            return to;
        }

        public static TextObject SetMapEventSideProperties(this TextObject to, string tag, MapEventSide mapEventSide)
        {
            var characterProperties = GetMapEventSideProperties(mapEventSide);
            to.SetTextVariable(tag, characterProperties);
            return to;
        }

        private static TextObject GetMapEventSideProperties(MapEventSide mapEventSide)
        {
            var textObject = new TextObject();
            textObject.SetTextVariable("NAME", mapEventSide.LeaderParty.Name);
            textObject.SetTextVariable("COUNT", mapEventSide.TroopCount);
            return textObject;
        }
    }
}