using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace JavaUtilities.Collections{
    /// <summary>
    /// Provides an <see cref="IDictionary{TKey, TValue}"/> implementation
    /// which not only maps keys to values, but also maps values to keys.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
    public class BiDictionary<TKey, TValue> : IDictionary<TKey, TValue>{
        /*
         * Fields
         */
        private const int DEFAULT_CAPACITY = 4;
        private readonly Dictionary<KeyWrapper<TKey>, TValue> keyToValue;
        private readonly Dictionary<KeyWrapper<TValue>, TKey> valueToKey;
        

        /*
         * Nested types
         */
        private readonly struct KeyWrapper<T>(T? key){
            public T? Key{get;} = key;

            public override bool Equals(object? obj){
                if(obj is not KeyWrapper<T> other){
                    return false;
                }
                return other.Key?.Equals(Key) ?? (Key == null && other.Key == null);
            }

            public override int GetHashCode(){
                return Key?.GetHashCode() ?? 0;
            }
        }

        
        /*
         * Constructors
         */
        /// <summary>
        /// Constructs a new <see cref="BiDictionary{TKey, TValue}"/>
        /// containing the contents of the given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="pairs">The enumerable containing entries for the
        /// new dictionary</param>
        public BiDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
            :this(pairs.Count()){
            foreach(KeyValuePair<TKey, TValue> pair in pairs){
                Add(pair.Key, pair.Value);
            }
        }
        
        /// <summary>
        /// Constructs a new, empty <see cref="BiDictionary{TKey, TValue}"/>
        /// with the given capacity pre-allocated.
        /// </summary>
        /// <param name="capacity">The number of entries to pre-allocate
        /// memory for</param>
        public BiDictionary(int capacity){
            keyToValue = new(capacity);
            valueToKey = new(capacity);
        }

        /// <summary>
        /// Constructs a new, empty <see cref="BiDictionary{TKey, TValue}"/>.
        /// </summary>
        public BiDictionary() : this(DEFAULT_CAPACITY){}


        /*
         * Properties
         */
        /// <inheritdoc/>
        public TValue this[TKey key]{
            get => keyToValue[new(key)];
            set{
                keyToValue[new(key)] = value;
                valueToKey[new(value)] = key;
            }
        }

        /// <summary>
        /// Gets or sets the key of the element with the specified value.
        /// </summary>
        /// <param name="val">The value of the element to get or set</param>
        /// <returns>The key for the given value</returns>
        public TKey this[TValue val]{
            get => valueToKey[new(val)];
            set{
                valueToKey[new(val)] = value;
                keyToValue[new(value)] = val;
            }
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys => valueToKey.Values;

        /// <inheritdoc/>
        public ICollection<TValue> Values => keyToValue.Values;

        /// <inheritdoc/>
        public int Count => keyToValue.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;


        /*
         * BiDictionary methods
         */
        /// <summary>
        /// Determines whether the <see cref="BiDictionary{TKey, TValue}"/>
        /// contains an element with the specified key.
        /// </summary>
        /// <param name="value">The value to locate in the dictionary</param>
        /// <returns><c>true</c> if the dictionary contains an element with
        /// the key; otherwise, <c>false</c></returns>
        public bool ContainsValue(TValue value){
            return keyToValue.ContainsValue(value);
        }

        /// <summary>
        /// Removes the element with the specified value from the
        /// <see cref="BiDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="value">The value of the element to remove</param>
        /// <returns><c>true</c> if the element is successfully removed;
        /// otherwise, <c>false</c>. This method also returns false if key
        /// was not found in the original dictionary</returns>
        public bool Remove(TValue value){
            if(!keyToValue.ContainsValue(value)){
                return false;
            }
            KeyWrapper<TValue> valueWrapper = new(value);
            TKey key = valueToKey[valueWrapper];
            if(valueToKey.Remove(valueWrapper) != keyToValue.Remove(new(key))){
                throw new Exception("BiDictionary internal dictionaries are out of sync, something has gone very wrong");
            }
            return true;
        }
        
        /// <summary>
        /// Gets the key associated with the specified value.
        /// </summary>
        /// <param name="value">The value whose key to get</param>
        /// <param name="key">When this method returns, the key associated
        /// with the specified value, if the value is found; otherwise, the
        /// default value for the type of the key parameter. This parameter
        /// is passed uninitialized.</param>
        /// <returns><c>true</c> if the <see cref="BiDictionary{TKey, TValue}"/>
        /// contains an element with the specified key; otherwise, <c>false</c></returns>
        public bool TryGetKey(TValue value, out TKey key){
            return valueToKey.TryGetValue(new(value), out key!);
        }


        /*
         * IDictionary methods
         */
        /// <inheritdoc/>
        public void Add(TKey key, TValue value){
            keyToValue.Add(new(key), value);
            valueToKey.Add(new(value), key);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key){
            return valueToKey.ContainsValue(key);
        }

        /// <inheritdoc/>
        public bool Remove(TKey key){
            if(!valueToKey.ContainsValue(key)){
                return false;
            }
            KeyWrapper<TKey> keyWrapper = new(key);
            TValue value = keyToValue[keyWrapper];
            if(keyToValue.Remove(keyWrapper) != valueToKey.Remove(new(value))){
                throw new Exception("BiDictionary internal dictionaries are out of sync, something has gone very wrong");
            }
            return true;
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value){
            return keyToValue.TryGetValue(new(key), out value!);
        }
        

        /*
         * ICollection methods
         */
        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item){
            keyToValue.Add(new(item.Key), item.Value);
            valueToKey.Add(new(item.Value), item.Key);
        }
        
        /// <inheritdoc/>
        public void Clear(){
            keyToValue.Clear();
            valueToKey.Clear();
        }
        
        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item){
            if(valueToKey.ContainsValue(item.Key)){
                return keyToValue[new(item.Key)]?.Equals(item.Value) ?? item.Value == null;
            }
            return false;
        }
        
        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex){
            int i=arrayIndex;
            foreach(KeyValuePair<KeyWrapper<TKey>, TValue> pair in keyToValue){
                array[i++] = new KeyValuePair<TKey, TValue>(pair.Key.Key!, pair.Value);
            }
        }
        
        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item){
            if(Contains(item)){
                keyToValue.Remove(new(item.Key));
                valueToKey.Remove(new(item.Value));
                return true;
            }
            return false;
        }


        /*
         * IEnumerable methods
         */
        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator(){
            return keyToValue
                .Select(p => new KeyValuePair<TKey, TValue>(p.Key.Key ?? default!, p.Value))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
    }
}
