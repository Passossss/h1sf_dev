namespace H1SF.Middleware.Helpers
{

    public static class Singleton<T> where T : class, new()
    {
        // Use Lazy<T> for thread-safe, lazy initialization
        private static readonly Lazy<T> instance = new Lazy<T>(() => new T());

        public static T Instance => instance.Value;
    }
}