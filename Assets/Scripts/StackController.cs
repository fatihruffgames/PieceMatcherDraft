using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    public event System.Action StackPickedEvent;
    public event System.Action StackReleasedEvent;
    public event System.Action StackIsMovingEvent;

    [Header("Config")]
    public Vector3 overlapCenterOffset;
    public Vector3 overlapSize;
    public bool isInTopLayer;

    [Header("Debug")]
    [SerializeField] bool hasCards;
    public bool IsPicked;
    public bool IsActive;
    BoxCollider _collider => GetComponent<BoxCollider>();
    const float VERTICAL_PADDING = 0.1f;
    const int MAX_STACKABLE_COUNT = 10;
    float moveDuration = .75f;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = new Vector3(0, i * VERTICAL_PADDING, 0);
        }

        hasCards = true;
        float ySize = transform.childCount * VERTICAL_PADDING;
        _collider.size = new Vector3(1, ySize, 1);
        _collider.center = new Vector3(0, ySize / 2, 0);

    }

    private void Start()
    {
        if (isInTopLayer)
        {
            IsActive = true;
        }
        else
            InputManager.instance.CheckUpperCardEvent += CheckUpperStack;
    }

    private void CheckUpperStack()
    {
        if (transform.childCount == 0) return;

        IEnumerator Routine()
        {
            yield return null;
            yield return null;

            Vector3 center = transform.position + overlapCenterOffset;
            Quaternion orientation = Quaternion.identity;
            Collider[] colliders = Physics.OverlapBox(center, overlapSize / 2, orientation, InputManager.instance.stackLayer);

            List<StackController> overlappedStack = new();

            for (int i = 0; i < colliders.Length; i++)
            {
                StackController stack = colliders[i].transform.GetComponent<StackController>();
                if (stack != null && stack != this)
                    overlappedStack.Add(stack);

            }

            if (overlappedStack.Count == 0)
            {
                GetActivated();
            }

        }

        StartCoroutine(Routine());

    }

    public void MoveStack(StackController targetStack)
    {
        InputManager.instance.CheckUpperCardEvent -= CheckUpperStack;
        StackIsMovingEvent?.Invoke();

        InputManager.instance.ResetPickingParams();
        _collider.enabled = false;

        IEnumerator MoveRoutine()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform card = transform.GetChild(i);
                Vector3 placementPos = new Vector3(0, targetStack.transform.childCount * VERTICAL_PADDING, 0);
                card.SetParent(targetStack.transform);

                Vector3 direction = (placementPos - card.localPosition).normalized;
                if (placementPos.z != card.localPosition.z)
                    direction.x = 0;

                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.down);

                Sequence sequence = DOTween.Sequence();
                sequence.Append(card.DOLocalJump(placementPos, 5, 1, moveDuration));
                sequence.Insert(0, card.DOLocalRotateQuaternion(targetRotation, moveDuration - 0.5f)).OnComplete(() =>
                {
                    card.localRotation = Quaternion.identity; // Set rotation to zero
                });

                int index = i;

                if (index == 0)
                {
                    targetStack.CheckStackCount();
                    hasCards = false;
                }
                else
                    yield return new WaitForSeconds(.1f);
            }
        }

        StartCoroutine(MoveRoutine());

    }


    public void CheckStackCount()
    {
        _collider.enabled = false;

        if (transform.childCount >= MAX_STACKABLE_COUNT)
        {
            InputManager.instance.CheckUpperCardEvent -= CheckUpperStack;
            hasCards = false;

            transform.DOScale(Vector3.zero, 0.5f)
                .SetDelay(0.5f)
                .OnComplete(() =>
            {
                Destroy(gameObject, .1f);
            });
        }
        else
            _collider.enabled = true;

        InputManager.instance.TriggerCheckEvent();
    }


    void GetActivated()
    {
        IsActive = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CardController>().GetActivated();
        }
    }


    #region GETTERS

    public bool HasCards()
    {
        return hasCards;
    }

    public void GetPicked()
    {
        IsPicked = true;
        transform.DOMoveY(transform.position.y + 1f, 0.2f);
        StackPickedEvent?.Invoke();
    }
    public void GetReleased()
    {
        IsPicked = false;
        transform.DOMoveY(transform.position.y - 1f, 0.2f);
        StackReleasedEvent?.Invoke();
    }

    public ColorEnum GetStackColor()
    {
        return transform.GetChild(0).GetComponent<CardController>().color;
    }


    void OnDrawGizmosSelected()
    {
        if (transform.childCount == 0) return;

        Vector3 center = transform.position + overlapCenterOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, overlapSize);
    }
    #endregion
}
