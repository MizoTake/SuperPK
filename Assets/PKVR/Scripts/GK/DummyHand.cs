using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

// [RequireComponent (typeof (Rigidbody))]
public class DummyHand : MonoBehaviour
{
    [Inject]
    private InputGoController _controller;

    [SerializeField]
    private Hand _hand;

    private Vector3 _nextPos;
    private Rigidbody _rigid;

    // Use this for initialization
    void Start ()
    {
        // _rigid = GetComponent<Rigidbody> ();

        Bind ();
    }

    private void Bind ()
    {
        var vec = Vector3.forward;
        var target = Vector3.zero;
        _controller.ovrTouchpad
            .Subscribe (_ =>
            {
                // target = transform.forward * Mathf.Clamp (target.z + _, transform.parent.localPosition.z, target.z + _);
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

        var range = transform.localScale.x;
        this.UpdateAsObservable ()
            .Where (_ => transform.position.z <= transform.parent.position.z + range &&
                transform.position.z >= transform.parent.position.z - range)
            .Subscribe (_ =>
            {
                transform.SetPositionAndRotation (new Vector3 (_hand.transform.position.x * -1f, _hand.transform.position.y, _hand.transform.position.z), _hand.transform.rotation);
            })
            .AddTo (this);
    }
}