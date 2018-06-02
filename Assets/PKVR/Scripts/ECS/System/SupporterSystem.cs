using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SuperPK
{
	public struct SupporterData
	{
		public Stairs AudienceSeat;
	}

	public class SupporterSystem : BaseComponentSystem<SupporterData>
	{

		private GameObject[][] elements;

		public override void Start (SupporterData element)
		{
			var elementPos = element.AudienceSeat.transform.position;
			var scaleX = element.AudienceSeat.ScaleX;
			var stairsCount = element.AudienceSeat.StairsCount;
			elements = new GameObject[(int) scaleX][];
			for (int i = 0; i < scaleX; i++)
			{
				elements[i] = new GameObject[(int) stairsCount];
				for (int j = 0; j < stairsCount; j++)
				{
					var indexJ = j + 1;
					var pos = (element.AudienceSeat.transform.forward + Vector3.up) * (indexJ * 1.85f);
					if (pos.x == 0.0f)
					{
						pos += Vector3.right * ((elementPos.x - (scaleX / 2.0f)) + i);
					}
					else
					{
						pos += Vector3.forward * ((-elementPos.z - (scaleX / 2.0f)) + i);
					}
					elements[i][j] = GameObject.Instantiate (element.AudienceSeat.Supporter, elementPos + pos, Quaternion.identity);
				}
			}

		}

		public override void Update (SupporterData element) { }
	}
}