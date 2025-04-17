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

        if (response != null && response.success)
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

            View.Initialize(levels);
            View.Show();
        }
        else
        {
            Debug.LogError("Failed to load level data from server.");
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
