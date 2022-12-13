using UnityEngine;

public class BossController : MonoBehaviour
{
    public bool IsMoving = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = Camera.main;
    }

    private void Update()
    {
        var direction = IsMoving ? Random.Range(0, 4) : 4;

        switch (direction)
        {
            case 0:
            {
                _input = new Vector3(0, 0, -1);
                break;
            }
            case 1:
            {
                _input = new Vector3(0, 0, 1);
                break;
            }
            case 2:
            {
                _input = new Vector3(-1, 0, 0);
                break;
            }
            case 3:
            {
                _input = new Vector3(1, 0, 0);
                break;
            }
            default:
            {
                _input = new Vector3(0, 0, 0);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    #region Movement

    [SerializeField] private float _acceleration = 80;
    [SerializeField] private float _maxVelocity = 10;
    private Vector3 _input;
    private Rigidbody _rb;

    private void HandleMovement()
    {
        _rb.velocity += _input.normalized * (_acceleration * Time.deltaTime);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);
    }

    #endregion

    #region Rotation

    [SerializeField] private float _rotationSpeed = 450;
    private Plane _groundPlane = new(Vector3.up, Vector3.zero);
    private Camera _cam;

    private void HandleRotation()
    {
        var hitPoint = SingleModeManager.Instance.player.transform.position;
        var dir = hitPoint - transform.position;
        var rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _rotationSpeed * Time.deltaTime);
    }

    #endregion
}