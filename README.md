# Bannerlord.DiscordRichPresence

Other mods can use this module to set their own presence:  
Note that you can use this code even if `Bannerlord.DiscordRichPresence` is not enabled by user, the actions will be skipped.
### ButterLib's DynamicAPI
```csharp
internal static class DiscordRichPresenceAPI
{
    private static readonly Action<string, DateTime?, DateTime?>? SetPresenceMethod =
        DynamicAPIProvider.RequestAPIMethod<Action<string, DateTime?, DateTime?>>("Bannerlord.DiscordRichPresence", "SetPresence");
    private static readonly Action? SetPreviousPresenceMethod =
        DynamicAPIProvider.RequestAPIMethod<Action>("Bannerlord.DiscordRichPresence", "SetPreviousPresence");

    public static void SetPresence(string details, DateTime? start, DateTime? end)
    {
        if (SetPresenceMethod != null)
        {
            SetPresenceMethod(details, start, end);
        }
    }
    public static void SetPreviousPresence()
    {
        if (SetPreviousPresenceMethod != null)
        {
            SetPreviousPresence();
        }
    }
}
```
### Reflection and Harmony
```csharp
internal static class DiscordRichPresenceAPI
{
    private static readonly Action<string, DateTime?, DateTime?>? SetPresenceMethod =
        AccessTools.Method("Bannerlord.DiscordRichPresence:SetPresence") is { } method
            ? AccessTools.MethodDelegate<Action<string, DateTime?, DateTime?>>(method)
            : null;
    private static readonly Action? SetPreviousPresenceMethod =
        AccessTools.Method("Bannerlord.DiscordRichPresence:SetPreviousPresence") is { } method
            ? AccessTools.MethodDelegate<Action>(method)
            : null;

    public static void SetPresence(string details, DateTime? start, DateTime? end)
    {
        if (SetPresenceMethod != null)
        {
            SetPresenceMethod(details, start, end);
        }
    }
    public static void SetPreviousPresence()
    {
        if (SetPreviousPresenceMethod != null)
        {
            SetPreviousPresence();
        }
    }
}
```