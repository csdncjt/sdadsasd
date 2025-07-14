namespace ETHotfix
{
	public class SeatCall<T> : FSMState<T>
	{
		private static SeatCall<T> instance;

		public static SeatCall<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatCall<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.CallEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.CallExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.CallExit();
		}
	}
}
