namespace ETHotfix
{
    public class SeatWaitDairu<T> : FSMState<T>
    {
        private static SeatWaitDairu<T> instance;

        public static SeatWaitDairu<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatWaitDairu<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.WaitDairuEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.WaitDairuExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.WaitDairuExit();
        }
    }
}
