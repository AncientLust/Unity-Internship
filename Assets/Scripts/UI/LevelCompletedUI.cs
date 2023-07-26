using TMPro;
using UnityEngine;

public class LevelCompletedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _completedLevel;
    [SerializeField] private TextMeshProUGUI _nextLevelGoal;

    private LevelProgressManager _levelProgressManager;

    public void Init(LevelProgressManager levelProgressManager)
    {
        _levelProgressManager = levelProgressManager;
    }

    private void OnEnable()
    {
        if (_levelProgressManager != null)
        {
            _levelProgressManager.onGameLevelChanged += SetCompletedLevelNumber;
            _levelProgressManager.onNextLevelGoalChanged += SetNextLevelGoal;
        }
    }

    private void OnDisable()
    {
        if (_levelProgressManager != null)
        {
            _levelProgressManager.onGameLevelChanged -= SetCompletedLevelNumber;
            _levelProgressManager.onNextLevelGoalChanged -= SetNextLevelGoal;
        }
    }

    public void SetCompletedLevelNumber(int level)
    {
        _completedLevel.text = $"LEVEL {--level}";
    }

    public void SetNextLevelGoal(int nextLevelGoal)
    {
        _nextLevelGoal.text = $"KILL {nextLevelGoal} ENEMIES";
    }
}
