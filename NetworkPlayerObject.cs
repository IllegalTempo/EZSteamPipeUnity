using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerObject : MonoBehaviour
{
    public int NetworkID;
    public ulong steamID;
    public bool IsLocal;
    public Vector3 NetworkPos;
    public Quaternion NetworkRot;
    public Quaternion NetworkBodyrot;
    public float NetworkAnimationX;
    public float NetworkAnimationY;
    public float NetworkAnimationZ;
    public void Disconnect()
    {
        Destroy(gameObject);
    }
    public void SetMovement(Vector3 pos, Quaternion Headrot, Quaternion bodyrot)
    {
        NetworkPos = pos;
        NetworkRot = Headrot;
        NetworkBodyrot = bodyrot;
    }
    public void SetAnimation(float x, float y)
    {
        NetworkAnimationX = x;
        NetworkAnimationY = y;
    }

}
