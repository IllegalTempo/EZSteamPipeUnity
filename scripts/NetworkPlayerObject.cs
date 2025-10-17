using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerObject : MonoBehaviour
{

    [Header("Network Data")]
    public ulong steamID;
    public bool IsLocal;
    public int NetworkID;

    public Vector3 NetworkPos;
    public Quaternion NetworkHeadRot;
    public Quaternion NetworkBodyrot;
    public float NetworkAnimationX;
    public float NetworkAnimationY;
    [Header("Network Setting")]
    public bool Sync_Pos = true;
    public bool Sync_HeadRot = true;
    public bool Sync_BodyRot = true;

    [Header("Object References")]
    private GameObject Head;
    private GameObject Body;
    public void Disconnect()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        if(!IsLocal)
        {
            if (Sync_Pos)
            {
                transform.position = Vector3.Lerp(transform.position, NetworkPos, Time.deltaTime * 10);
            }
            if (Sync_HeadRot)
            {
                Head.transform.rotation = Quaternion.Slerp(Head.transform.rotation, NetworkHeadRot, Time.deltaTime * 10);
            }
            if (Sync_BodyRot)
            {
                Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, NetworkBodyrot, Time.deltaTime * 10);
            }
        }
    }
    private void FixedUpdate()
    {
        if (IsLocal)
        {
            PacketSend.Client_Send_Position(transform.position,Head.transform.rotation,Body.transform.rotation);

        }
    }
    public void SetMovement(Vector3 pos, Quaternion Headrot, Quaternion bodyrot)
    {
        NetworkPos = pos;
        NetworkHeadRot = Headrot;
        NetworkBodyrot = bodyrot;
    }
    public void SetAnimation(float x, float y)
    {
        NetworkAnimationX = x;
        NetworkAnimationY = y;
    }

}
