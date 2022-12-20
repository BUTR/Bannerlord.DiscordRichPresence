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
  <img src="https://staticdelivery.nexusmods.com/mods/3174/images/4836/4836-1670247182-153716606.png" width="300">
</p>

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
