namespace ETHotfix
{
	// 补盲
	public class SeatWaitBlind<T> : FSMState<T>
	{
		private static SeatWaitBlind<T> instance;

		public static SeatWaitBlind<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatWaitBlind<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.WaitBlindEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.WaitBlindExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.WaitBlindExit();
		}
	}
}
