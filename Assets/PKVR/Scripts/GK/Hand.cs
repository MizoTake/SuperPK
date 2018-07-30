using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

// [RequireComponent (typeof (Rigidbody))]
public class Hand : MonoBehaviour
{
    [Inject]
    private InputGoController _controller;

    private Vector3 _nextPos;
    private Rigidbody _rigid;

    // Use this for initialization
    void Start ()
    {
        // _rigid = GetComponent<Rigidbody> ();

        Bind ();
    }

    protected void Bind ()
    {
        var vec = Vector3.forward;
        var target = Vector3.zero;
        _controller.ovrTouchpad
            .Subscribe (_ =>
            {
                target = transform.forward * Mathf.Clamp (target.z + _, transform.parent.localPosition.z, target.z + _);
                var value = (Mathf.Clamp (transform.position.z + _, transform.parent.position.z, transform.position.z + _) == transform.parent.position.z) ? 0f : _;
                transform.Translate (vec * value);
                // transform.forward = transform.parent.forward;
                // _nextPos = transform.parent.position + target;
            })
            .AddTo (this);

        this.FixedUpdateAsObservable ()
            .Subscribe (_ =>
            {
                // _rigid.MovePosition (_nextPos);
            })
            .AddTo (this);
    }
}