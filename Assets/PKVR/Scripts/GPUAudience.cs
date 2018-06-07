﻿using System.Collections;
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
			/// <summary>
			/// 座標
			/// </summary>
			public Vector3 Position;
			/// <summary>
			/// 回転
			/// </summary>
			public float3x3 Rotation;
		}

		[SerializeField] Mesh mesh;
		[SerializeField] Material mat;
		[SerializeField] Stairs[] audienceSeat;

		ComputeBuffer audienceDataBuffer;
		uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
		ComputeBuffer argsBuffer;

		void Start ()
		{
			var total = 0;
			foreach (var seat in audienceSeat)
			{
				total += (int) (seat.ScaleX * seat.StairsCount);
			}
			total *= audienceSeat.Length;

			audienceDataBuffer = new ComputeBuffer (total, Marshal.SizeOf (typeof (AudienceData)));
			argsBuffer = new ComputeBuffer (1, args.Length * sizeof (uint), ComputeBufferType.IndirectArguments);
			var audienceData = new AudienceData[total];
			int elementCnt = 1;

			foreach (var seat in audienceSeat)
			{
				var seatPos = seat.transform.position;
				var scaleX = seat.ScaleX;
				var stairsCount = seat.StairsCount;

				int cnt = 0;
				for (int i = 0; i < scaleX; i++)
				{
					for (int j = 0; j < stairsCount; j++)
					{
						var indexJ = j + 1;
						var pos = seat.transform.forward * (indexJ * 1.85f) + Vector3.up * indexJ * 1.5f;
						if (pos.x == 0.0f)
						{
							pos += Vector3.right * ((seatPos.x - (scaleX / 2.0f)) + i);
						}
						else
						{
							pos += Vector3.forward * ((-seatPos.z - (scaleX / 2.0f)) + i);
						}
						audienceData[cnt * elementCnt].Position = seatPos + pos;
						float sin = Mathf.Sin (90);
						float cos = Mathf.Cos (90);
						var matrix = new float3x3 (m00: 1.0f, m01: 1.0f + cos, m02: sin, m10: 0.0f, m11: cos, m12: -sin, m20: -sin, m21 : sin, m22 : cos);
						audienceData[cnt * elementCnt].Rotation = matrix;
						cnt += 1;
						// Debug.Log (cnt * elementCnt + " " + audienceData[cnt * elementCnt].Position);
					}
				}
				elementCnt += 1;
			}

			audienceDataBuffer.SetData (audienceData);
			audienceData = null;

			args[0] = (mesh != null) ? mesh.GetIndexCount (0) : 0;
			args[1] = (uint) total;
			argsBuffer.SetData (args);
			mat.SetBuffer ("_AudienceDataBuffer", audienceDataBuffer);
			mat.SetVector ("_AudienceMeshScale", Vector3.one * 5.0f);
		}

		/// <summary>
		/// 毎フレーム更新
		/// </summary>
		void Update ()
		{
			Graphics.DrawMeshInstancedIndirect (mesh, 0, mat, new Bounds (Vector3.zero, Vector3.one * 300f), argsBuffer);
		}

		/// <summary>
		/// 破棄
		/// </summary>
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