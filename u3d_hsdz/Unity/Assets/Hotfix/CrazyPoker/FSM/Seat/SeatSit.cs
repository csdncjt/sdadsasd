namespace ETHotfix
{
	public class SeatSit<T> : FSMState<T>
	{
		private static SeatSit<T> instance;

		public static SeatSit<T> Instance
		{
			get
			{
				return instance ?? (instance = new SeatSit<T>());
			}
		}

		public override void Enter(T entity)
		{
			Seat seat = entity as Seat;
			if (null != seat)
				seat.SitEnter();
		}

		public override void Execute(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.SitExecute();
		}

		public override void Exit(T entity)
		{
			Seat game = entity as Seat;
			if (null != game)
				game.SitExit();
		}
	}
}
