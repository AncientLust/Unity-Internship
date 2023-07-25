using System;

public class LevelProgressManager
{
    private float _enemiesToKill = 5;
    private float _levelKillGoal = 5;
    private float _levelKillMultiplier = 1.5f;
    private int _gameLevel = 1;
    private LevelCompletedUI _levelCompletedUI;

    private EnemyDisposalManager _enemyDisposalManager;

    public Action<int, int> onKillProgressChnaged;
    public Action<int> onLevelGoalReached;

    public void Init(EnemyDisposalManager enemyDisposalManager, LevelCompletedUI levelCompletedUI)
    {
        _enemyDisposalManager = enemyDisposalManager;
        _levelCompletedUI = levelCompletedUI;
        Subscribe();
    }

    ~LevelProgressManager()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_enemyDisposalManager != null) _enemyDisposalManager.onDisposed += EnemyKilled;
    }

    private void Unsubscribe()
    {
        if (_enemyDisposalManager != null) _enemyDisposalManager.onDisposed -= EnemyKilled;
    }

    public void EnemyKilled()
    {
        _enemiesToKill--;
        onKillProgressChnaged.Invoke((int)_enemiesToKill, (int)_levelKillGoal);
        CheckIfLevelGoalReached();
    }

    private void CheckIfLevelGoalReached()
    {
        if (_enemiesToKill == 0)
        {
            CalculateNextLevelGoal();
            UpdateCompleteLevelUI();
            onLevelGoalReached.Invoke(++_gameLevel);
        }
    }

    private void UpdateCompleteLevelUI()
    {
        _levelCompletedUI.SetCompletedLevelNumber(_gameLevel);
        _levelCompletedUI.SetNextLevelGoal((int)_levelKillGoal);
    }

    private void CalculateNextLevelGoal()
    {
        _levelKillGoal *= _levelKillMultiplier;
        _enemiesToKill = _levelKillGoal;
    }
}
