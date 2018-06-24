using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SuperPK
{
	public abstract class Shootable : ScriptableObject
	{
		[Inject]
		protected Transform _ball;
	}

}