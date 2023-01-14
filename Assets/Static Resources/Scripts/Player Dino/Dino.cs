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
        /*
         * Приймати інпут коли промінь граунд чеку вже торкнувся землі
         * коли гравець приземлився на землю одразу ж стрибнути в такому випадку
         * починати знову перевіряти торкання землі, коли промінь перестане торкатися землі
         */
        [SerializeField] private Transform _groundCheckOrigin;
        [SerializeField, Range(0.01f, 1f)] private float _groundCheckDistance;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField, Range(0.05f, .5f)] private float _checkDelay;

        private Rigidbody2D _rigidbody;
        private readonly Vector2 _jumpDirection = new (0, 1);
        private Vector2 _jump;

        private bool _jumped;
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
            yield return new WaitForSeconds(_checkDelay);
            _groundCheckDelayEnded = true;
        }

        private bool CanGroundCheck()
        {
            return _groundCheckDelayEnded && _grounded == false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var groundCheckPosition = _groundCheckOrigin != null ? _groundCheckOrigin.position : default;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(groundCheckPosition, groundCheckPosition + Vector3.down * _groundCheckDistance);
            Gizmos.color = Color.white;
        }
#endif
    }
}