using Ceto.Common.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ceto
{
	public class FourierTask : ThreadedTask
	{
		private FourierCPU m_fourier;

		private WaveSpectrumBufferCPU m_buffer;

		private int m_numGrids;

		private int m_index;

		private IList<Vector4[]> m_data;

		private Color[] m_results;

		private bool m_doublePacked;

		public FourierTask(WaveSpectrumBufferCPU buffer, FourierCPU fourier, int index, int numGrids) : base(true)
		{
			if (this.m_index == -1)
			{
				throw new InvalidOperationException("Index can be -1. Fourier for multiple buffers is not being used");
			}
			this.m_buffer = buffer;
			this.m_fourier = fourier;
			this.m_index = index;
			this.m_numGrids = numGrids;
			WaveSpectrumBufferCPU.Buffer buffer2 = this.m_buffer.GetBuffer(this.m_index);
			this.m_data = buffer2.data;
			this.m_results = buffer2.results;
			this.m_doublePacked = buffer2.doublePacked;
		}

		public override void Start()
		{
			base.Start();
		}

		public override IEnumerator Run()
		{
			this.PerformSingleFourier();
			this.FinishedRunning();
			return null;
		}

		public override void End()
		{
			base.End();
			this.m_buffer.PackData(this.m_index);
		}

		private void PerformSingleFourier()
		{
			int num;
			if (this.m_doublePacked)
			{
				num = this.m_fourier.PeformFFT_DoublePacked(0, this.m_data, this);
			}
			else
			{
				num = this.m_fourier.PeformFFT_SinglePacked(0, this.m_data, this);
			}
			if (base.Cancelled)
			{
				return;
			}
			if (num != 1)
			{
				throw new InvalidOperationException("Fourier transform did not result in the read buffer at index " + 1);
			}
			if (!Ocean.DISABLE_PROCESS_DATA_MULTITHREADING)
			{
				this.m_buffer.ProcessData(this.m_index, this.m_results, this.m_data[num], this.m_numGrids);
			}
		}
	}
}
