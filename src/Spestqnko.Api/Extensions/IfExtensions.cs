namespace Spestqnko.Api.Extensions
{
    public static class IfExtensions
    {
        public static If<T> If<T>(this T obj, Func<bool> ifPredicate, Action<T> action)
            where T : class
        {
            var success = false;
            if (ifPredicate.Invoke())
            {
                success = true;
                action.Invoke(obj);
            }

            return new If<T>(obj, success);
        }

        public static If<T> ElseIf<T>(this If<T> obj, Func<bool> ifPredicate, Action<T> action)
            where T : class
        {
            var success = false;
            if (!obj.Success && ifPredicate.Invoke())
            {
                success = true;
                action.Invoke(obj.Object);
            }

            return new If<T>(obj.Object, success);
        }

        public static T Else<T>(this If<T> obj, Action<T> action)
           where T : class
        {
            if (!obj.Success)
                action.Invoke(obj.Object);

            return obj.Object;
        }
    }

    public class If<T> where T : class
    {
        public If(T obj, bool success) 
        {
            Object = obj;
            Success = success;
        }

        public T Object { get; set; }
        public bool Success { get; set; }

        public static implicit operator T(If<T> ifObj) => ifObj.Object;
    }

    public class Else<T> where T : class
    {
        public T Object { get; set; }

        public static implicit operator T(Else<T> ifObj) => ifObj.Object;
    }
}
