using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Stats", menuName = "Breezeblocks/Player/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [BoxGroup("Mana")]
    [SerializeField]
    private float _baseMaxMana = 0f;
    public float MaxMana => _baseMaxMana;
    [BoxGroup("Mana")]
    [SerializeField]
    private float _baseManaGain = 0f;
    public float ManaGain => _baseManaGain;
    [BoxGroup("Mana")]
    [SerializeField]
    private float _baseManaRechargeRate = 0f;
    public float ManaRechargeRate => _baseManaRechargeRate;

    // ----------------------------------------------------------------------

    [BoxGroup("Movement")]
    [SerializeField]
    private float _baseMoveSpeed = 0f;
    public float MoveSpeed => _baseMoveSpeed;

    // ----------------------------------------------------------------------

    [BoxGroup("Jumping")]
    [SerializeField]
    private float _baseJumpForce = 0f;
    public float JumpForce => _baseJumpForce;
    [BoxGroup("Jumping")]
    [SerializeField]
    private float _jumpManaMultiplier = 0f;
    public float JumpManaMultiplier => _jumpManaMultiplier;

    // ----------------------------------------------------------------------

    [BoxGroup("Dash")]
    [SerializeField]
    private float _baseDashForce = 0f;
    public float DashForce => _baseDashForce;
    [BoxGroup("Dash")]
    [SerializeField]
    private float _baseDashCooldown = 0f;
    public float DashCooldown => _baseDashCooldown;
    [BoxGroup("Dash")]
    [SerializeField]
    private float _dashManaMultiplier = 0f;
    public float DashManaMultiplier => _dashManaMultiplier;

    // ----------------------------------------------------------------------

    [BoxGroup("Attack")]
    [SerializeField]
    private float _baseAttackSpeed = 0f;
    public float AttackSpeed => _baseAttackSpeed;
    [BoxGroup("Attack")]
    [SerializeField]
    private float _attackManaMultiplier = 0f;
    public float AttackManaMultiplier => _attackManaMultiplier;

    // ----------------------------------------------------------------------
}
