namespace ETHotfix
{
	public class SeatTimeout<T> : FSMState<T>
	{
		private static SeatTimeout<T> instance;

		public static SeatTimeout<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatTimeout<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.TimeoutEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.TimeoutExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.TimeoutExit();
		}
	}
}
