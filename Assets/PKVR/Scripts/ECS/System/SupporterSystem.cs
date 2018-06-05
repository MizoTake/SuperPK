// using System.Collections;
// using System.Collections.Generic;
// using System.Runtime.InteropServices;
// using DG.Tweening;
// using Unity.Entities;
// using Unity.Mathematics;
// using UnityEngine;

// namespace SuperPK
// {

// 	public class SupporterSystem : ComponentSystem
// 	{

// 		public struct SupporterData
// 		{
// 			public Stairs AudienceSeat;
// 		}

// 		public struct AudienceData
// 		{
// 			public Vector3 Position;
// 			public float2x2 Rotation;
// 		}

// 		private ComputeBuffer AudienceDataBuffer;
// 		private ComputeBuffer argsBuffer;
// 		private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
// 		private Mesh mesh;
// 		private Material mat;

// 		protected sealed override void OnUpdate ()
// 			{
// 				if (AudienceDataBuffer == null)
// 				{
// 					int elementCnt = 1;
// 					AudienceData[] audienceData = null;
// 					argsBuffer = new ComputeBuffer (1, args.Length * sizeof (uint), ComputeBufferType.IndirectArguments);

// 					foreach (var element in GetEntities<SupporterData> ())
// 					{
// 						mesh = element.AudienceSeat.SupporterMesh;
// 						mat = element.AudienceSeat.SupporterMaterial;
// 						var seatPos = element.AudienceSeat.transform.position;
// 						var scaleX = element.AudienceSeat.ScaleX;
// 						var stairsCount = element.AudienceSeat.StairsCount;
// 						var total = (int) scaleX * (int) stairsCount * GetEntities<SupporterData> ().Length;

// 						if (audienceData == null)
// 						{
// 							AudienceDataBuffer = new ComputeBuffer (total, Marshal.SizeOf (typeof (AudienceData)));
// 							audienceData = new AudienceData[total];
// 						}

// 						int cnt = 0;
// 						for (int i = 0; i < scaleX; i++)
// 						{
// 							for (int j = 0; j < stairsCount; j++)
// 							{
// 								var indexJ = j + 1;
// 								var pos = element.AudienceSeat.transform.forward * (indexJ * 1.85f) + Vector3.up * indexJ * 1.5f;
// 								if (pos.x == 0.0f)
// 								{
// 									pos += Vector3.right * ((seatPos.x - (scaleX / 2.0f)) + i);
// 								}
// 								else
// 								{
// 									pos += Vector3.forward * ((-seatPos.z - (scaleX / 2.0f)) + i);
// 								}
// 								audienceData[cnt * elementCnt].Position = seatPos + pos;
// 								audienceData[cnt * elementCnt].Rotation = default (float2x2);
// 								cnt += 1;
// 							}
// 						}
// 						elementCnt += 1;
// 					}
// 					args[0] = (mesh != null) ? mesh.GetIndexCount (0) : 0;
// 					args[1] = (uint) audienceData.Length;
// 					AudienceDataBuffer.SetData (audienceData);
// 					audienceData = null;
// 					argsBuffer.SetData (args);

// 					mat.SetBuffer ("AudienceDataBuffer", AudienceDataBuffer);
// 					mat.SetVector ("AudienceMeshScale", Vector3.one);
// 				}
// 				else if (mesh != null && AudienceDataBuffer != null)
// 				{
// 					Graphics.DrawMeshInstancedIndirect (mesh, 0, mat, new Bounds (Vector3.zero, Vector3.one * 100.0f), argsBuffer);
// 				}
// 			}

// 			~SupporterSystem ()
// 			{
// 				if (AudienceDataBuffer != null)
// 					AudienceDataBuffer.Release ();
// 				AudienceDataBuffer = null;

// 				if (argsBuffer != null)
// 					argsBuffer.Release ();
// 				argsBuffer = null;
// 			}
// 	}
// }