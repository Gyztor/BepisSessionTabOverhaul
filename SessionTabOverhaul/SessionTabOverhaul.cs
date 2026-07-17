using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Elements.Core;
using FrooxEngine;
using HarmonyLib;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.NET.Common;
using BepInExResoniteShim;
using BepisResoniteWrapper;

namespace SessionTabOverhaul
{
    [ResonitePlugin(PluginMetadata.GUID, PluginMetadata.NAME, PluginMetadata.VERSION, PluginMetadata.AUTHORS, PluginMetadata.REPOSITORY_URL)]
    [BepInDependency(BepInExResoniteShim.PluginMetadata.GUID)]
    public class SessionTabOverhaul : BasePlugin
    {
        /*public static ModConfiguration Config;

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ColorHostNameKey = new ModConfigurationKey<bool>("ColorHostName", "Color the Host's username like the host icon.", () => true);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ColorLocalUserNameKey = new ModConfigurationKey<bool>("ColorLocalUserName", "Color the Local Users's username?", () => true);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<colorX> LocalUserColorKey = new ModConfigurationKey<colorX>("LocalUserColor", "Color of the Local User in the Session user list.", () => RadiantUI_Constants.Hero.PURPLE);

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<colorX> FirstRowColorKey = new ModConfigurationKey<colorX>("FirstRowColor", "Background color of the first row in the Session user lists.", () => new colorX(0, .85f));
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<colorX> SecondRowColorKey = new ModConfigurationKey<colorX>("SecondRowColor", "Background color of the second row in the Session user lists.", () => new colorX(1, .15f));

        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> HideAllBadgesKey = new ModConfigurationKey<bool>("HideAllBadges", "Hide all Badges in the Session Users list.", () => false);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> HideCustomBadgesKey = new ModConfigurationKey<bool>("HideCustomBadges", "Hide Custom Badges in the Session Users list.", () => false);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> HidePatreonBadgeKey = new ModConfigurationKey<bool>("HidePatreonBadge", "Hide the Patreon Badge in the Session Users list.", () => false);
        
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ShowParentUserCheckboxKey = new ModConfigurationKey<bool>("ShowParentUserCheckbox", "Show the Parent User checkbox in the Session Users list.", () => true);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ShowBringButtonKey = new ModConfigurationKey<bool>("ShowBringButton", "Show the Bring button in the Session Users list.", () => true);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ShowDeviceLabelKey = new ModConfigurationKey<bool>("ShowDeviceLabel", "Show the Device label in the Session Users list.", () => true);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ShowFPSOrQueuedMessagesKey = new ModConfigurationKey<bool>("ShowFPSOrQueuedMessages", "Show the FPS / Queued messages in the Session Users list.", () => true);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ShowSteamButtonKey = new ModConfigurationKey<bool>("ShowSteamButton", "Show the Steam button in the Session Users list.", () => false);
        [AutoRegisterConfigKey] private static readonly ModConfigurationKey<bool> ShowVoiceModeKey = new ModConfigurationKey<bool>("ShowVoiceMode", "Show the Voice mode in the Session Users list.", () => true);

        public override string Author => "Banane9, NepuShiro";
        public override string Link => "https://github.com/NepuShiro/ResoniteSessionTabOverhaul";
        public override string Name => "SessionTabOverhaul";
        public override string Version => "2.2.0";*/

        /*internal static bool ColorHostName => Config.GetValue(ColorHostNameKey);
        internal static bool ColorLocalUserName => Config.GetValue(ColorLocalUserNameKey);
        internal static colorX LocalUserColor => Config.GetValue(LocalUserColorKey);
        internal static colorX FirstRowColor => Config.GetValue(FirstRowColorKey);
        internal static colorX SecondRowColor => Config.GetValue(SecondRowColorKey);
        internal static bool HideAllBadges => Config.GetValue(HideAllBadgesKey);
        internal static bool HideCustomBadges => Config.GetValue(HideCustomBadgesKey);
        internal static bool HidePatreonBadge => Config.GetValue(HidePatreonBadgeKey);
        internal static bool ShowParentUserCheckbox => Config.GetValue(ShowParentUserCheckboxKey);
        internal static bool ShowBringButton => Config.GetValue(ShowBringButtonKey);
        internal static bool ShowDeviceLabel => Config.GetValue(ShowDeviceLabelKey);
        internal static bool ShowFPSOrQueuedMessages => Config.GetValue(ShowFPSOrQueuedMessagesKey);
        internal static bool ShowSteamButton => Config.GetValue(ShowSteamButtonKey);
        internal static bool ShowVoiceMode => Config.GetValue(ShowVoiceModeKey);
        internal static bool SpritesInjected { get; set; }*/

        internal new static ManualLogSource Log = null!;

        internal static ConfigEntry<bool> ColorHostName;
        internal static ConfigEntry<bool> ColorLocalUserName;
        internal static ConfigEntry<bool> HideAllBadges;
        internal static ConfigEntry<bool> HideCustomBadges;
        internal static ConfigEntry<bool> HidePatreonBadge;
        internal static ConfigEntry<bool> ShowParentUserCheckbox;
        internal static ConfigEntry<bool> ShowBringButton;
        internal static ConfigEntry<bool> ShowDeviceLabel;
        internal static ConfigEntry<bool> ShowFPSOrQueuedMessages;
        internal static ConfigEntry<bool> ShowSteamButton;
        internal static ConfigEntry<bool> ShowVoiceMode;

        internal static ConfigEntry<colorX> LocalUserColor;
        internal static ConfigEntry<colorX> FirstRowColor;
        internal static ConfigEntry<colorX> SecondRowColor;

        internal static bool SpritesInjected { get; set; }

        public override void Load()
        {
            Log = base.Log;

            //Harmony harmony = new Harmony("net.NepuShiro.SessionTabOverhaul");
            ColorHostName = Config.Bind("Usernames", "Color Host Name", true, new ConfigDescription("Color the Host's username like the host icon."));
            ColorLocalUserName = Config.Bind("Usernames", "Color Local Username", true, new ConfigDescription("Colors the Local Users's username."));
            HideAllBadges = Config.Bind("Badges", "Hide All Badges", false, new ConfigDescription("Hide all Badges in the Session Users list."));
            HideCustomBadges = Config.Bind("Badges", "Hide Custom Badges", false, new ConfigDescription("Hide Custom Badges in the Session Users list."));
            HidePatreonBadge = Config.Bind("Badges", "Hide Patreon Badge", false, new ConfigDescription("Hides the Patreon badge in the Session Users list."));
            ShowParentUserCheckbox = Config.Bind("User Manipulation", "Show Parent User Checkbox", true, new ConfigDescription("Show the Parent User checkbox in the Session Users list."));
            ShowBringButton = Config.Bind("User Manipulation", "Show Bring Button", true, new ConfigDescription("Show the Bring button in the Session Users list."));
            ShowDeviceLabel = Config.Bind("User Info", "Show Device Label", true, new ConfigDescription("Show the Device label in the Session Users list"));
            ShowFPSOrQueuedMessages = Config.Bind("User Info", "Show FPS Or Queued", true, new ConfigDescription("Show the FPS / Queued messages in the Session Users list."));
            ShowSteamButton = Config.Bind("User Info", "Show Steam Button", false, new ConfigDescription("Show the Steam button in the Session Users list."));
            ShowVoiceMode = Config.Bind("User Info", "Show Voice Mode", true, new ConfigDescription("Colors the Local Users's username."));

            LocalUserColor = Config.Bind("Usernames", "Local User Color", RadiantUI_Constants.Hero.PURPLE, new ConfigDescription("The Color the Local Username should be set to."));
            FirstRowColor = Config.Bind("Row Colors", "First Row Color", new colorX(0, .85f), new ConfigDescription("Background color of the first row in the Session user lists."));
            SecondRowColor = Config.Bind("Row Colors", "Second Row Color", new colorX(1, .15f), new ConfigDescription("Background color of the second row in the Session user lists."));
            
            try {
                HarmonyInstance.PatchAll();
                Log.LogInfo("SessionTabOverhaul Loaded!");
            } catch (System.Exception ex)
            {
                Log.LogError($"SessionTabOverhaul failed to patch: {ex}");
            }
        }
    }
}