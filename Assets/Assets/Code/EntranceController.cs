using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntranceController : MonoBehaviour
{
    public GameObject entranceGo;
   
    public float delay = 1f;
    public float duration = 3f;
    public float distance = 3f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        originalPosition = entranceGo.transform.position;
        targetPosition = originalPosition + Vector3.up * distance;

        entranceGo.transform.DOMove(targetPosition, duration)
            .SetDelay(delay)
            .SetEase(Ease.OutQuad);
    }

    public void OpenDoor()
    {
        entranceGo.transform.DOMove(targetPosition, duration)
            .SetEase(Ease.OutQuad);
    }

    public void CloseDoor()
    {
        entranceGo.transform.DOMove(originalPosition, duration)
            .SetEase(Ease.OutQuad);
    }
}
