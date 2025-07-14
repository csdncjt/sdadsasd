namespace ETHotfix
{
	public class SeatStart<T> : FSMState<T>
	{
		private static SeatStart<T> instance;

		public static SeatStart<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatStart<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.StartEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.StartExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.StartExit();
		}
	}
}
