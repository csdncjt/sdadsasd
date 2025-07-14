namespace ETHotfix
{
	public class SeatOperation<T> : FSMState<T>
	{
		private static SeatOperation<T> instance;

		public static SeatOperation<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatOperation<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.OperationEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.OperationExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.OperationExit();
		}
	}
}
