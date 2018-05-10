namespace Jaiden.Redis
{
    using System;

    public sealed class RedisMqConsumerContainer : RedisMqBaseContainer
    {
        private RedisMqConsumerContainer()
        {

        }

        private static RedisMqConsumerContainer g_pContainer = null;

        public static RedisMqConsumerContainer Current
        {
            get
            {
                if (g_pContainer == null)
                    g_pContainer = new RedisMqConsumerContainer();
                return g_pContainer;
            }
        }

        protected override bool IsSubclassOf(Type clazz)
        {
            return base.IsSubclassOf(clazz, typeof(RedisMqConsumer<>));
        }
    }
}
