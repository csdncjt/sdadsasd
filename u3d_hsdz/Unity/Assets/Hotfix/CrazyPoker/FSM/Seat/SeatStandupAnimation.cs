namespace ETHotfix
{
    public class SeatStandupAnimation<T> : FSMState<T>
    {
        private static SeatStandupAnimation<T> instance;

        public static SeatStandupAnimation<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatStandupAnimation<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.StandupAnimationEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.StandupAnimationExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.StandupAnimationExit();
        }
    }
}
