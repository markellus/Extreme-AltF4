using System.Collections.Generic;

namespace AltF4
{
    internal class SafeList<T> : List<T>
    {
        public void SafeAdd(T item)
        {
            if (!this.Contains(item))
            {
                this.Add(item);
            }
        }

        public void SafeRemove(T item)
        {
            if (this.Contains(item))
            {
                this.Remove(item);
            }
        }
    }
}
