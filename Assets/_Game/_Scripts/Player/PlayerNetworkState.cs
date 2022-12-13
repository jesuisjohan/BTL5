using UnityEngine;
using Unity.Netcode;

struct PlayerNetworkState : INetworkSerializable
{
    private float _positionX, _positionZ;
    private short _rotationY;

    internal Vector3 Position
    {
        get => new(_positionX, 0, _positionZ);
        set
        {
            _positionX = value.x;
            _positionZ = value.z;
        }
    }

    internal Vector3 Rotation
    {
        get => new(0, _rotationY, 0);
        set => _rotationY = (short)value.y;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _positionX);
        serializer.SerializeValue(ref _positionZ);

        serializer.SerializeValue(ref _rotationY);
    }
}