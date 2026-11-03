using System;
namespace HashGame.CubeWorld
{
    public class QueueHandler<T>
    {
        public T this[int i]
        {
            get
            {
                if (values == null || i < 0 || i >= values.Length) return default(T);
                return values[i];
            }
        }
        #region readonly
        public int Capacity => values.Length;
        public int FirstIndex => 0;
        public int LastIndex => Capacity - 1;
        public T First => values[FirstIndex];
        public T Last => values[LastIndex];
        #endregion
        protected T[] values;
        public QueueHandler(int Capacity)
        {
            int count = (int)MathF.Max(1, Capacity);
            values = new T[count];
        }
        public void Add(T item)
        {
            for (int i = 0; i < Capacity - 1; i++)
            {
                values[i] = values[i + 1];
            }
            values[Capacity - 1] = item;
        }
        public bool GetFirst(T[] ExceptItems, out T result)
        {
            result = default(T);
            for (int i = LastIndex; i >= 0; i--)
            {
                bool isResult = true;
                T node = values[i];
                foreach (T item in ExceptItems)
                {
                    if (System.Object.Equals(node, item))
                    {
                        isResult = false;
                        break;
                    }
                }
                if (isResult)
                {
                    result = node;
                    return true;
                }
            }
            return false;
        }
    }
}