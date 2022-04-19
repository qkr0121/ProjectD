using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private GameObject _PlayerMesh;
    [SerializeField] private TrackingCamera _TrackingCamera;
    [SerializeField] private CameraAnim _CameraAnim;
    [SerializeField] private PlayerInteraction _PlayerInteraction;
    [SerializeField] private PlayerCharacterAnim _PlayerAnim;
    [SerializeField] private CaveStage _CaveStage;
    private PlayerMovement _PlayerMovement;

    public GameObject playerMesh => _PlayerMesh;
    public TrackingCamera trackingCamera => _TrackingCamera;
    public CameraAnim cameraAnim => _CameraAnim;
    public PlayerMovement playerMovement => _PlayerMovement;
    public PlayerInteraction playerInteraction => _PlayerInteraction;
    public PlayerCharacterAnim playerAnim => _PlayerAnim;
    public CaveStage caveStage => _CaveStage;

    // 리스폰 위치를 나타냅니다.
    public Vector3 respawnPosition { get; set; }

    private void Start()
    {
        _PlayerMovement = GetComponent<PlayerMovement>();

    }

    //private void Update()
    //{
    //    playerAnim.SetAnimVelocity(playerMovement.velocity);
    //}
}
