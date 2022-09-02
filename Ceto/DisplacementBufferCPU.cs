using Ceto.Common.Containers.Interpolation;
using Ceto.Common.Threading.Scheduling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ceto
{
	public class DisplacementBufferCPU : WaveSpectrumBufferCPU, IDisplacementBuffer
	{
		private const int NUM_BUFFERS = 3;

		private IList<InterpolatedArray2f[]> m_displacements;

		public DisplacementBufferCPU(int size, Scheduler scheduler) : base(size, 3, scheduler)
		{
			int gRIDS = QueryDisplacements.GRIDS;
			int cHANNELS = QueryDisplacements.CHANNELS;
			this.m_displacements = new List<InterpolatedArray2f[]>(2);
			this.m_displacements.Add(new InterpolatedArray2f[gRIDS]);
			this.m_displacements.Add(new InterpolatedArray2f[gRIDS]);
			for (int i = 0; i < gRIDS; i++)
			{
				this.m_displacements[0][i] = new InterpolatedArray2f(size, size, cHANNELS, true);
				this.m_displacements[1][i] = new InterpolatedArray2f(size, size, cHANNELS, true);
			}
		}

		protected override void Initilize(WaveSpectrumCondition condition, float time)
		{
			InterpolatedArray2f[] writeDisplacements = this.GetWriteDisplacements();
			writeDisplacements[0].Clear();
			writeDisplacements[1].Clear();
			writeDisplacements[2].Clear();
			writeDisplacements[3].Clear();
			if (this.m_initTask == null)
			{
				this.m_initTask = condition.GetInitSpectrumDisplacementsTask(this, time);
			}
			else if (this.m_initTask.SpectrumType != condition.Key.SpectrumType || this.m_initTask.NumGrids != condition.Key.NumGrids)
			{
				this.m_initTask = condition.GetInitSpectrumDisplacementsTask(this, time);
			}
			else
			{
				this.m_initTask.Reset(condition, time);
			}
		}

		public InterpolatedArray2f[] GetWriteDisplacements()
		{
			return this.m_displacements[0];
		}

		public InterpolatedArray2f[] GetReadDisplacements()
		{
			return this.m_displacements[1];
		}

		public override void Run(WaveSpectrumCondition condition, float time)
		{
			this.SwapDisplacements();
			base.Run(condition, time);
		}

		public void CopyAndCreateDisplacements(out IList<InterpolatedArray2f> displacements)
		{
			InterpolatedArray2f[] readDisplacements = this.GetReadDisplacements();
			QueryDisplacements.CopyAndCreateDisplacements(readDisplacements, out displacements);
		}

		public void CopyDisplacements(IList<InterpolatedArray2f> displacements)
		{
			InterpolatedArray2f[] readDisplacements = this.GetReadDisplacements();
			QueryDisplacements.CopyDisplacements(readDisplacements, displacements);
		}

		private void SwapDisplacements()
		{
			InterpolatedArray2f[] value = this.m_displacements[0];
			this.m_displacements[0] = this.m_displacements[1];
			this.m_displacements[1] = value;
		}

		public override void PackData(int index)
		{
			if (Ocean.DISABLE_PROCESS_DATA_MULTITHREADING)
			{
				IList<IList<Vector4[]>> data = base.GetData(index);
				IList<Color[]> results = base.GetResults(index);
				for (int i = 0; i < results.Count; i++)
				{
					Color[] results2 = results[i];
					Vector4[] data2 = data[i][1];
					int index2 = (index != -1) ? index : i;
					this.ProcessData(index2, results2, data2, this.m_initTask.NumGrids);
				}
			}
			base.PackData(index);
		}

		public override void ProcessData(int index, Color[] result, Vector4[] data, int numGrids)
		{
			int cHANNELS = QueryDisplacements.CHANNELS;
			int size = this.Size;
			InterpolatedArray2f[] writeDisplacements = this.GetWriteDisplacements();
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					int num = j + i * size;
					int num2 = num * cHANNELS;
					if (numGrids == 1)
					{
						result[num].r = data[num].x;
						result[num].g = data[num].y;
						result[num].b = 0f;
						result[num].a = 0f;
						if (index == 0)
						{
							writeDisplacements[0].Data[num2 + 1] = result[num].r;
						}
						else if (index == 1)
						{
							writeDisplacements[0].Data[num2] += result[num].r;
							writeDisplacements[0].Data[num2 + 2] += result[num].g;
						}
					}
					else if (numGrids == 2)
					{
						result[num].r = data[num].x;
						result[num].g = data[num].y;
						result[num].b = data[num].z;
						result[num].a = data[num].w;
						if (index == 0)
						{
							writeDisplacements[0].Data[num2 + 1] = result[num].r;
							writeDisplacements[1].Data[num2 + 1] = result[num].g;
						}
						else if (index == 1)
						{
							writeDisplacements[0].Data[num2] += result[num].r;
							writeDisplacements[0].Data[num2 + 2] += result[num].g;
							writeDisplacements[1].Data[num2] += result[num].b;
							writeDisplacements[1].Data[num2 + 2] += result[num].a;
						}
					}
					else if (numGrids == 3)
					{
						result[num].r = data[num].x;
						result[num].g = data[num].y;
						result[num].b = data[num].z;
						result[num].a = data[num].w;
						if (index == 0)
						{
							writeDisplacements[0].Data[num2 + 1] = result[num].r;
							writeDisplacements[1].Data[num2 + 1] = result[num].g;
							writeDisplacements[2].Data[num2 + 1] = result[num].b;
							writeDisplacements[3].Data[num2 + 1] = result[num].a;
						}
						else if (index == 1)
						{
							writeDisplacements[0].Data[num2] += result[num].r;
							writeDisplacements[0].Data[num2 + 2] += result[num].g;
							writeDisplacements[1].Data[num2] += result[num].b;
							writeDisplacements[1].Data[num2 + 2] += result[num].a;
						}
						else if (index == 2)
						{
							writeDisplacements[2].Data[num2] += result[num].r;
							writeDisplacements[2].Data[num2 + 2] += result[num].g;
						}
					}
					else if (numGrids == 4)
					{
						result[num].r = data[num].x;
						result[num].g = data[num].y;
						result[num].b = data[num].z;
						result[num].a = data[num].w;
						if (index == 0)
						{
							writeDisplacements[0].Data[num2 + 1] = result[num].r;
							writeDisplacements[1].Data[num2 + 1] = result[num].g;
							writeDisplacements[2].Data[num2 + 1] = result[num].b;
							writeDisplacements[3].Data[num2 + 1] = result[num].a;
						}
						else if (index == 1)
						{
							writeDisplacements[0].Data[num2] += result[num].r;
							writeDisplacements[0].Data[num2 + 2] += result[num].g;
							writeDisplacements[1].Data[num2] += result[num].b;
							writeDisplacements[1].Data[num2 + 2] += result[num].a;
						}
						else if (index == 2)
						{
							writeDisplacements[2].Data[num2] += result[num].r;
							writeDisplacements[2].Data[num2 + 2] += result[num].g;
							writeDisplacements[3].Data[num2] += result[num].b;
							writeDisplacements[3].Data[num2 + 2] += result[num].a;
						}
					}
					else
					{
						result[num].r = 0f;
						result[num].g = 0f;
						result[num].b = 0f;
						result[num].a = 0f;
					}
				}
			}
		}

		public Vector4 MaxRange(Vector4 choppyness, Vector2 gridScale)
		{
			InterpolatedArray2f[] readDisplacements = this.GetReadDisplacements();
			return QueryDisplacements.MaxRange(readDisplacements, choppyness, gridScale, null);
		}

		public void QueryWaves(WaveQuery query, QueryGridScaling scaling)
		{
			int num = this.EnabledBuffers();
			if (num == 0)
			{
				return;
			}
			InterpolatedArray2f[] readDisplacements = this.GetReadDisplacements();
			QueryDisplacements.QueryWaves(query, num, readDisplacements, scaling);
		}
	}
}
