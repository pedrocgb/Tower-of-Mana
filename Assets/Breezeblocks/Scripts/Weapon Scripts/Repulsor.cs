using Breezeblocks.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName ="Repulsor", menuName ="Weapons/Repulsor")]
public class Repulsor : Weapon
{
    [FoldoutGroup("Repulsor Variables")]
    [SerializeField]
    private float _baseRadius = 0f;

    public override void Attack(Transform Pos, float ManaMultiplier)
    {
        Transform obj = ObjectPooler.SpawnFromPool(_attackPrefab, Pos.position, Quaternion.identity).transform;
        obj.transform.localScale = new Vector3(_baseRadius * ManaMultiplier, _baseRadius * ManaMultiplier, obj.position.z);
    }
}
