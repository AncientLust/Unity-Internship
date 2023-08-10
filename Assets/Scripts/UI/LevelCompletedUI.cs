using TMPro;
using UnityEngine;

public class LevelCompletedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _completedLevel;
    [SerializeField] private TextMeshProUGUI _nextLevelGoal;

    private LevelProgressManager _levelProgressManager;
    private bool _isInitialized;

    public void Init(LevelProgressManager levelProgressManager)
    {
        _levelProgressManager = levelProgressManager;
        _isInitialized = true;
        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _levelProgressManager.onGameLevelChanged += SetCompletedLevelNumber;
        _levelProgressManager.onNextLevelGoalChanged += SetNextLevelGoal;
    }

    private void Unsubscribe()
    {
        _levelProgressManager.onGameLevelChanged -= SetCompletedLevelNumber;
        _levelProgressManager.onNextLevelGoalChanged -= SetNextLevelGoal;
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
