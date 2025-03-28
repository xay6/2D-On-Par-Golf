using System;
using System.Collections;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using Unity.Services.Multiplayer;
using UnityEngine;
using static Unity.Services.Matchmaker.Models.MultiplayAssignment;
using UnityEngine.SceneManagement;


namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class GuestController : Controller<MetagameApplication>
    {
        GuestView View => App.View.Guest;

        void Awake()
        {
            Debug.Log("🎯 GuestController Awake");
            AddListener<EnterGuestEvent>(OnEnterGuest);
            AddListener<ExitGuestEvent>(OnExitGuest);
            AddListener<StartGameEvent>(OnStartGame);
        }

        void OnDestroy()
        {
            RemoveListeners();
            
        }

        internal override void RemoveListeners()
        {
            RemoveListener<EnterGuestEvent>(OnEnterGuest);
            RemoveListener<ExitGuestEvent>(OnExitGuest);
            RemoveListener<StartGameEvent>(OnStartGame);
        }

        void OnEnterGuest(EnterGuestEvent evt)
        {
            View.Show();
             Broadcast(new StartGameEvent());
        }

        void OnExitGuest(ExitGuestEvent evt)
        {
            View.Hide();
        }
         void OnStartGame(StartGameEvent evt)
        {
            Debug.Log("Guest is starting the game. Loading Level01...");
            SceneManager.LoadScene("Level01"); // Replace "Level01" with the actual name if different
        }

    }
}
