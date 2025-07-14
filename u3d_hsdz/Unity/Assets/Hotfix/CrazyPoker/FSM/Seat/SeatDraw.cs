namespace ETHotfix
{
    public class SeatDraw<T> : FSMState<T>
    {
        private static SeatDraw<T> instance;

        public static SeatDraw<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatDraw<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.DrawEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.DrawExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.DrawExit();
        }
    }
}
