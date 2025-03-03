using System;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.Threading;
using Unity.Multiplayer.PlayMode.Common.Editor;

namespace Unity.Multiplayer.PlayMode.Configurations.Editor
{
    /// <summary>
    /// The Play Mode Manager provides an API to control advanced scenarios of the play mode state of the editor.
    /// </summary>
    [FilePath("Packages/com.unity.dedicated-server/PlayModeManager.asset", FilePathAttribute.Location.PreferencesFolder)]
    internal class PlayModeManager : ScriptableSingleton<PlayModeManager>
    {
        [InitializeOnLoadMethod]
        private static void OnDomainReload()
        {
            if (instance.m_StateMachine.CurrentState == PlayModeState.Running)
                instance.Resume();
        }

        private StateMachine<PlayModeState> m_StateMachine;
        [SerializeField] private PlayModeConfig m_Config;
        private PlayModeConfig m_DefaultConfig;
        private CancellationTokenSource m_CancellationTokenSource;
        private CancellationToken m_CancellationToken;

        /// <summary>
        /// An event that is raised when the play mode state changes.
        /// </summary>
        public event Action<PlayModeState> StateChanged
        {
            add => GetStateMachine().StateChanged += value;
            remove => GetStateMachine().StateChanged -= value;
        }

        /// <summary>
        /// An event that is raised when the active config asset changes.
        /// </summary>
        public event Action ConfigAssetChanged;

        /// <summary>
        /// The current play mode state.
        /// </summary>
        public PlayModeState CurrentState => GetStateMachine().CurrentState;

        /// <summary>
        /// True if the current play mode config supports pause and step.
        /// </summary>
        public bool SupportsPauseAndStep => ActivePlayModeConfig.SupportsPauseAndStep;

        /// <summary>
        /// The default play mode config asset.
        /// </summary>
        internal PlayModeConfig DefaultConfig
        {
            get
            {
                if (m_DefaultConfig == null)
                    m_DefaultConfig = CreateInstance<DefaultPlayModeConfig>();

                return m_DefaultConfig;
            }
        }

        /// <summary>
        /// The active play mode config asset.
        /// </summary>
        public PlayModeConfig ActivePlayModeConfig
        {
            get
            {
                if (m_Config == null)
                {
                    m_Config = DefaultConfig;
                    ConfigAssetChanged?.Invoke();
                }
                return m_Config;
            }
            set
            {
                if (GetStateMachine().CurrentState != PlayModeState.NotRunning || GetStateMachine().IsTransitioning())
                    throw new InvalidOperationException("Cannot set config while in a running state");

                if (m_Config == value)
                    return;

                m_Config = value;
                ConfigAssetChanged?.Invoke();
            }
        }

        /// <summary>
        /// Initializes the Play Mode Manager.
        /// This method is called by the Unity Editor only once during the lifetime of the editor,
        /// i.e. it's not called every time domain is reloaded.
        /// </summary>
        private void Awake()
        {
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            m_StateMachine = new StateMachine<PlayModeState>(PlayModeState.NotRunning);
            GetStateMachine().DefineTransition(PlayModeState.NotRunning, PlayModeState.Running);
            GetStateMachine().DefineTransition(PlayModeState.Running, PlayModeState.NotRunning);
        }

        private StateMachine<PlayModeState> GetStateMachine()
        {
            if (m_StateMachine == null)
            {
                SetupStateMachine();
                Debug.LogAssertion("PlayModeManager StateMachine was null, this should not happen");
            }

            return m_StateMachine;
        }

        /// <summary>
        /// Starts the play mode.
        /// </summary>
        public void Start()
        {
            if (!ActivePlayModeConfig.IsConfigurationValid(out var reason))
            {
                Debug.LogError("Cannot enter Playmode: " + reason);
                return;
            }

            TransitionToState(PlayModeState.Running);
        }

        private void Resume()
        {
            m_CancellationTokenSource = new CancellationTokenSource();
            m_CancellationToken = m_CancellationTokenSource.Token;

            instance.ActivePlayModeConfig.ExecuteResume(m_CancellationToken);
        }


        /// <summary>
        /// Stops the play mode.
        /// </summary>
        public void Stop()
        {
            var isInTransition = GetStateMachine().IsTransitioning();
            var transitionToState = GetStateMachine().NextState;

            // Handle the case where we are starting and the transition has not yet completed
            if (isInTransition && transitionToState != PlayModeState.NotRunning)
            {
                if (m_CancellationTokenSource == null)
                {
                    Debug.LogAssertion("CancellationTokenSource was null, this should not happen");
                    // This means we entered a bad state, as a temporary workaround we can reset the state machine
                    SetupStateMachine();
                    return;
                }
                m_CancellationTokenSource?.Cancel();
                return;
            }

            // Already transitioning to NotRunning no need to transition again.
            // Can happen when stop is called from externally (Rider)
            if (isInTransition && transitionToState == PlayModeState.NotRunning)
                return;

            TransitionToState(PlayModeState.NotRunning);
        }

        private void TransitionToState(PlayModeState state)
        {
            var currentState = GetStateMachine().CurrentState;

            if (currentState == state)
                return;

            if (!GetStateMachine().IsValidTransition(currentState, state))
                throw new InvalidOperationException($"Invalid transition from {currentState} to {state}");

            if (GetStateMachine().IsTransitioning())
                throw new InvalidOperationException($"Cannot transition from {currentState} to {state} while already transitioning to {GetStateMachine().NextState}");

            m_CancellationTokenSource = new CancellationTokenSource();
            m_CancellationToken = m_CancellationTokenSource.Token;

            GetStateMachine().TransitionAsync(state, TransitionActionAsync, m_CancellationToken).Forget();
        }

        private Task TransitionActionAsync(PlayModeState from, PlayModeState to, CancellationToken cancellationToken)
        {
            switch (to)
            {
                case PlayModeState.Running:
                    return instance.ActivePlayModeConfig.ExecuteStartAsync(cancellationToken);
                case PlayModeState.NotRunning:
                    instance.ActivePlayModeConfig.ExecuteStop();
                    return Task.CompletedTask;
                default:
                    throw new ArgumentOutOfRangeException(nameof(to), to, null);
            }
        }
    }
}
