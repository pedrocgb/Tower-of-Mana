using Breezeblocks.CharactersStats;
using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Breezeblocks.PlayerScripts
{
    public class PlayerBase : MonoBehaviour
    {
        [BoxGroup("Settings")]
        [SerializeField]
        private PlayerStats _stats = null;
        public PlayerStats Stats { get { return _stats; } }       

        [BoxGroup("Global Variables")]
        [SerializeField]
        [ReadOnly]
        private bool _isAttacking = false;
        public bool IsAttacking
        {
            get { return _isAttacking; }
            set { _isAttacking = value; }
        }

        private Player _input = null;
        public Player Input { get { return _input; } }

        private CharacterStat _maxMana;
        private CharacterStat _manaGain;
        private CharacterStat _manaRechargeRate;

        private float _manaGainTimer = 0f;
        private float _currentMana = 0f;
        private float CurrentManaPercentage { get { return (_currentMana / _maxMana.Value); } }

        [BoxGroup("Components")]
        [SerializeField]
        private Animator _myAnimator = null;
        public Animator MyAnimator => _myAnimator;
        [BoxGroup("Components")]
        [SerializeField]
        private Image _manaUI = null;

        // ----------------------------------------------------------------------

        private void Awake()
        {
            _input = ReInput.players.GetPlayer(0);
        }

        private void Start()
        {
            _maxMana = new CharacterStat(_stats.MaxMana);
            _manaGain = new CharacterStat(_stats.ManaGain);
            _manaRechargeRate = new CharacterStat(_stats.ManaRechargeRate);
        }

        // ----------------------------------------------------------------------

        private void Update()
        {
            RechargeMana();
        }

        // ----------------------------------------------------------------------

        private void RechargeMana()
        {
            if (_currentMana >= _maxMana.Value)
                return;            

            _manaGainTimer += Time.deltaTime;

            if (_manaGainTimer >= _manaRechargeRate.Value)
            {
                _currentMana += _manaGain.Value;
                _currentMana = Mathf.Min(_currentMana, _maxMana.Value);

                _manaGainTimer = 0f;
            }

            UpdateUI();
        }

        public void SpendMana(float amount)
        {
            _currentMana -= amount;

            if (_currentMana < 0)
                _currentMana = 0f;

            UpdateUI();
        }

        // ----------------------------------------------------------------------

        private void UpdateUI()
        {
            _manaUI.fillAmount = CurrentManaPercentage;
        }

        // ----------------------------------------------------------------------
    }
}