using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public sealed class StageWolfRock : MonoBehaviour
{
    [Header("돌의 속도")]
    [SerializeField] private float _RockSpeed;

    [Header("깨질 수 있음 상태")]
    [SerializeField] private bool _Isbreaked;

    [Header("돌을 밟을 수 있는 시간")]
    [SerializeField] private float _StandableTime;

    
    // 플레이어가 돌을 밟을 때
    private void OnTriggerEnter(Collider other)
    {
        // 밟은 오브젝트가 플레이어가 아닐시 실행하지 않습니다.
        if (other.gameObject.layer != LayerMask.NameToLayer("PlayerInteractionArea")) return;

        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 5.0f, ForceMode.Impulse);

        // 깨질 수 없는 돌이면 실행하지 않습니다.
        if (!_Isbreaked) return;


        StopCoroutine(BreakRock());
        StartCoroutine(BreakRock());

    }

    IEnumerator BreakRock()
    {
        yield return new WaitForSeconds(_StandableTime);

        gameObject.SetActive(false);
    }
}
