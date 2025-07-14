namespace ETHotfix
{
	public class SeatRoundEnd<T> : FSMState<T>
	{
		private static SeatRoundEnd<T> instance;

		public static SeatRoundEnd<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatRoundEnd<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
            if (null != seat)
				seat.RoundEndEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.RoundEndExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.RoundEndExit();
		}
	}
}
