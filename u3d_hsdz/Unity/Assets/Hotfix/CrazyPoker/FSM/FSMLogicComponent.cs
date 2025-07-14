using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class FSMLogicComponentAwakeSystem : AwakeSystem<FSMLogicComponent, Entity>
	{
		public override void Awake(FSMLogicComponent self, Entity entity)
		{
			self.Awake(entity);
		}
	}

	[ObjectSystem]
	public class FSMLogicComponentUpdateSystem: UpdateSystem<FSMLogicComponent>
	{
		public override void Update(FSMLogicComponent self)
		{
			self.Update();
		}
	}

	public class FSMLogicComponent : Component
	{
		public Entity entity;
		protected StateMachine<Entity> sm;
		public StateMachine<Entity> SM { get { return this.sm;} }

		public void Awake(Entity e)
		{
			this.entity = e;
			this.sm = new StateMachine<Entity>(this.entity);
		}

		public void Update()
		{
			if (null != this.sm)
				this.sm.UpdateStateMachine();
		}

		public void Reset(Entity entity)
		{
			this.sm = new StateMachine<Entity>(entity);
		}
	}
}
