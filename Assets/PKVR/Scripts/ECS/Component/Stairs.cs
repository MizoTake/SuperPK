using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SuperPK
{
	public class Stairs : MonoBehaviour
	{
		public GameObject Supporter;
		public Mesh SupporterMesh;
		public Material SupporterMaterial;
		public float ScaleX;
		public float OneSideWidth;
		public float StairsCount;
	}
}