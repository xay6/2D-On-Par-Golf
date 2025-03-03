using UnityEditor;

namespace Unity.Multiplayer.Playmode.Workflow.Editor
{
    static class MultiplayerPlayModeSettings
    {
        const string k_ShowLaunchScreenOnPlayersKey = nameof(k_ShowLaunchScreenOnPlayersKey);

        public static void SetIsMppmActive(bool value)
        {
            var dataStore = SystemDataStore.GetMain();
            dataStore.UpdateIsMppmActive(value);
            VirtualProjectWorkflow.UpdateMPPMRuntimeState(value);
        }

        public static bool GetIsMppmActive()
        {
            var dataStore = SystemDataStore.GetMain();
            return dataStore.GetIsMppmActive();
        }

        public static bool ShowLaunchScreenOnPlayers
        {
            get => EditorPrefs.GetBool(k_ShowLaunchScreenOnPlayersKey, false);
            set => EditorPrefs.SetBool(k_ShowLaunchScreenOnPlayersKey, value);
        }
        public static bool MutePlayers
        {
            get => SystemDataStore.GetMain().GetMutePlayers();
            set => SystemDataStore.GetMain().UpdateMutePlayers(value);
        }
        public static int AssetDatabaseRefreshTimeout // @norename we use the name of this member as the editor pref key name
        {
            get => EditorPrefs.GetInt(nameof(AssetDatabaseRefreshTimeout), 30);
            set => EditorPrefs.SetInt(nameof(AssetDatabaseRefreshTimeout), value);
        }
    }
}
