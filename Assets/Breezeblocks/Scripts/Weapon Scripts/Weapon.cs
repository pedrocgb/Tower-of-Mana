using Breezeblocks.PlayerScripts;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [FoldoutGroup("Weapon Variables")]
    [SerializeField]
    protected float _baseAttackRate = 0f;
    public float AttackRate => _baseAttackRate;
    [FoldoutGroup("Weapon Variables")]
    [SerializeField]
    protected float _attackManaMultiplier = 0f;
    public float AttackManaMultiplier => _attackManaMultiplier;
    [FoldoutGroup("Weapon Variables")]
    [SerializeField]
    protected string _attackPrefab = string.Empty;


    public virtual void Attack(Transform Pos, float ManaPower)
    {

    }
}
