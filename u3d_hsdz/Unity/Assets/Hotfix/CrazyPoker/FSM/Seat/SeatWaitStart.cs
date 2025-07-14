namespace ETHotfix
{
	public class SeatWaitStart<T> : FSMState<T>
	{
		private static SeatWaitStart<T> instance;

		public static SeatWaitStart<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatWaitStart<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.WaitStartEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.WaitStartExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.WaitStartExit();
		}
	}
}
