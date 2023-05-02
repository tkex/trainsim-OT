using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UltimateXR.Core;
using System.Linq;



public class FingerButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> _fingerObjects = new List<GameObject>();
    [SerializeField] private float _range = 0.0f;
    [SerializeField] private float _pushDownAmount = 0.1f;
    [SerializeField] private float _pushAnimationTime = 0.5f;

    private Vector3 _initialPosition;
    private bool _hasBeenPressed = false;
    private Vector3 _fingerPositionAtPress;
    private bool fingerIsTouching = false;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        // Check if any finger tips are in range
        foreach (GameObject fingerObject in _fingerObjects)
        {
            Collider[] colliders = Physics.OverlapSphere(fingerObject.transform.position, _range);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == gameObject)
                {
                    if (!_hasBeenPressed)
                    {
                        _hasBeenPressed = true;
                        // store finger position at touching the button
                        _fingerPositionAtPress = fingerObject.transform.position;
                        fingerIsTouching = true;
                    }

                    // calculate the y position difference between the finger and the button
                    float yDiff = fingerObject.transform.position.y - _fingerPositionAtPress.y;

                    // Move the button down by the difference in y position
                    transform.DOMoveY(transform.position.y + yDiff, 0.1f);
                }
                else
                {
                    if (!fingerIsTouching)
                    {
                        // Move the button back to its initial position if the finger is not touching
                        transform.DOMoveY(_initialPosition.y, _pushAnimationTime)
                            .SetEase(Ease.OutBounce);
                        _hasBeenPressed = false;
                    }
                }
            }
        }

        // Reset fingerIsTouching when all fingers are lifted off the button
        if (!_fingerObjects.Exists(fingerObject => Physics.OverlapSphere(fingerObject.transform.position, _range)
            .Any(collider => collider.gameObject == gameObject)))
        {
            fingerIsTouching = false;
        }
    }
}