using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class StageWolfRock : MonoBehaviour
{
    [Header("깨질 수 있음 상태")]
    [SerializeField] private bool _Isbreaked;

    [Header("돌을 밟을 수 있는 시간")]
    [SerializeField] private float _StandableTime;

    private MeshCollider _MeshCollider;

    private MeshRenderer _MeshRenderer;


    private void Start()
    {
        _MeshCollider = GetComponent<MeshCollider>();
        _MeshRenderer = GetComponent<MeshRenderer>();
    }

    // 플레이어가 돌을 밟을 때
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("asdf");
        // 밟은 오브젝트가 플레이어가 아닐시 실행하지 않습니다.
        if (other.gameObject.layer != LayerMask.NameToLayer("PlayerInteractionArea")) return;

        // 깨질 수 없는 돌이면 실행하지 않습니다.
        if (!_Isbreaked) return;


        StopCoroutine(BreakRock());
        StartCoroutine(BreakRock());

    }

    // 돌이 깨졌다가 다시 생깁니다.
    IEnumerator BreakRock()
    {
        yield return new WaitForSeconds(_StandableTime);

        _MeshCollider.enabled = false;
        _MeshRenderer.enabled = false;

        yield return new WaitForSeconds(2);

        _MeshCollider.enabled = true;
        _MeshRenderer.enabled = true;
        
    }
}
