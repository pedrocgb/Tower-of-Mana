using Breezeblocks.CharactersStats;
using Breezeblocks.Managers;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Breezeblocks.PlayerScripts
{
    [RequireComponent(typeof(PlayerBase))]
    public class PlayerActions : MonoBehaviour
    {
        #region Variables and Properties
        private PlayerBase _player = null;
        private CharacterStat _attackSpeed;

        [BoxGroup("Weapon")]
        [SerializeField]
        private Weapon _equippedWeapon = null;

        private float _attackTimeStamp = 0f;
        #endregion

        // ----------------------------------------------------------------------

        #region Initializers
        private void Awake()
        {
            _player = GetComponent<PlayerBase>();
        }
        #endregion


        // ----------------------------------------------------------------------

        #region Loops
        private void Update()
        {
            HandleInputs(); 
        }
        #endregion

        // ----------------------------------------------------------------------

        #region Actions
        private void HandleInputs()
        {
            if (_player.Input.GetButtonDown("Attack") && _attackTimeStamp <= Time.time && _player.IsAttacking == false)
                _equippedWeapon.Attack(transform, _player.GetManaPower(_equippedWeapon.AttackManaMultiplier));
        }
        #endregion

        // ----------------------------------------------------------------------
    }
}