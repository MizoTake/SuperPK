using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace PKVR
{
	public abstract class HandAbstract : MonoBehaviour
	{
		public virtual void HandBind (Subject<float> input)
		{
			var vec = Vector3.forward;
			var target = Vector3.zero;
			input
				.Subscribe (_ =>
				{
					target = transform.forward * Mathf.Clamp (target.z + _, transform.parent.localPosition.z, target.z + _);
					var value = (Mathf.Clamp (transform.position.z + _, transform.parent.position.z, transform.position.z + _) == transform.parent.position.z) ? 0f : _;
					transform.Translate (vec * value);
				})
				.AddTo (this);
		}

	}
}