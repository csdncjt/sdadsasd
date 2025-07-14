namespace ETHotfix
{
    public class SeatReservationWait<T> : FSMState<T>
    {
        private static SeatReservationWait<T> instance;

        public static SeatReservationWait<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatReservationWait<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.ReservationWaitEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.ReservationWaitExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.ReservationWaitExit();
        }
    }
}
