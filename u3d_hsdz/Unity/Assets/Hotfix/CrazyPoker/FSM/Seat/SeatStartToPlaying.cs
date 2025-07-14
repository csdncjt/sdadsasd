namespace ETHotfix
{
	public class SeatStartToPlaying<T> : FSMState<T>
	{
		private static SeatStartToPlaying<T> instance;

		public static SeatStartToPlaying<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatStartToPlaying<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.StartToPlayingEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.StartToPlayingExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.StartToPlayingExit();
		}
	}
}
