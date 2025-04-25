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

        [FoldoutGroup("Attack Variables")]
        [SerializeField]
        private string _attackPrefab = string.Empty;
        [FoldoutGroup("Attack Variables")]
        [SerializeField]
        private float _attackDelay = 0.25f;

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
                StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            _player.IsAttacking = true;
            _attackTimeStamp = Time.time + _player.Stats.AttackSpeed;
            ObjectPooler.SpawnFromPool(_attackPrefab, transform.position, Quaternion.identity);

            _player.MyAnimator.SetTrigger("Attack");

            _player.SpendMana(25f);

           yield return new WaitForSeconds(_attackDelay);

            _player.IsAttacking = false;
        }
        #endregion

        // ----------------------------------------------------------------------
    }
}