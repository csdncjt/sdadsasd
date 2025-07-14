namespace ETHotfix
{
	public class SeatStandup<T> : FSMState<T>
	{
		private static SeatStandup<T> instance;

		public static SeatStandup<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatStandup<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.StandupEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.StandupExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.StandupExit();
		}
	}
}
