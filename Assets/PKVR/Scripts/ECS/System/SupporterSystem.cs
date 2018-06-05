using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SuperPK
{

	public class SupporterSystem : ComponentSystem
	{

		public struct SupporterData
		{
			public Stairs AudienceSeat;
		}

		public struct AudienceData
		{
			public float3 Position;
			public float2x2 Rotation;
		}

		private AudienceData[] audienceData;
		private ComputeBuffer AudienceDataBuffer;
		private ComputeBuffer argsBuffer;
		private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

		protected sealed override void OnUpdate ()
			{
				int elementCnt = 1;
				int subMeshCnt = 0;
				Mesh mesh = null;
				Material mat = null;
				foreach (var element in GetEntities<SupporterData> ())
				{
					mesh = element.AudienceSeat.SupporterMesh;
					mat = element.AudienceSeat.SupporterMaterial;
					var seatPos = element.AudienceSeat.transform.position;
					var scaleX = element.AudienceSeat.ScaleX;
					var stairsCount = element.AudienceSeat.StairsCount;
					var total = (int) scaleX * (int) stairsCount * GetEntities<SupporterData> ().Length;

					if (argsBuffer == null)
					{
						argsBuffer = new ComputeBuffer (1, args.Length * sizeof (uint), ComputeBufferType.IndirectArguments);
						AudienceDataBuffer = new ComputeBuffer (total, Marshal.SizeOf (typeof (AudienceData)));
					}

					if (audienceData == null) audienceData = new AudienceData[total];

					int cnt = 0;
					for (int i = 0; i < scaleX; i++)
					{
						for (int j = 0; j < stairsCount; j++)
						{
							var indexJ = j + 1;
							var pos = element.AudienceSeat.transform.forward * (indexJ * 1.85f) + Vector3.up * indexJ * 1.5f;
							if (pos.x == 0.0f)
							{
								pos += Vector3.right * ((seatPos.x - (scaleX / 2.0f)) + i);
							}
							else
							{
								pos += Vector3.forward * ((-seatPos.z - (scaleX / 2.0f)) + i);
							}
							audienceData[cnt * elementCnt].Position = seatPos + pos;
							audienceData[cnt * elementCnt].Rotation = default (float2x2);
							// matrix.SetTRS (, Quaternion.Euler (270.0f, 0.0f, 0.0f), Vector3.one);

							cnt += 1;
						}
					}
					elementCnt += 1;
				}

				if (mesh != null)
				{
					AudienceDataBuffer.SetData (audienceData);
					mat.SetBuffer ("AudienceDataBuffer", AudienceDataBuffer);
					mat.SetVector ("AudienceMeshScale", Vector3.one);

					subMeshCnt = Mathf.Clamp (subMeshCnt, 0, mesh.subMeshCount - 1);
					args[0] = (uint) mesh.GetIndexCount (subMeshCnt);
					args[1] = (uint) audienceData.Length;
					args[2] = (uint) mesh.GetIndexStart (subMeshCnt);
					args[3] = (uint) mesh.GetBaseVertex (subMeshCnt);

					argsBuffer.SetData (args);
					Graphics.DrawMeshInstancedIndirect (mesh, subMeshCnt, mat, new Bounds (Vector3.zero, new Vector3 (100f, 100f, 100f)), argsBuffer);
				}
				else
				{
					args[0] = args[1] = args[2] = args[3] = 0;
				}

			}

			~SupporterSystem ()
			{
				if (AudienceDataBuffer != null)
					AudienceDataBuffer.Release ();
				AudienceDataBuffer = null;

				if (argsBuffer != null)
					argsBuffer.Release ();
				argsBuffer = null;
			}
	}
}