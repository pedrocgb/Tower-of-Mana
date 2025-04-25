using Breezeblocks.CharactersStats;
using Rewired;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Breezeblocks.PlayerScripts
{
    [RequireComponent(typeof(PlayerBase))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Variables and Properties
        private PlayerBase _player = null;

        private CharacterStat _moveSpeed;
        private CharacterStat _jumpForce;
        private CharacterStat _dashForce;
        private CharacterStat _dashCooldown;

        [FoldoutGroup("Jump")]
        [SerializeField]
        private float _coyoteTime = 0.2f;
        [FoldoutGroup("Jump")]
        [SerializeField]
        private float _fallMultiplier = 2.5f;
        [FoldoutGroup("Jump")]
        [SerializeField]
        private float _lowJumpMultiplier = 5f;
        [FoldoutGroup("Jump")]
        [SerializeField]
        private float _jumpHoldGravityModifier = 1f;
        private bool _isHoldingJump;
        private int _jumpsRemaining;
        private int _maxJumps = 1;


        [FoldoutGroup("Dash")]
        [SerializeField]
        private float _dashDuration = 0.2f;
        private KeyCode _dashKey = KeyCode.LeftShift;

        [FoldoutGroup("Ground Check")]
        [SerializeField]
        private Transform _groundCheck;
        [FoldoutGroup("Ground Check")]
        [SerializeField]
        private float _groundCheckRadius = 0.2f;
        [FoldoutGroup("Ground Check")]
        [SerializeField]
        private LayerMask _groundLayer;

        private Rigidbody2D _rb;
        private float _moveInput = 0f;
        private float _coyoteTimeCounter;
        private bool _jumpPressed;
        private bool _isGrounded;

        private Vector2 _lastMoveDir = Vector2.right;
        private bool _isDashing;
        private float _dashCooldownTimer;
        #endregion

        // ----------------------------------------------------------------------

        #region Initializer
        private void Awake()
        {
            _player = GetComponent<PlayerBase>();
        }
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>(); 
            _rb.gravityScale = 1f;

            _moveSpeed = new CharacterStat(_player.Stats.MoveSpeed);
            _jumpForce = new CharacterStat(_player.Stats.JumpForce);
            _dashForce = new CharacterStat(_player.Stats.DashForce);
            _dashCooldown = new CharacterStat(_player.Stats.DashCooldown);
        }
        #endregion

        // ----------------------------------------------------------------------

        #region Loops
        void Update()
        {
            if (!_isDashing)
            {
                HandleInput();
                HandleMovement();
                HandleJump();
                HandleGravityModifiers();
            }

            HandleDash();
        }
        #endregion

        // ----------------------------------------------------------------------

        private void HandleInput()
        {
            _moveInput = _player.Input.GetAxisRaw("Horizontal Axis");

            if (_moveInput != 0)
                _lastMoveDir = new Vector2(_moveInput, 0).normalized;

            _jumpPressed = _player.Input.GetButtonDown("Jump");
            _isHoldingJump = _player.Input.GetButton("Jump");
        }

        private void HandleMovement()
        {
            SetLinearVelocity(new Vector2(_moveInput * _moveSpeed.Value, _rb.linearVelocity.y));

            Flip(_moveInput); // Flip sprite based on direction
            _player.MyAnimator.SetFloat("Speed", Mathf.Abs(_rb.linearVelocity.x));
        }

        private void Flip(float moveInput)
        {
            if (moveInput != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(moveInput) * Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }

        // ----------------------------------------------------------------------

        private void HandleJump()
        {
            UpdateGroundCheck();

            if (_isGrounded)
            {
                _coyoteTimeCounter = _coyoteTime;
                _jumpsRemaining = _maxJumps; // Reset jumps when grounded
            }
            else
            {
                _coyoteTimeCounter -= Time.deltaTime;
            }

            if (_jumpPressed)
            {
                if (_coyoteTimeCounter > 0f || _jumpsRemaining > 0)
                {
                    SetLinearVelocity(new Vector2(_rb.linearVelocity.x, _jumpForce.Value * _player.GetManaPower(_player.Stats.JumpManaMultiplier)));
                    _coyoteTimeCounter = 0f;
                    _jumpsRemaining--; // Consume a jump
                    
                }
            }

            _jumpPressed = false;
        }

        private void HandleGravityModifiers()
        {
            if (_rb.linearVelocity.y < 0)
            {
                // Falling
                SetLinearVelocity(_rb.linearVelocity + Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1f) * Time.deltaTime);
            }
            else if (_rb.linearVelocity.y > 0)
            {
                // Going up
                if (_isHoldingJump)
                {
                    // Holding = float a bit longer (very slight effect)
                    SetLinearVelocity(_rb.linearVelocity + Vector2.up * Physics2D.gravity.y * (_jumpHoldGravityModifier - 1f) * Time.deltaTime);
                }
                else
                {
                    // Let go = cut jump short
                    SetLinearVelocity(_rb.linearVelocity + Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1f) * Time.deltaTime);
                }
            }
        }

        private void UpdateGroundCheck()
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
            _player.MyAnimator.SetBool("isGrounded", _isGrounded);
        }

        // ----------------------------------------------------------------------

        private void HandleDash()
        {
            _dashCooldownTimer -= Time.deltaTime;

            if (_player.Input.GetButtonDown("Dash") && _dashCooldownTimer <= 0 && !_isDashing)
            {
                StartCoroutine(DoDash());
            }
        }

        private IEnumerator DoDash()
        {
            _isDashing = true;
            _dashCooldownTimer = _dashCooldown.Value;

            float originalGravity = _rb.gravityScale;
            _rb.gravityScale = 0f;

            SetLinearVelocity(_lastMoveDir * _dashForce.Value * _player.GetManaPower(_player.Stats.DashManaMultiplier));

            yield return new WaitForSeconds(_dashDuration);

            _rb.gravityScale = originalGravity;
            _isDashing = false;
        }

        // ----------------------------------------------------------------------

        private void SetLinearVelocity(Vector2 velocity)
        {
            _rb.linearVelocity = velocity;
        }

        void OnDrawGizmosSelected()
        {
            if (_groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
            }
        }

        // ----------------------------------------------------------------------
    }
}