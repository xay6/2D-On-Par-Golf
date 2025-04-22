using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Template.Multiplayer.NGO.Runtime;
using OnPar.Routers;
using OnPar.RouterHandlers;

internal class AllLevelsController : Controller<MetagameApplication>
{
    AllLevelsView View => App.View.AllLevels;

    void Awake()
    {
        AddListener<EnterAllLevelsEvent>(OnEnter);
        AddListener<ExitAllLevelsEvent>(OnExit);
    }

    async void OnEnter(EnterAllLevelsEvent evt)
    {
        string username = LoginRegister.getUsername();
        var response = await Handlers.GetAllUserScoresHandler(username);

        if (response != null && response.success && response.scores != null)
        {
            var levels = new List<LevelData>();

            foreach (var scoreEntry in response.scores)
            {
                levels.Add(new LevelData
                {
                    courseId = scoreEntry.courseId,
                    score = scoreEntry.score
                });
            }

            Debug.Log($"Levels count: {levels.Count}");
            foreach (var level in levels)
            {
                Debug.Log($"Level: {level.courseId}, Score: {level.score}");
            }
            
            View.Initialize(levels);
            View.Show();
        }
        else
        {
            Debug.LogError("Failed to load level data from server or scores were null.");
            View.Initialize(new List<LevelData>()); // fallback: prevent null arg crash
            View.Show();
        }
    }


    void OnExit(ExitAllLevelsEvent evt)
    {
        View.Hide();
    }

    internal override void RemoveListeners()
    {
        RemoveListener<EnterAllLevelsEvent>(OnEnter);
        RemoveListener<ExitAllLevelsEvent>(OnExit);
    }
}
