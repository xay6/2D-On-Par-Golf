using System.Collections.Generic;
using UnityEngine;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class AllLevelsController : Controller<MetagameApplication>
    {
        AllLevelsView View => App.View.AllLevels;

        void Awake()
        {
            AddListener<EnterAllLevelsEvent>(OnEnter);
            AddListener<ExitAllLevelsEvent>(OnExit);
        }

        void OnDestroy()
        {
            RemoveListeners();
        }

        internal override void RemoveListeners()
        {
            RemoveListener<EnterAllLevelsEvent>(OnEnter);
            RemoveListener<ExitAllLevelsEvent>(OnExit);
        }

        void OnEnter(EnterAllLevelsEvent evt)
        {
            var levels = LevelDataProvider.GetAllLevels();
            int unlockedUpTo = PlayerPrefs.GetInt("UnlockedLevels", 1);
            View.Initialize(levels, unlockedUpTo);
            View.Show();
        }

        void OnExit(ExitAllLevelsEvent evt)
        {
            View.Hide();
        }
    }
}
