namespace ETHotfix
{
    public class SeatKeep<T> : FSMState<T>
    {
        private static SeatKeep<T> instance;

        public static SeatKeep<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatKeep<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.KeepEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.KeepExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.KeepExit();
        }
    }
}
