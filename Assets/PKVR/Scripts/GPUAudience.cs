using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace SuperPK
{
	public class GPUAudience : MonoBehaviour
	{

		struct AudienceData
		{
			public Vector3 Position;
			public float4x4 Rotation;
		}

		[SerializeField] Mesh mesh;
		[SerializeField] Material mat;
		[SerializeField] Stairs[] audienceSeat;
		[SerializeField] Transform lookTarget;

		ComputeBuffer audienceDataBuffer;
		uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
		ComputeBuffer argsBuffer;
		private int total = 0;
		private AudienceData[] audienceData;

		void Start ()
		{
			foreach (var seat in audienceSeat)
			{
				total += (int) (seat.instanceCount * seat.StairsCount);
			}
			total *= audienceSeat.Length;

			audienceDataBuffer = new ComputeBuffer (total, Marshal.SizeOf (typeof (AudienceData)));
			argsBuffer = new ComputeBuffer (1, args.Length * sizeof (uint), ComputeBufferType.IndirectArguments);
			audienceData = new AudienceData[total];
			int elementCnt = 1;

			foreach (var seat in audienceSeat)
			{
				var seatPos = seat.transform.position;
				var scaleX = seat.ScaleX;
				var stairsCount = seat.StairsCount;

				int cnt = 0;
				for (int i = 0; i < seat.instanceCount; i++)
				{
					for (int j = 0; j < stairsCount; j++)
					{
						var indexJ = j + 1;
						var pos = seat.transform.forward * (indexJ * 1.85f) + Vector3.up * indexJ * 1.5f;
						if (pos.x == 0.0f)
						{
							pos += Vector3.right * (seatPos.x - (scaleX / 2.0f) + i);
						}
						else
						{
							pos += Vector3.forward * ((-seatPos.z - (scaleX / 2.0f)) + i * 3.0f);
						}
						audienceData[cnt * elementCnt].Position = seatPos + pos;
						var mat = Matrix4x4.LookAt (seatPos + pos, lookTarget.position, Vector3.up);
						mat.SetTRS (Vector3.zero, Quaternion.Euler (Vector3.right * -90.0f + mat.rotation.eulerAngles), Vector3.one);
						audienceData[cnt * elementCnt].Rotation = mat;
						cnt += 1;
					}
				}
				elementCnt += 1;
			}
			audienceDataBuffer.SetData (audienceData);

			args[0] = (mesh != null) ? mesh.GetIndexCount (0) : 0;
			args[1] = (uint) total;
			argsBuffer.SetData (args);
			mat.SetBuffer ("_AudienceDataBuffer", audienceDataBuffer);
			mat.SetVector ("_AudienceMeshScale", Vector3.one * 5.0f);
		}

		void Update ()
		{
			for (int i = 0; i < audienceData.Length; i++)
			{
				audienceData[i].Position += Vector3.up * Mathf.Sin (Time.time * 10f) / 100f;
			}
			audienceDataBuffer.SetData (audienceData);
			mat.SetBuffer ("_AudienceDataBuffer", audienceDataBuffer);

			Graphics.DrawMeshInstancedIndirect (mesh, 0, mat, new Bounds (Vector3.zero, Vector3.one * 100f), argsBuffer);
		}

		void OnDestroy ()
		{
			if (audienceDataBuffer != null)
			{
				audienceDataBuffer.Release ();
				audienceDataBuffer = null;
			}
			if (argsBuffer != null)
			{
				argsBuffer.Release ();
				argsBuffer = null;
			}
		}
	}
}