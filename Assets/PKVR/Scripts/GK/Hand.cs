using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class Hand : MonoBehaviour
{
    [Inject]
    private InputGoController _controller;

    // Use this for initialization
    void Start()
    {
        var vec = Vector3.forward;
        var initZ = transform.position.z;
        _controller.ovrTouchpad
            .Subscribe(_ =>
            {
                var value = (Mathf.Clamp(transform.position.z + _, initZ, transform.position.z + _) == initZ) ? 0f : _;
                transform.Translate(vec * value);
            })
            .AddTo(this);
    }
}
