using System;
using System.Threading.Tasks;
using Codice.Client.BaseCommands;
using OnPar.Routers;
using PlasticGui.Configuration.CloudEdition.Welcome;
using UnityEngine;

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
            Username = username;
            Password = password;
        }

        public static Func<string, string, Task<LoginRegisterResponse>> LoginHandler = 
            (username, password) => OnPar.RouterHandlers.Handlers.LoginHandler(username, password);

        public static async Task<bool> LoginHelper(string username, string password)
        {
            try
            {
                var response = await LoginHandler(username, password);
                return response != null && response.success;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Login failed: {ex.Message}");
                return false;
            }
        }
    }

    internal class SignupAttemptEvent : AppEvent
    {
        public string Username { get; }
        public string Password { get; }
        public static LoginRegisterResponse Signup;
        public SignupAttemptEvent(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public static Func<string, string, Task<LoginRegisterResponse>> RegisterHandler =
            (username, password) => OnPar.RouterHandlers.Handlers.RegisterHandler(username, password);

        public static async Task<bool> RegisterHelper(string username, string password)
        {
            try
            {
                var response = await RegisterHandler(username, password);
                return response != null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Signup failed: {ex.Message}");
                return false;
            }
        }
    }

    internal class EnterGuestEvent : AppEvent { }
    internal class ExitGuestEvent : AppEvent { }

    internal class EnterAccountEvent : AppEvent { }
    internal class ExitAccountEvent : AppEvent { }
    internal class StartGameEvent : AppEvent { }
}
