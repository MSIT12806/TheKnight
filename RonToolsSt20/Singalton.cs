namespace CSharpFinalWork_TheKnight
{
    public class Singalton<T>
    {
        private static T _single;
        private static object syncRoot = new object();

        public static T SingletonObj
        {
            get
            {
                lock(syncRoot)
                {
                    if (_single == null)
                    {
                        _single = System.Activator.CreateInstance<T>();
                    }
                    return _single;
                }
            }
        }
    }
}