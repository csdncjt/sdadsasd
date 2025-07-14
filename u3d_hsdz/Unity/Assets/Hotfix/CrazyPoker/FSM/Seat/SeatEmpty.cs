namespace ETHotfix
{
	public class SeatEmpty<T> : FSMState<T>
	{
		private static SeatEmpty<T> instance;

		public static SeatEmpty<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatEmpty<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.EmptyEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.EmptyExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.EmptyExit();
		}
	}
}
