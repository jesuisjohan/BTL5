using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Simple server-authoritative example.
/// Also shows reactive checks instead of per-frame checks
/// If you have questions, pop into discord and have a chat https://discord.gg/tarodev
/// </summary>
public class PlayerColor : NetworkBehaviour {
    private readonly NetworkVariable<Color> _netColor = new();
    private readonly Color[] _colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white, Color.magenta, Color.gray };
    private int _index;

    [SerializeField] private MeshRenderer _renderer;

    private void Awake() {
        _netColor.OnValueChanged += OnValueChanged;
    }

    public override void OnDestroy() {
        _netColor.OnValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(Color prev, Color next) {
        _renderer.material.color = next;
    }

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            _index = (int)OwnerClientId;
            CommitNetworkColorServerRpc(GetNextColor());
        }
        else {
            _renderer.material.color = _netColor.Value;
        }
    }

    [ServerRpc]
    private void CommitNetworkColorServerRpc(Color color) {
        _netColor.Value = color;
    }

    private void OnTriggerEnter(Collider other) {
        if (!IsOwner) return;
        CommitNetworkColorServerRpc(GetNextColor());
    }

    private Color GetNextColor() {
        return _colors[_index++ % _colors.Length];
    }
}