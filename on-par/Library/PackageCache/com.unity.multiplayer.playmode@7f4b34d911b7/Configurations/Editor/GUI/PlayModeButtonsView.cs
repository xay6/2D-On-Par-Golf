using Unity.Multiplayer.PlayMode.Common.Editor;
using Unity.Multiplayer.PlayMode.Configurations.Editor;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Multiplayer.PlayMode.Configurations.Editor.Gui
{
    internal class PlayModeButtonsView : UxmlView<PlayModeButtonsView>
    {
        private EditorToolbarToggle m_PlayButton;
        private EditorToolbarToggle m_PauseButton;
        private EditorToolbarButton m_StepButton;
        private PlaymodeDropdownButton m_DropdownButton;

        internal EditorToolbarToggle PlayButton => m_PlayButton;
        internal EditorToolbarToggle PauseButton => m_PauseButton;
        internal EditorToolbarButton StepButton => m_StepButton;
        internal PlaymodeDropdownButton DropdownButton => m_DropdownButton;


        public PlayModeButtonsView()
        {
            style.flexDirection = FlexDirection.Row;

            AddPlayStopButton();
            AddPauseButton();
            AddStepButton();
            AddDropdownButton();

            EditorToolbarUtility.SetupChildrenAsButtonStrip(this);

            RegisterCallback<AttachToPanelEvent>(OnAttachedToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachedToPanel(AttachToPanelEvent evt)
        {
            PlayModeManager.instance.ConfigAssetChanged += OnConfigAssetChanged;
            PlayModeManager.instance.StateChanged += OnPlayModeStateChanged;
            EditorApplication.pauseStateChanged += OnPauseStateChanged;

            EditorApplication.playModeStateChanged += OnEditorChangedPlaymodeState;

            // Setup initial states
            OnPlayModeStateChanged(PlayModeManager.instance.CurrentState == PlayModeState.Running ? PlayModeState.Running : PlayModeState.NotRunning);
            OnPauseStateChanged(EditorApplication.isPaused ? PauseState.Paused : PauseState.Unpaused);
            OnConfigAssetChanged();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            PlayModeManager.instance.ConfigAssetChanged -= OnConfigAssetChanged;
            PlayModeManager.instance.StateChanged -= OnPlayModeStateChanged;
            EditorApplication.pauseStateChanged -= OnPauseStateChanged;
        }

        // We need this because a user can bypass the UI and start the PlayMode via the Editor
        // or via Rider. Later we want to capture all those possibilities and make sure the scenario will be started.
        void OnEditorChangedPlaymodeState(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.EnteredPlayMode)
            {
                m_PlayButton.SetValueWithoutNotify(true);

                if (PlayModeManager.instance.CurrentState != PlayModeState.Running && PlayModeManager.instance.ActivePlayModeConfig.GetType() != typeof(DefaultPlayModeConfig))
                {
                    Debug.LogWarning("Entered Playmode, without starting a Scenario (Did you enter PlayMode from an IDE?). Starting scenarios only works via the UI at the moment. \n" +
                                     "Please start a scenario via the UI to make sure the scenario is started correctly.");
                }
            }

            // Maybe we exit PlayMode from rider, make sure we stop the scenario in that case.
            if (playModeStateChange == PlayModeStateChange.ExitingPlayMode)
            {
                m_PlayButton.SetValueWithoutNotify(false);
                if (PlayModeManager.instance.CurrentState == PlayModeState.Running)
                {
                    PlayModeManager.instance.Stop();
                }
            }
        }



        private void AddPlayStopButton()
        {
            Add(m_PlayButton = new EditorToolbarToggle
            {
                name = "Play",
                tooltip = "Play",
                onIcon = LoadIcon("StopButton"),
                offIcon = LoadIcon("PlayButton"),
            });
            m_PlayButton.RegisterValueChangedCallback(OnPlayButtonValueChanged);
        }

        private void AddPauseButton()
        {
            Add(m_PauseButton = new EditorToolbarToggle
            {
                name = "Pause",
                tooltip = "Pause",
                onIcon = LoadIcon("PauseButton On"),
                offIcon = LoadIcon("PauseButton"),
            });
            m_PauseButton.RegisterValueChangedCallback(OnPauseButtonValueChanged);
        }

        private void AddStepButton()
        {
            Add(m_StepButton = new EditorToolbarButton
            {
                name = "Step",
                tooltip = "Step",
                icon = LoadIcon("StepButton"),
            });
            m_StepButton.clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.RightMouse });
            m_StepButton.clicked += OnStepButtonClicked;
        }

        private void AddDropdownButton()
        {
            Insert(0, m_DropdownButton = new PlaymodeDropdownButton());
        }

        static private Texture2D LoadIcon(string name)
        {
            return EditorGUIUtility.IconContent(name).image as Texture2D;
        }


        private void OnConfigAssetChanged()
        {
            var currentConfig = PlayModeManager.instance.ActivePlayModeConfig;
            var so = new SerializedObject(currentConfig);
            this.Unbind();
            this.TrackSerializedObjectValue(so, o =>
            {
                ValidateCurrentConfiguration(currentConfig);
            });
            ValidateCurrentConfiguration(currentConfig);

            var supportsPauseAndStep = PlayModeManager.instance.SupportsPauseAndStep;

            m_PauseButton.SetEnabled(supportsPauseAndStep);

            // We're supposed to be in NotRunning state here and so Step should be disabled.
            m_StepButton.SetEnabled(false);
        }

        void ValidateCurrentConfiguration(PlayModeConfig currentConfig)
        {
            var playmodeConfigIsValid = currentConfig.IsConfigurationValid(out var reason);
            m_DropdownButton.UpdateUI(currentConfig);
            m_PlayButton.tooltip = playmodeConfigIsValid ? "Play" : "Playmode not available.\n" + reason;
            m_PlayButton.SetEnabled(playmodeConfigIsValid);
        }


        private void OnPlayModeStateChanged(PlayModeState state)
        {
            m_PlayButton.SetValueWithoutNotify(state == PlayModeState.Running);
            m_StepButton.SetEnabled(PlayModeManager.instance.SupportsPauseAndStep && state == PlayModeState.Running);
        }

        private void OnPauseStateChanged(PauseState state)
        {
            m_PauseButton.SetValueWithoutNotify(state == PauseState.Paused);
        }

        private void OnStepButtonClicked()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.Step();
            }
        }

        private void OnPlayButtonValueChanged(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
            {
                PlayModeManager.instance.Start();
            }
            else
            {
                if (PlayModeManager.instance.CurrentState != PlayModeState.Running)
                {
                    EditorApplication.isPlaying = false;
                }
                PlayModeManager.instance.Stop();
            }

            // set the state of the button manually to make sure we are in sync with the actual state.
            var button = evt.target as EditorToolbarToggle;
            button.SetValueWithoutNotify(PlayModeManager.instance.CurrentState == PlayModeState.Running);
        }

        private void OnPauseButtonValueChanged(ChangeEvent<bool> evt)
        {
            EditorApplication.isPaused = evt.newValue;
        }
    }
}
