namespace ETHotfix
{
    public class SeatStraddle<T> : FSMState<T>
    {
        private static SeatStraddle<T> instance;

        public static SeatStraddle<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatStraddle<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.StraddleEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.StraddleExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.StraddleExit();
        }
    }
}
