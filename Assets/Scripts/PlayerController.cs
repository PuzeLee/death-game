using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpPower = 8f;
    [SerializeField] private LayerMask _groundLayer;


    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private float _horizontalInput;
    private bool _isDied;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (!_isDied)
        {
            // Move
            _horizontalInput = Input.GetAxis("Horizontal");
            _rb.linearVelocityX = _horizontalInput * _speed;

            // Flip
            if (_horizontalInput > 0.01f)
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            else if (_horizontalInput < -0.01f)
                transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);

            // Jump
            if (Input.GetKey(KeyCode.Space) && IsGrounded())
                Jump();

            // Die
            if (Input.GetKey(KeyCode.A))
                Die();

            // Set animator parameters
            _animator.SetBool("isRun", _horizontalInput != 0);
            _animator.SetBool("isJump", !IsGrounded());
        }
    }

    private void Jump()
    {
        _rb.linearVelocityY = _jumpPower;
        _animator.SetTrigger("jump");

    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f, _groundLayer);
        return raycastHit.collider != null;
    }

    private void Die()
    {
        _animator.SetTrigger("die");
        _isDied = true;
        _rb.linearVelocityX = 0;
    }
}
