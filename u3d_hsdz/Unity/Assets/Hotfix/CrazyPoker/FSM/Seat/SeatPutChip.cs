namespace ETHotfix
{
	public class SeatPutChip<T> : FSMState<T>
	{
		private static SeatPutChip<T> instance;

		public static SeatPutChip<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatPutChip<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.PutChipEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.PutChipExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.PutChipExit();
		}
	}
}
