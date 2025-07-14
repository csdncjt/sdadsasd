namespace ETHotfix
{
    public class SeatInsurance<T> : FSMState<T>
    {
        private static SeatInsurance<T> instance;

        public static SeatInsurance<T> Instance
        {
            get
            {
                return instance ?? (instance = new SeatInsurance<T>());
            }
        }

        public override void Enter(T entity)
        {
            Seat seat = entity as Seat;
            if (null != seat)
                seat.InsuranceEnter();
        }

        public override void Execute(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.InsuranceExecute();
        }

        public override void Exit(T entity)
        {
            Seat game = entity as Seat;
            if (null != game)
                game.InsuranceExit();
        }
    }
}
