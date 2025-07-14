namespace ETHotfix
{
    public class SeatReservation<T> : FSMState<T>
    {
        private static SeatReservation<T> instance;

        public static SeatReservation<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatReservation<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.ReservationEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.ReservationExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.ReservationExit();
        }
    }
}
