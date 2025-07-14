namespace ETHotfix
{
	public class SeatCompare<T> : FSMState<T>
	{
		private static SeatCompare<T> instance;

		public static SeatCompare<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatCompare<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.CompareEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.CompareExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.CompareExit();
		}
	}
}
