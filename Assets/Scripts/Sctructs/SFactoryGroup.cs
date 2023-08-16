namespace Structs
{
    public struct FactoryGroup
    {
        public GenericFactory genericFactory;
        public ProjectileFactory projectileFactory;
        public EnemyFactory enemyFactory;
        public EffectFactory effectFactory;
        public GrenadeFactory grenadeFactory;
        public PickupFactory pickupFactory;

        public FactoryGroup(
            GenericFactory genericFactory,
            ProjectileFactory projectileFactory,
            EnemyFactory enemyFactory,
            EffectFactory effectFactory,
            GrenadeFactory grenadeFactory,
            PickupFactory pickupFactory)
        {
            this.genericFactory = genericFactory;
            this.projectileFactory = projectileFactory;
            this.enemyFactory = enemyFactory;
            this.effectFactory = effectFactory;
            this.grenadeFactory = grenadeFactory;
            this.pickupFactory = pickupFactory;
        }
    }
}
