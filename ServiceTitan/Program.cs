using System;
using System.Collections;
using System.Collections.Generic;

namespace ServiceTitan
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Our Application");

            var dictionary = new MultiValueDictionary<int, string>();
            dictionary.Add(1, "value1");
            dictionary.Add(1, "value2");
            dictionary.Add(2, "value3");

            foreach (var value in dictionary)
            {
                Console.WriteLine(value.Key);
                Console.WriteLine(value.Value);

            }
        }
    }
    public interface IMultiValueDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        /// <summary>
        /// Adds a value to either existing key or creates a new key and adds the value to it if the key value pair does not already exist
        /// </summary>
        /// <param name="key">Key to add value to</param>
        /// <param name="value">Value to add</param>
        /// <returns>true if the underlying collection has changed; false otherwise</returns>
        bool Add(K key, V value);

        /// <summary>
        /// returns a sequence of values for the given key. throws KeyNotFoundException if the key is not present
        /// </summary>
        /// <param name="key">key to retrive the sequence of values for</param>
        /// <returns>sequence of values for the given key.</returns>
        IEnumerable<V> Get(K key);

        /// <summary>
        /// returns a sequence of values for the given key. returns empty sequence if the key is not present
        /// </summary>
        /// <param name="key">key to retrieve the sequence of values for</param>
        /// <returns>sequence of values for the given key</returns>
        IEnumerable<V> GetOrDefault(K key);

        /// <summary>
        /// Removes the value from the values associated with the given key. throws KeyNotFoundException if the key is not present
        /// </summary>
        /// <param name="key">key which values need to be adjusted</param>
        /// <param name="value">value to remove from the values for the given key</param>
        void Remove(K key, V value);

        /// <summary>
        /// Removes the given key from the dictionary with all the values associated with it
        /// </summary>
        /// <param name="key"></param>
        void Clear(K key);
    }

    public class MultiValueDictionary<K, V> : IMultiValueDictionary<K, V>
    {
        Dictionary<K, List<V>> _dictionary = new Dictionary<K, List<V>>();

        // Add your implementation here
        public bool Add(K key, V value)
        {
            //Add 
            List<V> list;
            if (this._dictionary.TryGetValue(key, out list))
            {
                if (list.Contains(value))
                {
                    return false;
                }
                else
                {
                    list.Add(value);
                    return true;
                }

            }
            else
            {
                list = new List<V>();
                list.Add(value);
                this._dictionary[key] = list;
                return true;

            }
        }

        public IEnumerable<V> Get(K key)
        {
            List<V> list;

            if (this._dictionary.TryGetValue(key, out list))
            {
                return list;
            }

            throw new KeyNotFoundException("KeyNotFoundException");


        }
        public IEnumerable<V> GetOrDefault(K key)
        {

            List<V> list;

            if (this._dictionary.TryGetValue(key, out list))
            {
                return list;
            }

            return list;
        }

        public void Remove(K key, V value)
        {
            List<V> list;
            if (this._dictionary.TryGetValue(key, out list))
            {
                if (list.Contains(value))
                {
                    list.Remove(value);
                    this._dictionary[key] = list;

                }

            }

            throw new KeyNotFoundException("KeyNotFoundException");

        }

        public void Clear(K key)
        {
            this._dictionary.Remove(key);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            //  return this._dictionary.GetEnumerator();
            foreach (var item in this._dictionary)
            {
                foreach (var listValue in item.Value)
                {
                    yield return new KeyValuePair<K, V>(item.Key, listValue);
                }

            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



    }

}
