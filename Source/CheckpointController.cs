using System;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
	public Checkpoint[] CheckpointsList;

	public LookAtTargetController Arrow;

	private Checkpoint CurrentCheckpoint;

	private int CheckpointId;

	private void Start()
	{
		if (this.CheckpointsList.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.CheckpointsList.Length; i++)
		{
			this.CheckpointsList[i].gameObject.SetActive(false);
		}
		this.CheckpointId = 0;
		this.SetCurrentCheckpoint(this.CheckpointsList[this.CheckpointId]);
	}

	private void SetCurrentCheckpoint(Checkpoint checkpoint)
	{
		if (this.CurrentCheckpoint != null)
		{
			this.CurrentCheckpoint.gameObject.SetActive(false);
			Checkpoint expr_28 = this.CurrentCheckpoint;
			expr_28.CheckpointActivated = (Action)Delegate.Remove(expr_28.CheckpointActivated, new Action(this.CheckpointActivated));
		}
		this.CurrentCheckpoint = checkpoint;
		Checkpoint expr_56 = this.CurrentCheckpoint;
		expr_56.CheckpointActivated = (Action)Delegate.Combine(expr_56.CheckpointActivated, new Action(this.CheckpointActivated));
		this.Arrow.Target = this.CurrentCheckpoint.transform;
		this.CurrentCheckpoint.gameObject.SetActive(true);
	}

	private void CheckpointActivated()
	{
		this.CheckpointId++;
		if (this.CheckpointId >= this.CheckpointsList.Length)
		{
			this.CurrentCheckpoint.gameObject.SetActive(false);
			Checkpoint expr_38 = this.CurrentCheckpoint;
			expr_38.CheckpointActivated = (Action)Delegate.Remove(expr_38.CheckpointActivated, new Action(this.CheckpointActivated));
			this.Arrow.gameObject.SetActive(false);
			return;
		}
		this.SetCurrentCheckpoint(this.CheckpointsList[this.CheckpointId]);
	}

	private void Update()
	{
	}
}
