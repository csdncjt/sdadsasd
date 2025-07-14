namespace ETHotfix
{
	public class FSMState<T>
	{
		private static FSMState<T> instance;

		public static FSMState<T> Instance
		{
			get
			{
				return instance ?? (instance = new FSMState<T>());
			}
		}

		public virtual void Enter(T entity)
		{
			Log.Debug("FSMState Enter");
		}

		public virtual void Execute(T entity)
		{
			Log.Debug("FSMState Execute");
		}

		public virtual void Exit(T entity)
		{
			Log.Debug("FSMState Exit");
		}
	}
}
