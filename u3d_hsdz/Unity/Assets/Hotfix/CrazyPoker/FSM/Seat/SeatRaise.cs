namespace ETHotfix
{
	public class SeatRaise<T> : FSMState<T>
	{
		private static SeatRaise<T> instance;

		public static SeatRaise<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatRaise<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.RaiseEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.RaiseExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.RaiseExit();
		}
	}
}
