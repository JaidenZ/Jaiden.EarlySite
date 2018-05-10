namespace Jaiden.Redis
{
    using System;

    public sealed class RedisMqPublisherContainer : RedisMqBaseContainer
    {
        private RedisMqPublisherContainer()
        {

        }

        private static RedisMqPublisherContainer g_pContainer = null;

        public static RedisMqPublisherContainer Current
        {
            get
            {
                if (g_pContainer == null)
                    g_pContainer = new RedisMqPublisherContainer();
                return g_pContainer;
            }
        }

        protected override bool IsSubclassOf(Type clazz)
        {
            return base.IsSubclassOf(clazz, typeof(RedisMqPublisher<>));
        }
    }
}
