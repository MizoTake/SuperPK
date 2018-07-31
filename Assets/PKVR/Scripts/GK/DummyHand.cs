using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace PKVR
{
    public class DummyHand : HandAbstract
    {
        [Inject]
        private InputGoController _controller;

        [SerializeField]
        private Hand _hand;

        // Use this for initialization
        void Start () => Bind ();

        private void Bind ()
        {
            HandBind (_controller.ovrTouchpad);

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
}