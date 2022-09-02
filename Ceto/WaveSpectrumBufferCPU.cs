using Ceto.Common.Threading.Scheduling;
using Ceto.Common.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ceto
{
	public abstract class WaveSpectrumBufferCPU : WaveSpectrumBuffer
	{
		public class Buffer
		{
			public IList<Vector4[]> data;

			public Color[] results;

			public Texture2D map;

			public bool disabled;

			public bool doublePacked;
		}

		public const int READ = 1;

		public const int WRITE = 0;

		protected WaveSpectrumBufferCPU.Buffer[] m_buffers;

		private FourierCPU m_fourier;

		private Scheduler m_scheduler;

		private List<ThreadedTask> m_fourierTasks;

		protected InitSpectrumDisplacementsTask m_initTask;

		public override bool Done
		{
			get
			{
				return this.IsDone();
			}
		}

		public override int Size
		{
			get
			{
				return this.m_fourier.size;
			}
		}

		public override bool IsGPU
		{
			get
			{
				return false;
			}
		}

		public Color[] WTable
		{
			get;
			private set;
		}

		public WaveSpectrumBufferCPU(int size, int numBuffers, Scheduler scheduler)
		{
			this.m_buffers = new WaveSpectrumBufferCPU.Buffer[numBuffers];
			this.m_fourier = new FourierCPU(size);
			this.m_fourierTasks = new List<ThreadedTask>();
			this.m_scheduler = scheduler;
			for (int i = 0; i < numBuffers; i++)
			{
				this.m_buffers[i] = this.CreateBuffer(size);
			}
		}

		private WaveSpectrumBufferCPU.Buffer CreateBuffer(int size)
		{
			WaveSpectrumBufferCPU.Buffer buffer = new WaveSpectrumBufferCPU.Buffer();
			buffer.doublePacked = true;
			buffer.data = new List<Vector4[]>();
			buffer.data.Add(new Vector4[size * size]);
			buffer.data.Add(new Vector4[size * size]);
			buffer.results = new Color[size * size];
			buffer.map = new Texture2D(size, size, TextureFormat.RGBAFloat, false, true);
			buffer.map.wrapMode = TextureWrapMode.Repeat;
			buffer.map.filterMode = FilterMode.Bilinear;
			buffer.map.hideFlags = HideFlags.HideAndDontSave;
			buffer.map.name = "Ceto Wave Spectrum CPU Buffer";
			buffer.map.SetPixels(buffer.results);
			buffer.map.Apply();
			return buffer;
		}

		public override void Release()
		{
			int num = this.m_buffers.Length;
			for (int i = 0; i < num; i++)
			{
				UnityEngine.Object.Destroy(this.m_buffers[i].map);
				this.m_buffers[i].map = null;
			}
		}

		public override Texture GetTexture(int idx)
		{
			if (idx < 0 || idx >= this.m_buffers.Length)
			{
				return Texture2D.blackTexture;
			}
			if (this.m_buffers[idx].disabled)
			{
				return Texture2D.blackTexture;
			}
			return this.m_buffers[idx].map;
		}

		public Vector4[] GetWriteBuffer(int idx)
		{
			if (idx < 0 || idx >= this.m_buffers.Length)
			{
				return null;
			}
			if (this.m_buffers[idx].disabled)
			{
				return null;
			}
			return this.m_buffers[idx].data[0];
		}

		public Vector4[] GetReadBuffer(int idx)
		{
			if (idx < 0 || idx >= this.m_buffers.Length)
			{
				return null;
			}
			if (this.m_buffers[idx].disabled)
			{
				return null;
			}
			return this.m_buffers[idx].data[1];
		}

		public WaveSpectrumBufferCPU.Buffer GetBuffer(int idx)
		{
			if (idx < 0 || idx >= this.m_buffers.Length)
			{
				return null;
			}
			if (this.m_buffers[idx].disabled)
			{
				return null;
			}
			return this.m_buffers[idx];
		}

		public IList<IList<Vector4[]>> GetData(int idx)
		{
			IList<IList<Vector4[]>> list = new List<IList<Vector4[]>>(3);
			int num = this.m_buffers.Length;
			if (idx < -1 || idx >= num)
			{
				return list;
			}
			if (idx == -1)
			{
				for (int i = 0; i < num; i++)
				{
					if (!this.m_buffers[i].disabled)
					{
						list.Add(this.m_buffers[i].data);
					}
				}
			}
			else if (!this.m_buffers[idx].disabled)
			{
				list.Add(this.m_buffers[idx].data);
			}
			return list;
		}

		public IList<Color[]> GetResults(int idx)
		{
			IList<Color[]> list = new List<Color[]>(3);
			int num = this.m_buffers.Length;
			if (idx < -1 || idx >= num)
			{
				return list;
			}
			if (idx == -1)
			{
				for (int i = 0; i < num; i++)
				{
					if (!this.m_buffers[i].disabled)
					{
						list.Add(this.m_buffers[i].results);
					}
				}
			}
			else if (!this.m_buffers[idx].disabled)
			{
				list.Add(this.m_buffers[idx].results);
			}
			return list;
		}

		public IList<Texture2D> GetMaps(int idx)
		{
			IList<Texture2D> list = new List<Texture2D>(3);
			int num = this.m_buffers.Length;
			if (idx < -1 || idx >= num)
			{
				return list;
			}
			if (idx == -1)
			{
				for (int i = 0; i < num; i++)
				{
					if (!this.m_buffers[i].disabled)
					{
						list.Add(this.m_buffers[i].map);
					}
				}
			}
			else if (!this.m_buffers[idx].disabled)
			{
				list.Add(this.m_buffers[idx].map);
			}
			return list;
		}

		public override void EnableBuffer(int idx)
		{
			int num = this.m_buffers.Length;
			if (idx < -1 || idx >= num)
			{
				return;
			}
			if (idx == -1)
			{
				for (int i = 0; i < num; i++)
				{
					this.m_buffers[i].disabled = false;
				}
			}
			else
			{
				this.m_buffers[idx].disabled = false;
			}
		}

		public override void DisableBuffer(int idx)
		{
			int num = this.m_buffers.Length;
			if (idx < -1 || idx >= num)
			{
				return;
			}
			if (idx == -1)
			{
				for (int i = 0; i < num; i++)
				{
					this.m_buffers[i].disabled = true;
				}
			}
			else
			{
				this.m_buffers[idx].disabled = true;
			}
		}

		public bool IsDone()
		{
			if (this.m_initTask == null)
			{
				return true;
			}
			if (!this.m_initTask.Done)
			{
				return false;
			}
			int count = this.m_fourierTasks.Count;
			for (int i = 0; i < count; i++)
			{
				if (!this.m_fourierTasks[i].Done)
				{
					return false;
				}
			}
			return true;
		}

		public override int EnabledBuffers()
		{
			int num = 0;
			int num2 = this.m_buffers.Length;
			for (int i = 0; i < num2; i++)
			{
				if (!this.m_buffers[i].disabled)
				{
					num++;
				}
			}
			return num;
		}

		public override bool IsEnabledBuffer(int idx)
		{
			return idx >= 0 && idx < this.m_buffers.Length && !this.m_buffers[idx].disabled;
		}

		public abstract void ProcessData(int index, Color[] results, Vector4[] data, int numGrids);

		public virtual void PackData(int index)
		{
			IList<Color[]> results = this.GetResults(index);
			IList<Texture2D> maps = this.GetMaps(index);
			for (int i = 0; i < results.Count; i++)
			{
				if (!(maps[i] == null))
				{
					maps[i].SetPixels(results[i]);
					maps[i].Apply();
				}
			}
		}

		public override void Run(WaveSpectrumCondition condition, float time)
		{
			if (!this.IsDone())
			{
				throw new InvalidOperationException("Can not run when there are tasks that have not finished");
			}
			base.TimeValue = time;
			base.HasRun = true;
			base.BeenSampled = false;
			this.m_fourierTasks.Clear();
			if (this.EnabledBuffers() == 0)
			{
				return;
			}
			this.Initilize(condition, time);
			ThreadedTask initTask = this.m_initTask;
			if (initTask == null)
			{
				throw new InvalidCastException("Init spectrum task is not a threaded task");
			}
			if (Ocean.DISABLE_FOURIER_MULTITHREADING)
			{
				initTask.Start();
				initTask.Run();
				initTask.End();
			}
			int num = this.m_buffers.Length;
			for (int i = 0; i < num; i++)
			{
				if (!this.m_buffers[i].disabled)
				{
					FourierTask fourierTask = new FourierTask(this, this.m_fourier, i, this.m_initTask.NumGrids);
					if (Ocean.DISABLE_FOURIER_MULTITHREADING)
					{
						fourierTask.Start();
						fourierTask.Run();
						fourierTask.End();
					}
					else
					{
						fourierTask.RunOnStopWaiting = true;
						fourierTask.WaitOn(initTask);
						this.m_scheduler.AddWaiting(fourierTask);
					}
					this.m_fourierTasks.Add(fourierTask);
				}
			}
			if (!Ocean.DISABLE_FOURIER_MULTITHREADING)
			{
				initTask.NoFinish = true;
				this.m_scheduler.Run(initTask);
			}
		}
	}
}
