using System.Collections.Generic;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    [System.Serializable]
    public class AllUserScoresResponse
    {
        public bool success;
        public string message;
        public List<ScoreEntry> scores;
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public string courseId;
        public string username;
        public int score;
    }

    [System.Serializable]
    public class LevelData
    {
        public string courseId;
        public int score;
        public bool isUnlocked => score > 0;
    }
}
