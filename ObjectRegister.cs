using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GameCore
{
    public class ObjectRegister<T> : IEnumerable<T>
    {
        protected List<Type> _ignoredTypes;
        protected int _index = 0;
        protected List<T> _allValues;
        protected Dictionary<Type, RegisteredValue> _dict;

        public int Count => _allValues.Count;

        public ObjectRegister()
        {
            _allValues = new List<T>();
            _ignoredTypes = new List<Type>();
            _dict = new Dictionary<Type, RegisteredValue>();
            //_idDict = new Dictionary<int, RegisteredValue>();
        }

        /// <summary>
        /// Ignores this type when automatically collecting types from an assembly.
        /// </summary>
        /// <typeparam name="U">The type to ignore.</typeparam>
        public void Ignore<U>() where U : T
        {
            _ignoredTypes.Add(typeof(U));
        }

        /// <summary>
        /// Registers types automatically from the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        public void RegisterTypesFrom(Assembly assembly)
        {
            Type myType = typeof(T);
            var types = assembly.GetTypes().Where(t => !t.IsAbstract && myType.IsAssignableFrom(t));
            foreach (var type in types)
            {
                Register(type);
            }
        }

        public virtual int Register(Type type)
        {
            var rValue = new RegisteredValue((T)Activator.CreateInstance(type), _index);
            _dict.Add(type, rValue);
            _allValues.Add(rValue.Value);
            //_idDict.Add(_index, rValue);

            return _index++;
        }

        public virtual int Register<U>() where U : T, new()
        {
            Type uType = typeof(U);

            var rValue = new RegisteredValue((T)Activator.CreateInstance(uType), _index);
            _dict.Add(uType, rValue);
            _allValues.Add(rValue.Value);
            // _idDict.Add(_index, rValue);

            return _index++;
        }

        public virtual int Register<U>(params object[] args) where U : T
        {
            Type uType = typeof(U);

            var rValue = new RegisteredValue((T)Activator.CreateInstance(uType, args), _index);
            _dict.Add(uType, rValue);
            _allValues.Add(rValue.Value);
            // _idDict.Add(_index, rValue);

            return _index++;
        }

        public virtual int Register<U>(U value) where U : T
        {
            Type uType = typeof(U);

            var rValue = new RegisteredValue(value, _index);
            _dict.Add(uType, rValue);
            _allValues.Add(rValue.Value);
            // _idDict.Add(_index, rValue);

            return _index++;
        }

        public virtual U Get<U>() where U : T
        {
            if (_dict.TryGetValue(typeof(U), out var obj))
            {
                return (U)obj.Value;
            }
            throw new KeyNotFoundException("Key: " + typeof(U).FullName + " does not exist!");
        }

        public virtual T Get(int index)
        {
            /*if (_idDict.TryGetValue(index, out var value))
            {
                return value.Value;
            }
            */
            return _allValues[index];
            throw new KeyNotFoundException("ID " + index + " does not exist!");
        }

        public virtual U Get<U>(int index) where U : T
        {
            /*if (_idDict.TryGetValue(index, out var value))
            {
                return (U)value.Value;
            }
            */
            return (U)_allValues[index];
            throw new KeyNotFoundException("ID " + index + " does not exist!");
        }

        public virtual int GetID<U>() where U : T
        {
            if (_dict.TryGetValue(typeof(U), out var obj))
            {
                return obj.Index;
            }
            throw new KeyNotFoundException("Key: " + typeof(U).FullName + " does not exist!");
        }

        public virtual int GetID(T obj)
        {
            if (_dict.TryGetValue(obj.GetType(), out var result))
            {
                return result.Index;
            }
            throw new KeyNotFoundException("Key: " + obj.GetType().FullName + " does not exist!");
        }

        public virtual void Remove<U>() where U : T
        {
            int index = GetID<U>();

            _allValues.RemoveAt(index);
            _dict.Remove(typeof(U));
            //_idDict.Remove(index);
        }

        public virtual bool TryRemove<U>() where U : T
        {
            Type t = typeof(U);
            if (!_dict.TryGetValue(t, out RegisteredValue value))
            {
                return false;
            }

            _allValues.RemoveAt(value.Index);
            _dict.Remove(t);
            //_idDict.Remove(value.Index);

            return true;
        }

        public T this[int index] => _allValues[index];
        public T this[Type type] => _dict[type].Value;

        public IEnumerator<T> GetEnumerator()
        {
            return _allValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected struct RegisteredValue
        {
            public T Value;
            public int Index;
            public RegisteredValue(T ob, int index)
            {
                Value = ob;
                Index = index;
            }
        }
    }
}
