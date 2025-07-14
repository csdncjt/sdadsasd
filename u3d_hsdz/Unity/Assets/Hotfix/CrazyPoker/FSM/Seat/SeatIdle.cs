namespace ETHotfix
{
	public class SeatIdle<T> : FSMState<T>
	{
		private static SeatIdle<T> instance;

		public static SeatIdle<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatIdle<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.IdleEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.IdleExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.IdleExit();
		}
	}
}
