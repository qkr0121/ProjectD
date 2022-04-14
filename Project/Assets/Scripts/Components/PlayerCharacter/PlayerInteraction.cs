using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 가능 레이어")]
    [SerializeField] private LayerMask _InteractableLayer;

    [Header("상호작용 가능 오브젝트")]
    [SerializeField] private GameObject _InteractableObject;

    // PlayerCharacter 컴포넌트를 나타냅니다.
    private PlayerCharacter _PlayerCharacter;

    // 상호작용 설명 UI 를 나타냅니다.
    private GameObject _InteractionUI;

    // 상호작용 가능 상태를 나타냅니다
    public bool isInteracting { get; private set; }

    // 상호작용 키 사용 상태를 나타냅니다.
    public bool useInteractionKey { get; set; }

    private void Awake()
    {
        _PlayerCharacter = PlayerManager.Instance.playerCharacter;
        
    }

    private void Update()
    {
        // 상호작용 키를 눌렀을 경우
        if(Input.GetKeyDown(KeyCode.E))
        {
            TryInteraction();
        }
        
    }

    // 상호작용 가능한 오브젝트인지 확인합니다.
    private bool IsInteractableObject(GameObject gameObject)
    {
        return ((1 << gameObject.layer) & _InteractableLayer) != 0;
    }

    // 상호작용 가능 오브젝트를 찾습니다.
    private void OnTriggerEnter(Collider other)
    {
        // 겹친 오브젝트가 상호작용 가능 오브젝트가 아니라면 실행하지 않습니다.
        if (!IsInteractableObject(other.gameObject)) return;

        _InteractableObject = other.gameObject;

        Interact interactObject = other.GetComponent<Interact>();

        useInteractionKey = interactObject.UseInteractionKey();

        StartCoroutine(ShowInteractableObjectName(other.gameObject));
    }

    // 빠져 나갔을 경우 상호작용을 실행하고
    // 상호작용 오브젝트를 초기화합니다.
    private void OnTriggerExit(Collider other)
    {
        // 상호작용 키를 사용하지 않을경우
        if (useInteractionKey != true)
            // 상호작용을 실행합니다.
            TryInteraction();


        StopCoroutine(ShowInteractableObjectName(other.gameObject));
        Destroy(_InteractionUI);

        _InteractableObject = null;
    }

    // 상호작용 가능한 오브젝트 이름을 화면에 표시합니다
    private IEnumerator ShowInteractableObjectName(GameObject other)
    {
        _InteractionUI = Instantiate(
            ResourceManager.Instance.LoadResource<GameObject>("InteractionUI", "Prefabs/UI/Text(Interact)"),
            PlayerManager.Instance.gameUI.interactUI.transform, true);

        RectTransform rectTransform = _InteractionUI.GetComponent<RectTransform>();

        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;

        Text text = _InteractionUI.GetComponent<Text>();

        text.text ="E\n" + other.GetComponent<Interact>().interactionDetail;

        yield return null;
    }

    // 상호작용을 시도합니다.
    private void TryInteraction()
    {
        // 상호작용중 이라면 실행하지 않습니다.
        if (isInteracting) return;

        // 상호작용 가능 오브젝트가 존재하지 않는다면 실행하지 않습니다.
        if (_InteractableObject == null) return;

        Interact interactObject = _InteractableObject.GetComponent<Interact>();

        isInteracting = true;

        // 상호작용을 하고 끝 이벤트를 정의합니다.
        interactObject.StartInteraction(
            finishInteractionEvent : () =>
           {
               isInteracting = false;

               _PlayerCharacter.playerMovement.AllowMove();
               _PlayerCharacter.trackingCamera.InitCameraPos();
           });

        _PlayerCharacter.playerMovement.StopMove();
    }
}
