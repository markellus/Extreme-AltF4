/**********************************************
 *               Extreme AltF4                *
 *            (C) 2017 Marcel Bulla           *
 * https://github.com/markellus/Extreme-AltF4 *
 *  See file LICENSE for license information  *
 **********************************************/

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
