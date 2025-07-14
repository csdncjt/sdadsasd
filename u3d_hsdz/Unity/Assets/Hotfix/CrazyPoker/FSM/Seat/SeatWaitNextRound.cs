namespace ETHotfix
{
    public class SeatWaitNextRound<T> : FSMState<T>
    {
        private static SeatWaitNextRound<T> instance;

        public static SeatWaitNextRound<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatWaitNextRound<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.WaitNextRoundEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.WaitNextRoundExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.WaitNextRoundExit();
        }
    }
}
