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
        Bind();
    }

    protected void Bind()
    {
        var vec = Vector3.forward;
        _controller.ovrTouchpad
            .Subscribe(_ =>
            {
                var value = (Mathf.Clamp(transform.position.z + _, transform.parent.position.z, transform.position.z + _) == transform.parent.position.z) ? 0f : _;
                transform.Translate(vec * value);
            })
            .AddTo(this);
    }
}
