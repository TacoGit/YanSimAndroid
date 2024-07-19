using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace CielaSpike
{
	public class Task : IEnumerator
	{
		private enum RunningState
		{
			Init = 0,
			RunningAsync = 1,
			PendingYield = 2,
			ToBackground = 3,
			RunningSync = 4,
			CancellationRequested = 5,
			Done = 6,
			Error = 7
		}

		private readonly IEnumerator _innerRoutine;

		private RunningState _state;

		private RunningState _previousState;

		private object _pendingCurrent;

		public object Current { get; private set; }

		public TaskState State
		{
			get
			{
				switch (_state)
				{
				case RunningState.CancellationRequested:
					return TaskState.Cancelled;
				case RunningState.Done:
					return TaskState.Done;
				case RunningState.Error:
					return TaskState.Error;
				case RunningState.Init:
					return TaskState.Init;
				default:
					return TaskState.Running;
				}
			}
		}

		public Exception Exception { get; private set; }

		public Task(IEnumerator routine)
		{
			_innerRoutine = routine;
			_state = RunningState.Init;
		}

		public bool MoveNext()
		{
			return OnMoveNext();
		}

		public void Reset()
		{
			throw new NotSupportedException("Not support calling Reset() on iterator.");
		}

		public void Cancel()
		{
			if (State == TaskState.Running)
			{
				GotoState(RunningState.CancellationRequested);
			}
		}

		public IEnumerator Wait()
		{
			while (State == TaskState.Running)
			{
				yield return null;
			}
		}

		private void GotoState(RunningState state)
		{
			if (_state == state)
			{
				return;
			}
			lock (this)
			{
				_previousState = _state;
				_state = state;
			}
		}

		private void SetPendingCurrentObject(object current)
		{
			lock (this)
			{
				_pendingCurrent = current;
			}
		}

		private bool OnMoveNext()
		{
			if (_innerRoutine == null)
			{
				return false;
			}
			Current = null;
			while (true)
			{
				switch (_state)
				{
				case RunningState.Init:
					GotoState(RunningState.ToBackground);
					break;
				case RunningState.RunningAsync:
					return true;
				case RunningState.RunningSync:
					MoveNextUnity();
					break;
				case RunningState.ToBackground:
					GotoState(RunningState.RunningAsync);
					MoveNextAsync();
					return true;
				case RunningState.PendingYield:
					if (_pendingCurrent == Ninja.JumpBack)
					{
						GotoState(RunningState.ToBackground);
						break;
					}
					if (_pendingCurrent == Ninja.JumpToUnity)
					{
						GotoState(RunningState.RunningSync);
						break;
					}
					Current = _pendingCurrent;
					if (_previousState == RunningState.RunningAsync)
					{
						_pendingCurrent = Ninja.JumpBack;
					}
					else
					{
						_pendingCurrent = Ninja.JumpToUnity;
					}
					return true;
				default:
					return false;
				}
			}
		}

		private void MoveNextAsync()
		{
			ThreadPool.QueueUserWorkItem(BackgroundRunner);
		}

		private void BackgroundRunner(object state)
		{
			MoveNextUnity();
		}

		private void MoveNextUnity()
		{
			try
			{
				if (_innerRoutine.MoveNext())
				{
					SetPendingCurrentObject(_innerRoutine.Current);
					GotoState(RunningState.PendingYield);
				}
				else
				{
					GotoState(RunningState.Done);
				}
			}
			catch (Exception ex)
			{
				Exception ex3 = (Exception = ex);
				Debug.LogError(string.Format("{0}\n{1}", ex3.Message, ex3.StackTrace));
				GotoState(RunningState.Error);
			}
		}
	}
}
