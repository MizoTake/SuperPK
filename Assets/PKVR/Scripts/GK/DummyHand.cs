using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

public class DummyHand : MonoBehaviour
{
    [Inject]
    private InputGoController _controller;

    [SerializeField]
    private Hand _hand;

    // Use this for initialization
    void Start()
    {
        Bind();
    }

    private void Bind()
    {
        var vec = Vector3.forward;
        _controller.ovrTouchpad
            .Subscribe(_ =>
            {
                var value = (Mathf.Clamp(transform.position.z + _, transform.parent.position.z, transform.position.z + _) == transform.parent.position.z) ? 0f : _;
                transform.Translate(vec * value);
            })
            .AddTo(this);

        var range = transform.localScale.x;
        this.UpdateAsObservable()
            .Where(_ => transform.position.z <= transform.parent.position.z + range
                    && transform.position.z >= transform.parent.position.z - range)
            .Subscribe(_ =>
            {
                transform.SetPositionAndRotation(new Vector3(_hand.transform.position.x * -1f, _hand.transform.position.y, _hand.transform.position.z), _hand.transform.rotation);
            })
            .AddTo(this);
    }
}
