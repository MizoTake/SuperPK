using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace PKVR
{
	[RequireComponent (typeof (Rigidbody))]
	public abstract class HandAbstract : MonoBehaviour
	{
		[Inject]
		private InputGoController _controller;

		[SerializeField]
		private float _speed = 0.3f;

		public void HandBind ()
		{
			var rigid = GetComponent<Rigidbody> ();
			var target = Vector3.zero;
			var next = transform.position;
			var lerpValue = 0f;
			_controller.ovrTouchpad
				.Subscribe (_ =>
				{
					var speed = (_) ? _speed : -_speed;
					var value = (Mathf.Clamp (transform.position.z + speed, _controller.transform.position.z, transform.position.z + speed) == _controller.transform.position.z) ? 0f : speed;
					lerpValue += speed;
					next = Vector3.Lerp (_controller.transform.position, transform.position + _controller.transform.forward * value, Mathf.Clamp01 (lerpValue));
				})
				.AddTo (this);

			this.FixedUpdateAsObservable ()
				.Subscribe (_ => rigid.MovePosition (next))
				.AddTo (this);
		}

	}
}