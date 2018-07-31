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

        private const float ANIM_TIME = 0.01f;

        // Use this for initialization
        void Start () => Bind ();

        private void Bind ()
        {
            base.HandBind (_controller.ovrTouchpad);

            var range = transform.localScale.x;
            this.UpdateAsObservable ()
                .Where (_ => transform.position.z <= transform.parent.position.z + range &&
                    transform.position.z >= transform.parent.position.z - range)
                .Subscribe (_ =>
                {
                    var handRotY = _hand.transform.rotation.eulerAngles.y;
                    handRotY = (isLeftRotationT (handRotY)) ? handRotY / 45.0f : handRotY;
                    var sign = (isLeftRotationT (handRotY)) ? -1f : 1f;
                    var angleLerp = Mathf.Clamp01 (handRotY / 45.0f) * sign / 10f;
                    transform.DOMove (new Vector3 (_hand.transform.position.x - 0.6f, _hand.transform.position.y, _hand.transform.position.z + angleLerp), ANIM_TIME).Play ();
                    transform.DORotateQuaternion (_hand.transform.rotation, ANIM_TIME).Play ();
                })
                .AddTo (this);
        }

        /// <summary>
        /// 270 ~ 360を -45 ~ 0　でとるための判定
        /// </summary>
        /// <param name="handRotY">チェック対象</param>
        /// <returns></returns>
        private bool isLeftRotationT (float handRotY)
        {
            return handRotY >= 270;
        }
    }
}