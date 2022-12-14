using Unity.Netcode;
using UnityEngine;

public class PlayerTransform : NetworkBehaviour
{
    [SerializeField] private bool _serverAuth;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;

    private NetworkVariable<PlayerNetworkState> _playerState;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;

        _playerState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
    }

    public override void OnNetworkSpawn()
    {
        if (SingleModeManager.Instance.isPlaying)
        {
            var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
            var state = new PlayerNetworkState();
            if (GetComponent<Boss>())
            {
                Debug.Log("Is boss");
                state.Position = SingleModeManager.Instance.bossSpawner.position;
                _playerState = new NetworkVariable<PlayerNetworkState>(state, writePerm: permission);
            }
            else
            {
                Debug.Log("Is Player");
                state.Position = SingleModeManager.Instance.playerSpawner.position;
                _playerState = new NetworkVariable<PlayerNetworkState>(state, writePerm: permission);
            }
        }

        if (IsOwner || !GetComponent<Boss>()) return;
        Debug.Log("Destroy Controller");
        Destroy(transform.GetComponent<PlayerController>());
    }

    private void Update()
    {
        if (SingleModeManager.Instance.isPlaying)
        {
            TransmitState();
            ConsumeState();
        }
        else
        {
            if (IsOwner)
            {
                TransmitState();
            }
            else
            {
                ConsumeState();
            }
        }
    }

    #region Transmit State

    private void TransmitState()
    {
        var state = new PlayerNetworkState
        {
            Position = _rb.position,
            Rotation = transform.rotation.eulerAngles
        };

        if (IsServer || !_serverAuth)
        {
            _playerState.Value = state;
        }
        else
        {
            TransmitStateServerRpc(state);
        }
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerNetworkState state)
    {
        _playerState.Value = state;
    }

    #endregion

    #region Interpolate State

    private Vector3 _positionVelocity;
    private float _rotationVelocityY;

    private void ConsumeState()
    {
        var position = Vector3.SmoothDamp(_rb.position,
            _playerState.Value.Position,
            ref _positionVelocity,
            _cheapInterpolationTime);

        _rb.MovePosition(position);

        var angle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y,
            _playerState.Value.Rotation.y,
            ref _rotationVelocityY,
            _cheapInterpolationTime);

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    #endregion
}