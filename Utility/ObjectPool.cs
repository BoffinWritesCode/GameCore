using System;
using System.Collections.Generic;

namespace GameCore.Utility
{
    public class ObjectPool<T>
	{
		protected Func<T> _allocator;
		protected Queue<T> _pool;

		public ObjectPool(Func<T> allocator, int initialCapacity = 8)
        {
            _allocator = allocator ?? throw new ArgumentNullException("allocator");
			_pool = new Queue<T>(initialCapacity);
			
			for (int i = 0; i < initialCapacity; i++)
				_pool.Enqueue(allocator());
        }

		public T GetItem()
        {
			if (_pool.Count > 1)
				return _pool.Dequeue();

			return _allocator();
        }

        public void ReturnItem(T item)
        {
			_pool.Enqueue(item);
        }
    }
}