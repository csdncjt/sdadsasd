namespace ETHotfix
{
	public class SeatWaitOther<T> : FSMState<T>
	{
		private static SeatWaitOther<T> instance;

		public static SeatWaitOther<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatWaitOther<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.WaitOtherEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.WaitOtherExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.WaitOtherExit();
		}
	}
}
