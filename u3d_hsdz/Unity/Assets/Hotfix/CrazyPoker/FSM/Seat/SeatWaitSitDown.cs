namespace ETHotfix
{
    public class SeatWaitSitDown<T> : FSMState<T>
    {
        private static SeatWaitSitDown<T> instance;

        public static SeatWaitSitDown<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatWaitSitDown<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.WaitSitDownEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.WaitSitDownExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.WaitSitDownExit();
        }
    }
}
