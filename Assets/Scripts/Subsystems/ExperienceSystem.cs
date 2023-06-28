using TMPro;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI _levelText;
    //[SerializeField] private TextMeshProUGUI _levelPercentText;
    //[SerializeField] private ExperienceBar _experienceBar;
    [SerializeField] private ParticleSystem _levelUp;

    private int _level = 1;
    private float _experience = 0;
    private float _nextLevelExperienceStartValue = 50;
    private float _nextLevelExperience;
    private float _nextLevelMultiplier = 1.2f;
    private StatsSystem _statSystem;
    private Player _player;

    public delegate void OnLevelChangedHandler(int level);
    public delegate void OnExperienceChangedHandler(float levelPercent);

    public event OnLevelChangedHandler OnLevelChanged;
    public event OnExperienceChangedHandler OnExperienceChanged;

    public int Level { 
        get 
        {
            return _level;
        }  
        set 
        {
            _experience = 0;
            _level = value > 1 ? value : 1;
            _statSystem.SetLevelStats(_level);
            _nextLevelExperience = _nextLevelExperienceStartValue * Mathf.Pow(_nextLevelMultiplier, _level);
            UpdateExperienceOnHUD();
        } 
    }
    public float Experience => _experience;

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        _nextLevelExperience = _nextLevelExperienceStartValue * _nextLevelMultiplier;
        UpdateExperienceOnHUD();
    }

    private void CacheComponents()
    {
        _statSystem = gameObject.GetComponent<StatsSystem>();
        _player = GetComponent<Player>();
    }

    public void AddExperience(float experience)
    {
        _experience += experience;

        if (_experience >= _nextLevelExperience)
        {
            LevelUp();
        }

        //OnExperienceChanged.Invoke(_experience / _nextLevelExperience);
        UpdateExperienceOnHUD();
    }

    private void LevelUp()
    {
        _level++;
        _experience = _experience - _nextLevelExperience;
        _nextLevelExperience = _nextLevelExperienceStartValue * Mathf.Pow(_nextLevelMultiplier, _level);
        _levelUp.Play();
        _statSystem.SetLevelStats(_level);
    }

    private void UpdateExperienceOnHUD()
    {
        if (_player != null)
        {
            //OnLevelChanged.Invoke(_level);
            //OnExperienceChanged.Invoke(_experience / _nextLevelExperience);
            //_levelText.text = _level.ToString();
            //_levelPercentText.text = ((int)(_experience / _nextLevelExperience * 100)).ToString() + " %";
            //_experienceBar.SetFill(_experience / _nextLevelExperience);
            OnLevelChanged.Invoke(_level);
            OnExperienceChanged.Invoke(_experience / _nextLevelExperience);
        }
    }
}
