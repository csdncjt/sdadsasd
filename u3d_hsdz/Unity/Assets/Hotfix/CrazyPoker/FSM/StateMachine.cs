using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
	public class StateMachine<T>
	{
		private T owner;
		private FSMState<T> currentState;
		private FSMState<T> previousState;
		private FSMState<T> globalState;

		public StateMachine(T entity)
		{
			this.owner = entity;
			this.currentState = null;
			this.previousState = null;
			this.globalState = null;
		}

		public void SetCurrentState(FSMState<T> state)
		{
			this.currentState = state;
		}

		public void SetPreviousState(FSMState<T> state)
		{
			this.previousState = state;
		}

		public void SetGlobalState(FSMState<T> state)
		{
			this.globalState = state;
		}

		public void ChangeState(FSMState<T> newState)
		{
			if (newState.Equals(this.currentState))
				return;

			this.previousState = this.currentState;

			if (null != this.currentState && newState != this.currentState)
			{
				this.currentState.Exit(this.owner);
			}

			this.currentState = newState;
			if (null != this.currentState)
			{
				this.currentState.Enter(this.owner);
			}
			else
			{
				Log.Error("state is null");
			}
		}

		public void ReverToPreviousState()
		{
			if (null != this.previousState)
			{
				ChangeState(this.previousState);
			}
		}

		public FSMState<T> CurrentState { get { return this.currentState;} }

		public FSMState<T> PreviousState { get { return this.previousState;} }

		public FSMState<T> GlobalState { get { return this.globalState;} }

		public bool IsInState(FSMState<T> state)
		{
			return this.currentState.Equals(state);
		}

		public void UpdateStateMachine()
		{
			if (null != this.globalState)
			{
				this.globalState.Execute(this.owner);
			}
			else
			{
				if (null != this.currentState)
				{
					this.currentState.Execute(this.owner);
				}
			}
		}

		public void Clear()
		{
			this.owner = default(T);
			this.currentState = null;
			this.previousState = null;
			this.globalState = null;
		}
	}
}
