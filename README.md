# Bannerlord.DiscordRichPresence
<p align="center">
  <a href="https://github.com/BUTR/Bannerlord.DiscordRichPresence" alt="Lines Of Code">
    <img src="https://aschey.tech/tokei/github/BUTR/Bannerlord.DiscordRichPresence?category=code" />
  </a>
  <a href="https://www.codefactor.io/repository/github/butr/bannerlord.discordrichpresence">
    <img src="https://www.codefactor.io/repository/github/butr/bannerlord.discordrichpresence/badge" alt="CodeFactor" />
  </a>
  <a href="https://codeclimate.com/github/BUTR/Bannerlord.DiscordRichPresence/maintainability">
    <img alt="Code Climate maintainability" src="https://img.shields.io/codeclimate/maintainability-percentage/BUTR/Bannerlord.DiscordRichPresence">
  </a>
  <a title="Crowdin" target="_blank" href="https://crowdin.com/project/discord-rich-presence">
    <img src="https://badges.crowdin.net/discord-rich-presence/localized.svg">
  </a>
  </br>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/4836" alt="NexusMods DiscordRichPresence">
    <img src="https://img.shields.io/badge/NexusMods-DiscordRichPresence-yellow.svg" />
  </a>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/4836" alt="NexusMods DiscordRichPresence">
    <img src="https://img.shields.io/endpoint?url=https%3A%2F%2Fnexusmods-version-pzk4e0ejol6j.runkit.sh%3FgameId%3Dmountandblade2bannerlord%26modId%3D4836" />
  </a>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/4836" alt="NexusMods DiscordRichPresence">
    <img src="https://img.shields.io/endpoint?url=https%3A%2F%2Fnexusmods-downloads-ayuqql60xfxb.runkit.sh%2F%3Ftype%3Dunique%26gameId%3D3174%26modId%3D4836" />
  </a>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/4836" alt="NexusMods DiscordRichPresence">
    <img src="https://img.shields.io/endpoint?url=https%3A%2F%2Fnexusmods-downloads-ayuqql60xfxb.runkit.sh%2F%3Ftype%3Dtotal%26gameId%3D3174%26modId%3D4836" />
  </a>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/4836" alt="NexusMods DiscordRichPresence">
    <img src="https://img.shields.io/endpoint?url=https%3A%2F%2Fnexusmods-downloads-ayuqql60xfxb.runkit.sh%2F%3Ftype%3Dviews%26gameId%3D3174%26modId%3D4836" />
  </a>
  </br>
  <a href="https://steamcommunity.com/sharedfiles/filedetails/?id=2897891539">
    <img alt="Steam Yell To Inspire" src="https://img.shields.io/badge/Steam-Discord%20Rich%20Presence-blue.svg" />
  </a>
  <a href="https://steamcommunity.com/sharedfiles/filedetails/?id=2897891539">
    <img alt="Steam Downloads" src="https://img.shields.io/steam/downloads/2897891539?label=Downloads&color=blue">
  </a>
  <a href="https://steamcommunity.com/sharedfiles/filedetails/?id=2897891539">
    <img alt="Steam Views" src="https://img.shields.io/steam/views/2897891539?label=Views&color=blue">
  </a>
  <a href="https://steamcommunity.com/sharedfiles/filedetails/?id=2897891539">
    <img alt="Steam Subscriptions" src="https://img.shields.io/steam/subscriptions/2897891539?label=Subscriptions&color=blue">
  </a>
  <a href="https://steamcommunity.com/sharedfiles/filedetails/?id=2897891539">
    <img alt="Steam Favorites" src="https://img.shields.io/steam/favorites/2897891539?label=Favorites&color=blue">
  </a>
  </br>
  <img src="https://staticdelivery.nexusmods.com/mods/3174/images/4836/4836-1670247182-153716606.png" width="300">
</p>

Adds Discord Rich Presence support for the game! Inspired by one of the first mods on NexusMods, [Discord RP for Bannerlord](https://github.com/TheDoctorOne/Discord-Rich-Presence-for-Bannerlord)! 

Full translation support as expected from BUTR mods!
Settings are available via MCM!

You can also enable mod sharing in Options that will add a fancy button on the bottom of your profile, opening it will give you an URL in format `https://modlist.butr.link/d29ee27c-5f08-4a53-b4ae-231a76d5e404` that will contain your game version and the list of enabled mods! The url will be available for 8 hour since the start of your game!
Some commentary about this feature. At the start of the game when the option is enabled or when saving with the enabled option, the module will send to `https://modlist.butr.link` the game version and the mod list and obtain a link that lives 8 hours that will show you the info. This is done this way because Discord has a limit on the URL length and we can't put the info inside the URL via the query params, so we have a quick lived backend server that stores this info.
Because the Module is open-sourced, even if our server dies after some years, we have the [backend available](https://github.com/BUTR/BUTR.ModListServer)﻿ for anyone to host and setup the module again with the feature!

The game supports basic states:

* Main Menu
* Loading
* Custom Battles
* Campaign:
  * Traveling near a settlement
  * Conversation with someone
  * At Settlement
  * In Settlement (Mission)
  * In Battle (simulation and not) with troop count. Attacking/Defending. Raid, Siege, Forcing* are all supported

## For Modders
Other mods can use this module to set their own presence:  
Note that you can use this code even if `Bannerlord.DiscordRichPresence` is not enabled by user, the actions will be skipped.
### ButterLib's DynamicAPI
```csharp
internal static class DiscordRichPresenceAPI
{
    private static readonly Action<string, DateTime?, DateTime?, bool>? SetPresenceMethod =
        DynamicAPIProvider.RequestAPIMethod<Action<string, DateTime?, DateTime?, bool>>("Bannerlord.DiscordRichPresence", "SetPresence");
    private static readonly Action<bool>? SetPreviousPresenceMethod =
        DynamicAPIProvider.RequestAPIMethod<Action<bool>>("Bannerlord.DiscordRichPresence", "SetPreviousPresence");

    public static void SetPresence(string details, DateTime? start, DateTime? end)
    {
        if (SetPresenceMethod != null)
        {
            SetPresenceMethod(details, start, end, true);
        }
    }
    public static void SetPreviousPresence()
    {
        if (SetPreviousPresenceMethod != null)
        {
            SetPreviousPresence(true);
        }
    }
}
```
### Reflection and Harmony
```csharp
internal static class DiscordRichPresenceAPI
{
    private static readonly Action<string, DateTime?, DateTime?, bool>? SetPresenceMethod =
        AccessTools.Method("Bannerlord.DiscordRichPresence.DynamicAPI:SetPresence") is { } method
            ? AccessTools.MethodDelegate<Action<string, DateTime?, DateTime?, bool>>(method)
            : null;
    private static readonly Action<bool>? SetPreviousPresenceMethod =
        AccessTools.Method("Bannerlord.DiscordRichPresence.DynamicAPI:SetPreviousPresence") is { } method
            ? AccessTools.MethodDelegate<Action<bool>>(method)
            : null;

    public static void SetPresence(string details, DateTime? start, DateTime? end)
    {
        if (SetPresenceMethod != null)
        {
            SetPresenceMethod(details, start, end, true);
        }
    }
    public static void SetPreviousPresence()
    {
        if (SetPreviousPresenceMethod != null)
        {
            SetPreviousPresence(true);
        }
    }
}
```
