namespace ETHotfix
{
	public class SeatAllin<T> : FSMState<T>
	{
		private static SeatAllin<T> instance;

		public static SeatAllin<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatAllin<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.AllinEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.AllinExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.AllinExit();
		}
	}
}
