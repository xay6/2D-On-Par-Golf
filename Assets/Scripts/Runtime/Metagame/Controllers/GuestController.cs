using System;
using System.Collections;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using Unity.Services.Multiplayer;
using UnityEngine;
using static Unity.Services.Matchmaker.Models.MultiplayAssignment;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class GuestController : Controller<MetagameApplication>
    {
        GuestView View => App.View.Guest;

        void Awake()
        {
            AddListener<EnterGuestEvent>(OnEnterGuest);
            AddListener<ExitGuestEvent>(OnExitGuest);
        }

        void OnDestroy()
        {
            RemoveListeners();
        }

        internal override void RemoveListeners()
        {
            RemoveListener<EnterGuestEvent>(OnEnterGuest);
            RemoveListener<ExitGuestEvent>(OnExitGuest);
        }

        void OnEnterGuest(EnterGuestEvent evt)
        {
            View.Show();
        }

        void OnExitGuest(ExitGuestEvent evt)
        {
            View.Hide();
        }

    }
}
