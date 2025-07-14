namespace ETHotfix
{
	public class SeatFold<T> : FSMState<T>
	{
		private static SeatFold<T> instance;

		public static SeatFold<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatFold<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.FoldEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.FoldExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.FoldExit();
		}
	}
}
