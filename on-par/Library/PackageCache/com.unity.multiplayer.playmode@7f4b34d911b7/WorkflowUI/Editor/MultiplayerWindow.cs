using System;
using System.Collections.Generic;
using Unity.Multiplayer.PlayMode.Common.Runtime;
using Unity.Multiplayer.Playmode.Workflow.Editor;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Unity.Multiplayer.PlayMode.Configurations.Editor;
#if UNITY_USE_MULTIPLAYER_ROLES
using Unity.Multiplayer.Editor;
#endif

namespace Unity.Multiplayer.Playmode.WorkflowUI.Editor
{
    class MultiplayerWindow : EditorWindow
    {
        public MainView MainView { get; private set; }
        public HelpBox DisabledHelpBox;

        void OnFocus()
        {
            MultiplayerWindowController.ShouldUpdateUI = true;
        }

        public void CreateGUI()
        {
            if (!MultiplayerWindowController.IsVirtualProjectWorkflowInitialized)
            {
                DestroyImmediate(this);
                return;
            }

            DisabledHelpBox = new HelpBox("Play mode is currently managed by Playmode Scenario. Please use the dropdown next to the play button to change scenarios, " +
                                          "and use the configuration window to modify the scenario settings." + "\n" + "\n" +
                                          "To use the original Multiplayer Play Mode, select 'Default' from the scenario dropdown. When there is an active scenario selected all currently active players will be deactivated", HelpBoxMessageType.Info);
            rootVisualElement.Add(DisabledHelpBox);



            MainView = new MainView();
            rootVisualElement.Add(MainView);
            var currentConfig = PlayModeManager.instance.ActivePlayModeConfig;
            MainView.SetEnabled(currentConfig.name == "Default");
            UnityPlayer[] players = MultiplayerPlaymode.Players;
            DisabledHelpBox.style.display = currentConfig.name == "Default" ? DisplayStyle.None : DisplayStyle.Flex;
            DisabledHelpBox.style.marginLeft = 6;
            DisabledHelpBox.style.marginRight = 16;
            DisabledHelpBox.style.marginTop = 8;
            DisabledHelpBox.style.marginBottom = 10;
            DisabledHelpBox.style.alignSelf = Align.Auto;
            DisabledHelpBox.style.height = 95;

            PlayModeManager.instance.ConfigAssetChanged += () =>
            {
                var newConfig = PlayModeManager.instance.ActivePlayModeConfig;
                MainView.SetEnabled(newConfig.name == "Default");
                DisabledHelpBox.style.display = newConfig.name == "Default" ? DisplayStyle.None : DisplayStyle.Flex;
                foreach (var player in players)
                {
                    if (player.PlayerState is PlayerState.Launched or PlayerState.Launching && newConfig.name != "Default")
                    {
                        player.Deactivate(out _);
                    }
                }
            };
            MultiplayerWindowController.ShouldStartWindow = true;
        }
    }

    static class MultiplayerWindowController
    {
        const string k_Title = "Multiplayer Play Mode";
#if UNITY_USE_MULTIPLAYER_ROLES
        internal const string RoleServerClient = "Client and Server";
        internal const string RoleServer = "Server";
        internal const string RoleClient = "Client";
#endif

        static MultiplayerWindow s_MultiplayerWindow;
        static readonly Dictionary<PlayerView, UnityPlayer> APIModelToViewMapping = new Dictionary<PlayerView, UnityPlayer>();

        public static bool ShouldUpdateUI;
        public static bool ShouldStartWindow;
        public static bool IsVirtualProjectWorkflowInitialized;

        [MenuItem("Window/Multiplayer/Multiplayer Play Mode")]
        static void ShowConfiguration()
        {
            if (IsVirtualProjectWorkflowInitialized)
            {
                ShouldStartWindow = true;
            }
            else
            {
                Debug.LogWarning("MPPM is not enabled. [Preferences->Multiplayer Play Mode]");
            }
        }

        static MultiplayerWindowController()
        {
            VirtualProjectWorkflow.OnInitialized += isMainEditor =>
            {
                if (!isMainEditor)
                {
                    MppmLog.Debug("We are a clone. No need to open the projects window.");
                    return;
                }
                IsVirtualProjectWorkflowInitialized = true;
            };
            VirtualProjectWorkflow.OnDisabled += isMainEditor =>
            {
                if (!isMainEditor)
                {
                    MppmLog.Debug("We are a clone. No need to open the projects window.");
                    return;
                }
                IsVirtualProjectWorkflowInitialized = false;
            };

            if (!IsVirtualProjectWorkflowInitialized)
                return;

            // Events that update the UI
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
            CompilationPipeline.assemblyCompilationFinished += (_, _) => { ShouldUpdateUI = true; };
            EditorApplication.playModeStateChanged += _ => { ShouldUpdateUI = true; };
            MultiplayerPlaymode.PlayerTags.OnUpdated += () => { ShouldUpdateUI = true; };
#if UNITY_USE_MULTIPLAYER_ROLES
            EditorMultiplayerRolesManager.ActiveMultiplayerRoleChanged += () => { ShouldUpdateUI = true; };
            EditorMultiplayerRolesManager.EnableMultiplayerRolesChanged += () => { ShouldUpdateUI = true; };
#endif

            // Main window update loop
            EditorApplication.update += () =>
            {
#if UNITY_USE_MULTIPLAYER_ROLES
                foreach (var (view, player) in APIModelToViewMapping)
                {
                    var roleString = player.Role switch
                    {
                        MultiplayerRoleFlags.ClientAndServer => RoleServerClient,
                        MultiplayerRoleFlags.Server => RoleServer,
                        MultiplayerRoleFlags.Client => RoleClient,
                        _ => RoleServerClient,
                    };
                    // Updates from the role if the json file change
                    // see :UpdatedDataStore for where this would have changed
                    if (view.MultiplayerRolesDropdown.value != roleString)
                    {
                        ShouldUpdateUI = true;
                        break;
                    }
                }
#endif

                if (ShouldStartWindow && IsVirtualProjectWorkflowInitialized)
                {
                    ShouldStartWindow = false;
                    Start();
                }

                if (ShouldUpdateUI && IsVirtualProjectWorkflowInitialized && s_MultiplayerWindow != null)
                {
                    ShouldUpdateUI = false;
                    Update();
                }

#if UNITY_MP_TOOLS_DEV
                foreach (var (view, player) in APIModelToViewMapping)
                {
                    var isPlayerMain = player.Type == PlayerType.Main;
                    view.UIActivateState(isPlayerMain, (PlayerView.PlayerState)player.PlayerState, MultiplayerPlaymodeEditorUtility.IsPlayerActivateProhibited);
                }
#endif

            };
        }

        static void Start()
        {
            if (s_MultiplayerWindow == null)
            {
                s_MultiplayerWindow = EditorWindow.GetWindow<MultiplayerWindow>();
                s_MultiplayerWindow.titleContent = new GUIContent(k_Title);

                APIModelToViewMapping.Clear();

                for (var index = 0; index < MultiplayerPlaymode.Players.Length; index++)
                {
                    var player = MultiplayerPlaymode.Players[index];
                    var view = new PlayerView(player);
                    var captureIndex = index;
#if UNITY_USE_MULTIPLAYER_ROLES
                    view.MultiplayerRolesDropdown.RegisterValueChangedCallback(evt =>
                    {
                        var unityPlayer = APIModelToViewMapping[view];
                        unityPlayer.Role = evt.newValue switch
                        {
                            RoleServerClient => MultiplayerRoleFlags.ClientAndServer,
                            RoleServer => MultiplayerRoleFlags.Server,
                            RoleClient => MultiplayerRoleFlags.Client,
                            _ => throw new ArgumentOutOfRangeException(),
                        };
                    });
#endif
                    view.ActiveUpdatedEvent += newValue =>
                    {
                        if (newValue)
                        {
                            if (!EditorApplication.isPlaying && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            {
                                return;
                            }

                            if (!player.Activate(out var error))
                            {
                                if (error == ActivationError.CompileErrors)
                                {
                                    MppmLog.Warning("Cannot activate a player while there are compile errors");
                                }
                            }
                        }
                        else
                        {
                            player.Deactivate(out _);
                        }

                        ShouldUpdateUI = true;
                    };
                    view.PlayerTagDropdown.RegisterValueChangedCallback(evt =>
                    {
                        Debug.Assert(player.Tags != null, "Tags should never be null");

                        var newValue = evt.newValue;
                        if (newValue == PlayerView.TagDefault) return;
                        if (newValue == PlayerView.TagLineBreak) return;

                        if (newValue != PlayerView.TagCreateTag)
                        {
                            if (!player.AddTag(newValue, out var tagError))
                            {
                                switch (tagError)
                                {
                                    case TagError.InPlayMode:
                                        MppmLog.Warning("Cannot modify tag while player is active in play mode");
                                        break;
                                    case TagError.Duplicate:
                                        MppmLog.Warning($"Attempting to add tag \"{newValue}\" to a player that already have \"{newValue}\" assigned to the player.");
                                        break;
                                    case TagError.None:
                                    case TagError.DoesNotExist:
                                    case TagError.Empty:
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }

                            view.AddPill(newValue);
                        }
                        else
                        {
                            var result = ModeSwitcher.TryOpenProjectSettingsWindow("Project/Multiplayer/Playmode");
                            Debug.Assert(result, "Could not open project settings window.");
                        }

                        view.PlayerTagDropdown.SetValueWithoutNotify(PlayerView.TagDefault);

                        ShouldUpdateUI = true;
                    });
                    view.PillCloseEvent += tagEntry =>
                    {
                        if (!player.RemoveTag(tagEntry, out var tagError))
                        {
                            switch (tagError)
                            {
                                case TagError.DoesNotExist:
                                    MppmLog.Warning($"Attempting to remove tag \"{tagEntry}\" from a player without \"{tagEntry}\" assigned to the player.");
                                    break;
                                case TagError.None:
                                case TagError.InPlayMode:
                                case TagError.Duplicate:
                                case TagError.Empty:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        var canModifyTag1 = player.PlayerState is not (PlayerState.Launched or PlayerState.Launching) || !EditorApplication.isPlaying;
                        view.RepopulateTagsAndPills(MultiplayerPlaymode.PlayerTags.Tags, player.Tags, canModifyTag1);
                        ShouldUpdateUI = true;
                    };

                    player.OnPlayerCommunicative += () =>
                    {
                        ShouldUpdateUI = true;
                    };

                    var isPlayerActive = player.PlayerState is PlayerState.Launched or PlayerState.Launching;
                    var isPlayerMainEditor = player.Type == PlayerType.Main;

                    if (isPlayerMainEditor)
                    {
                        view.AddToClassList(PlayerView.ClassNames.mainEditorPlayer);
                    }
                    else
                    {
                        view.PlayerViewContent.AddManipulator(new ContextualMenuManipulator(populateEvent =>
                        {
                            PlayerContextMenuOptions(populateEvent, player, captureIndex);
                        }));
                        var t = new ContextualMenuManipulator(populateEvent =>
                        {
                            PlayerContextMenuOptions(populateEvent, player, captureIndex);
                        });
                        t.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
                        view.EllipsesContainer.AddManipulator(t);
                        view.EllipseIcon.AddToClassList("ellipse-icon");
                        view.EllipsesContainer.Add(view.EllipseIcon);
                    }

                    // Format Name Foldout
                    view.NameToggle.text = player.Name;
                    view.NameToggle.value = isPlayerActive;

                    var checkMark = view.NameToggle.Q<VisualElement>("unity-checkmark");
                    var spacing = new VisualElement { style = { minWidth = 24, }, };
                    view.RefreshEditorIconBackgroundImage(isPlayerMainEditor);
                    view.EditorIcon.AddToClassList("editorIcon");
                    var foldoutComponents = new VisualElement
                    {
                        name = "foldout-components",
                        style = { flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row), },
                    };
                    foldoutComponents.Add(spacing);
                    foldoutComponents.Add(view.EditorIcon);
                    checkMark.parent.Insert(1, foldoutComponents);
                    view.NameToggle.SetEnabled(!MultiplayerPlaymodeEditorUtility.IsPlayerActivateProhibited || isPlayerActive);
                    var canModifyTag = player.PlayerState is not (PlayerState.Launched or PlayerState.Launching) ||
                                       !EditorApplication.isPlaying;
                    var logs = MultiplayerPlaymodeLogUtility.PlayerLogs(player.PlayerIdentifier).LogCounts;
                    view.RefreshEditorIconBackgroundImage(isPlayerMainEditor);
                    view.RepopulateTagsAndPills(MultiplayerPlaymode.PlayerTags.Tags, player.Tags, canModifyTag);
                    view.UILogCounts(isPlayerMainEditor, logs.Logs.ToString(), logs.Warnings.ToString(), logs.Errors.ToString());
                    view.UIActivateState(isPlayerMainEditor, (PlayerView.PlayerState)player.PlayerState, MultiplayerPlaymodeEditorUtility.IsPlayerActivateProhibited);
#if UNITY_USE_MULTIPLAYER_ROLES
                    view.MultiplayerRolesDropdown.style.display = EditorMultiplayerRolesManager.EnableMultiplayerRoles
                            ? DisplayStyle.Flex
                            : DisplayStyle.None;
                    view.MultiplayerRolesDropdown.SetValueWithoutNotify(isPlayerMainEditor
                        ? RoleServer
                        : RoleClient);
                    view.MultiplayerRolesDropdown.choices = new List<string>
                    {
                        RoleClient,
                        RoleServer,
                        RoleServerClient,
                    };
#else
                    view.MultiplayerRolesDropdown.style.display = DisplayStyle.None;
#endif

                    if (isPlayerMainEditor)
                    {
                        s_MultiplayerWindow.MainView.MainListView.Add(view);
                    }
                    else if (player.Type == PlayerType.Clone)
                    {
                        s_MultiplayerWindow.MainView.VirtualListView.Add(view);
                    }

                    APIModelToViewMapping.Add(view, player); // bind our view to the api model
                }
            }
            else
            {
                s_MultiplayerWindow.Focus();
            }
        }

        static void PlayerContextMenuOptions(ContextualMenuPopulateEvent populateEvent, UnityPlayer player, int index)
        {
            var openInExplorerContextualMenuLabel = Application.platform switch
            {
                RuntimePlatform.WindowsEditor => "Open in Explorer",
                RuntimePlatform.OSXEditor => "Show in Finder",
                _ => "Open Directory",
            };

            populateEvent.menu.AppendAction(
                openInExplorerContextualMenuLabel,
                _ => MultiplayerPlaymodeEditorUtility.RevealInFinder(player),
                DropdownMenuAction.AlwaysEnabled);
            populateEvent.menu.AppendAction(
                "Focus on Player",
                _ =>
                {
                    var err = MultiplayerPlaymodeEditorUtility.FocusPlayerView((PlayerIndex)index + 1);
                    if (err != default)
                    {
                        MppmLog.Debug(err == MultiplayerPlaymodeEditorUtility.FocusPlayerStatus.PlayerNotFound
                            ? $"Failed to find project for {player.Name} using {index}"
                            : $"Failed to open the window {err}");
                    }
                },
                DropdownMenuAction.AlwaysEnabled);
        }

        static void Update()
        {
            var showPlayerLaunchingHelpBox = false;
            foreach (var (view, player) in APIModelToViewMapping)
            {
                // double check log counts. this was broken and this probably fixes it
                var canModifyTag = player.PlayerState is not (PlayerState.Launched or PlayerState.Launching) ||
                                   !EditorApplication.isPlaying;

                var logs = MultiplayerPlaymodeLogUtility.PlayerLogs(player.PlayerIdentifier).LogCounts;
                var isPlayerMain = player.Type == PlayerType.Main;
                view.RefreshEditorIconBackgroundImage(isPlayerMain);
                view.RepopulateTagsAndPills(MultiplayerPlaymode.PlayerTags.Tags, player.Tags, canModifyTag);
                view.UILogCounts(isPlayerMain, logs.Logs.ToString(), logs.Warnings.ToString(), logs.Errors.ToString());
                view.UIActivateState(isPlayerMain, (PlayerView.PlayerState)player.PlayerState, MultiplayerPlaymodeEditorUtility.IsPlayerActivateProhibited);

                showPlayerLaunchingHelpBox = showPlayerLaunchingHelpBox || (!isPlayerMain && player.PlayerState is PlayerState.Launching);
#if UNITY_USE_MULTIPLAYER_ROLES
                var role = player.Role switch
                {
                    MultiplayerRoleFlags.ClientAndServer => RoleServerClient,
                    MultiplayerRoleFlags.Server => RoleServer,
                    MultiplayerRoleFlags.Client => RoleClient,
                    _ => RoleServerClient,
                };
                view.MultiplayerRolesDropdown.style.display = EditorMultiplayerRolesManager.EnableMultiplayerRoles
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
                view.MultiplayerRolesDropdown.SetValueWithoutNotify(role);
#endif
            }

            if (MultiplayerPlaymodeEditorUtility.IsPlayerActivateProhibited)
            {
                s_MultiplayerWindow.MainView.AddToClassList(MainView.k_HasCompileErrorsClassName);
            }
            else
            {
                s_MultiplayerWindow.MainView.RemoveFromClassList(MainView.k_HasCompileErrorsClassName);
            }

            if (!MultiplayerPlayModeSettings.GetIsMppmActive())
            {
                s_MultiplayerWindow.MainView.AddToClassList(MainView.k_HasMPPMDisabled);
            }
            else
            {
                s_MultiplayerWindow.MainView.RemoveFromClassList(MainView.k_HasMPPMDisabled);
            }

            if (showPlayerLaunchingHelpBox)
            {
                s_MultiplayerWindow.MainView.AddToClassList(MainView.k_HasPlayerLaunchingClassName);
            }
            else
            {
                s_MultiplayerWindow.MainView.RemoveFromClassList(MainView.k_HasPlayerLaunchingClassName);
            }
        }

        static void OnSceneChanged(Scene _, Scene __)
        {
            foreach (var (view, player) in APIModelToViewMapping)
            {
                view.RefreshEditorIconBackgroundImage(player.Type == PlayerType.Main);
            }
        }
        [Shortcut("FocusPlayerOne", KeyCode.F9, ShortcutModifiers.Control)]
        public static void FocusPlayerOne()
        {
            _ = MultiplayerPlaymodeEditorUtility.FocusPlayerView(PlayerIndex.Player1);
        }
        [Shortcut("FocusPlayerTwo", KeyCode.F10, ShortcutModifiers.Control)]
        public static void FocusPlayerTwo()
        {
            _ = MultiplayerPlaymodeEditorUtility.FocusPlayerView(PlayerIndex.Player2);
        }
        [Shortcut("FocusPlayerThree", KeyCode.F11, ShortcutModifiers.Control)]
        public static void FocusPlayerThree()
        {
            _ = MultiplayerPlaymodeEditorUtility.FocusPlayerView(PlayerIndex.Player3);
        }
        [Shortcut("FocusPlayerFour", KeyCode.F12, ShortcutModifiers.Control)]
        public static void FocusPlayerFour()
        {
            _ = MultiplayerPlaymodeEditorUtility.FocusPlayerView(PlayerIndex.Player4);
        }
    }
}
