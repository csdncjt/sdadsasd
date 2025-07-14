namespace ETHotfix
{
    public class SeatSitAnimation<T> : FSMState<T>
    {
        private static SeatSitAnimation<T> instance;

        public static SeatSitAnimation<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatSitAnimation<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.SitAnimationEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.SitAnimationExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.SitAnimationExit();
        }
    }
}
