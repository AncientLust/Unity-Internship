// This is your ScriptableObject class
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/Character")]
public class CharacterConfig : ScriptableObject
{
    public float health;
    public float speed;
    public float attackPower;
}