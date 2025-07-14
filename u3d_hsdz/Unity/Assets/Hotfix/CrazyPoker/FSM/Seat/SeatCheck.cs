namespace ETHotfix
{
	public class SeatCheck<T> : FSMState<T>
	{
		private static SeatCheck<T> instance;

		public static SeatCheck<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatCheck<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.CheckEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.CheckExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.CheckExit();
		}
	}
}
