namespace ETHotfix
{
	public class SeatAddChips<T> : FSMState<T>
	{
		private static SeatAddChips<T> instance;

		public static SeatAddChips<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatAddChips<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.AddChipsEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.AddChipsExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.AddChipsExit();
		}
	}
}
