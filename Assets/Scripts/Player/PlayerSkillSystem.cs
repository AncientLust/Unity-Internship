using UnityEngine;

public class PlayerSkillSystem : MonoBehaviour
{
    private PlayerInputSystem _inputSystem;
    private ISkill[] _skillSlots;

    public void Init(PlayerInputSystem playerInputSystem, ISkill[] skills)
    {
        _inputSystem = playerInputSystem;
        _skillSlots = skills;
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _inputSystem.onSkill1Clicked += ExecuteSkillSlot;
    }

    private void Unsubscribe()
    {
        _inputSystem.onSkill1Clicked -= ExecuteSkillSlot;
    }

    private void ExecuteSkillSlot(int skillIndex)
    {
        if (skillIndex > _skillSlots.Length - 1 || skillIndex < 0)
        {
            Debug.LogError($"Invalid skill index {skillIndex}");
        }
        else
        {
            _skillSlots[skillIndex].Execute();
        }
    }

    public void ResetSkillsCooldown()
    {
        foreach (var skill in _skillSlots)
        {
            skill.ResetCooldown();
        }
    }
}
