using System;
using System.Threading.Tasks;
using Codice.Client.BaseCommands;
using OnPar.Routers;
using PlasticGui.Configuration.CloudEdition.Welcome;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class EnterMatchmakerQueueEvent : AppEvent
    {
        public string QueueName { get; private set; }

        public EnterMatchmakerQueueEvent(string queueName)
        {
            QueueName = queueName;
        }
    }

    internal class StartSinglePlayerModeEvent : AppEvent { }

    /// <summary>
    /// Called to stop the matchmaker
    /// </summary>
    internal class ExitMatchmakerQueueEvent : AppEvent { }
    
    /// <summary>
    /// Called after the matchmaking stops
    /// </summary>
    internal class ExitedMatchmakerQueueEvent : AppEvent { }
    
    internal class MatchLoadingEvent : AppEvent { }
    internal class ExitMatchLoadingEvent : AppEvent { }

    internal class PlayerSignedIn : AppEvent
    {
        public bool Success { get; private set; }
        public string PlayerId { get; private set; }

        public PlayerSignedIn(bool success, string playerId)
        {
            Success = success;
            PlayerId = playerId;
        }
    }

    /// <summary>
    /// Event triggered when entering the leaderboard screen.
    /// </summary>
    internal class EnterLeaderboardEvent : AppEvent { }

    /// <summary>
    /// Event triggered when exiting the leaderboard screen.
    /// </summary>
    internal class ExitLeaderboardEvent : AppEvent { }

    internal class EnterLoginEvent : AppEvent { }
    internal class ExitLoginEvent : AppEvent { }
    internal class LoginAttemptEvent : AppEvent
    {
        public string Username { get; }
        public string Password { get; }
        public static LoginRegisterResponse Login;

        public LoginAttemptEvent(string username, string password)
        {
            LoginHelper(username, password);
        }

        public static async void LoginHelper(string username, string password)
        {
            Login = await OnPar.RouterHandlers.Handlers.LoginHandler(username, password);
        }
    }

    internal class SignupAttemptEvent : AppEvent
    {
        public string Username { get; }
        public string Password { get; }
        public static LoginRegisterResponse Signup;

        public SignupAttemptEvent(string username, string password)
        {
            RegisterHelper(username, password);
        }

        public static async void RegisterHelper(string username, string password)
        {
            Signup = await OnPar.RouterHandlers.Handlers.RegisterHandler(username, password);
        }
    }

    internal class EnterGuestEvent : AppEvent { }
    internal class ExitGuestEvent : AppEvent { }
    internal class StartGameEvent : AppEvent { }
}
