using TMPro;
using UnityEngine;

public class LevelCompletedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _completedLevel;
    [SerializeField] private TextMeshProUGUI _nextLevelGoal;

    public void SetCompletedLevelNumber(int level)
    {
        _completedLevel.text = $"LEVEL {level}";
    }

    public void SetNextLevelGoal(int nextLevelGoal)
    {
        _nextLevelGoal.text = $"KILL {nextLevelGoal} ENEMIES";
    }
}
