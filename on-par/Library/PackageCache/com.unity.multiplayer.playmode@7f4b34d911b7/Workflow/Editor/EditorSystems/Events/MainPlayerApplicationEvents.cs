using System;
using Unity.Multiplayer.Playmode.VirtualProjects.Editor;
using UnityEngine;

namespace Unity.Multiplayer.Playmode.Workflow.Editor
{
    class MainPlayerApplicationEvents
    {
        public event Action<PlayerIdentifier> PlayerCommunicative;
        public event Action<string, PlayerIdentifier> SceneChanged;
        public event Action<PlayerIdentifier, LogCounts> LogCountsChanged;
        public event Action UIPollUpdate;
        public event Action PausedOnPlayer;
        public event Action<VirtualProjectIdentifier, string, string, LogType> LogMessageReceived;

        internal void InvokeEditorCommunicative(PlayerIdentifier identifier)
        {
            PlayerCommunicative?.Invoke(identifier);
        }

        internal void InvokeSceneChanged(string scenePath, PlayerIdentifier identifier)
        {
            SceneChanged?.Invoke(scenePath, identifier);
        }

        internal void InvokeLogCountsChanged(PlayerIdentifier playerIdentifier, LogCounts logCounts)
        {
            LogCountsChanged?.Invoke(playerIdentifier, logCounts);
        }

        internal void InvokeUIPollUpdate()
        {
            UIPollUpdate?.Invoke();
        }

        internal void InvokePausedOnPlayer()
        {
            PausedOnPlayer?.Invoke();
        }

        internal void InvokeLogMessageReceived(VirtualProjectIdentifier identifier, string message, string stackTrace, LogType type)
        {
            LogMessageReceived?.Invoke(identifier, message, stackTrace, type);
        }
    }
}
