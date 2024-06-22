using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Dino : MonoBehaviour
    {
        [SerializeField] private float _jumpForce;
        [SerializeField] private KeyCode _jumpKey;
        [Space(20)]
        [Header("Ground Check Parameters")]
        [SerializeField] private Transform _groundCheckOrigin;
        [SerializeField, Range(.01f, 1f)] private float _groundCheckDistance;
        [SerializeField, Range(.05f, .5f)] private float _groundCheckDelayAfterJump;
        [SerializeField, Range(.05f, .2f)] private float _groundTriggerRadius;
        [SerializeField] private LayerMask _layerMask;

        private Rigidbody2D _rigidbody;
        private readonly Vector2 _jumpDirection = new (0, 1);
        private Vector2 _jump;

        private bool _jumped;
        private bool _pressedJumpInAir;
        
        private bool _grounded;
        private bool _groundCheckDelayEnded;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _jump = _jumpDirection * _jumpForce;
        }

        private void Update()
        {
            if (CanGroundCheck())
            {
                if (IsGrounded())
                {
                    _jumped = false;
                }
            }

            if (CanReceiveJumpInput())
            {
                _pressedJumpInAir = Input.GetKeyDown(_jumpKey);
            }

            if (_pressedJumpInAir)
            {
                JumpAfterGrounding();
            }

            if (CanJump())
            {
                Jump();
            }
        }

        private void Jump()
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
            _rigidbody.AddRelativeForce(_jump, ForceMode2D.Impulse);
            _jumped = true;
            _grounded = false;
            StartCoroutine(GroundCheckDelay());
        }

        private bool IsGrounded()
        {
            var hitResults = new RaycastHit2D[1];
            var results = Physics2D.RaycastNonAlloc(_groundCheckOrigin.position,
                Vector2.down, hitResults, _groundCheckDistance,
                _layerMask);

            _grounded = results > 0;

            return _grounded;
        }

        private bool CanJump()
        {
            return _jumped == false && Input.GetKeyDown(_jumpKey);
        }

        private IEnumerator GroundCheckDelay()
        {
            _groundCheckDelayEnded = false;
            yield return new WaitForSeconds(_groundCheckDelayAfterJump);
            _groundCheckDelayEnded = true;
        }

        private bool CanGroundCheck()
        {
            return _groundCheckDelayEnded && _grounded == false;
        }

        private bool CanReceiveJumpInput()
        {
            return _pressedJumpInAir == false && CanGroundCheck();
        }

        private void JumpAfterGrounding()
        {
            var groundResults = new Collider2D[1];
            var results = Physics2D.OverlapCircleNonAlloc(_groundCheckOrigin.position,
                _groundTriggerRadius,
                groundResults,
                _layerMask);
                
            if (results > 0)
            {
                Jump();
                _pressedJumpInAir = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var groundCheckPosition = _groundCheckOrigin != null ? _groundCheckOrigin.position : default;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(groundCheckPosition, groundCheckPosition + Vector3.down * _groundCheckDistance);
            Gizmos.DrawWireSphere(groundCheckPosition, _groundTriggerRadius);
            Gizmos.color = Color.white;
        }
#endif
    }
}