using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    public XRNode inputSource;
    public float gravity = -9.81f;
    public LayerMask groundLayer;

    private XROrigin rig;
    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XROrigin>();
    }
    

}