using System;
using UnityEngine;

public class LevelProgressManager
{
    private int _enemiesKilled;
    private int _levelKillGoal;
    private int _baseLevelKillGoal = 20;
    private float _levelKillMultiplier = 1.75f;
    private int _gameLevel = 1;

    private EnemyDisposalManager _enemyDisposalManager;

    public Action<int, int> onKillProgressChnaged;
    public Action<int> onLevelGoalReached;
    public Action<int> onCompletedLevelChanged;
    public Action<int> onNextLevelGoalChanged;
    public Action<int> onGameLevelChanged;

    public void Init(EnemyDisposalManager enemyDisposalManager)
    {
        _enemyDisposalManager = enemyDisposalManager;
        _levelKillGoal = _baseLevelKillGoal;
        onKillProgressChnaged.Invoke(_enemiesKilled, _levelKillGoal);
        Subscribe();
    }

    ~LevelProgressManager()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _enemyDisposalManager.onEnemyKilled += EnemyKilled;
    }

    private void Unsubscribe()
    {
        _enemyDisposalManager.onEnemyKilled -= EnemyKilled;
    }

    private void EnemyKilled()
    {
        _enemiesKilled++;
        onKillProgressChnaged.Invoke(_enemiesKilled, _levelKillGoal);
        CheckIfLevelGoalReached();
    }

    private void CheckIfLevelGoalReached()
    {
        if (_enemiesKilled == _levelKillGoal)
        {
            onLevelGoalReached.Invoke(_gameLevel);
            CalculateNextLevelGoal();
            IncrementGameLevel();
        }
    }

    private void CalculateNextLevelGoal()
    {
        _enemiesKilled = 0;
        _levelKillGoal = (int)(_levelKillGoal * _levelKillMultiplier);
        _levelKillGoal = Mathf.RoundToInt(_levelKillGoal / 10.0f) * 10;
        onNextLevelGoalChanged.Invoke(_levelKillGoal);
        onKillProgressChnaged.Invoke(_enemiesKilled, _levelKillGoal);
    }

    private void IncrementGameLevel()
    {
        _gameLevel++;
        onGameLevelChanged.Invoke(_gameLevel);
    }

    public void ResetProgress()
    {
        _enemiesKilled = 0;
        _levelKillGoal = _baseLevelKillGoal;
        _gameLevel = 1;

        onGameLevelChanged.Invoke(_gameLevel);
        onKillProgressChnaged.Invoke(_enemiesKilled, _levelKillGoal);
    }
}
