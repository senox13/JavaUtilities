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
        private readonly Dictionary<TKey, TValue> keyToValue;
        private readonly Dictionary<TValue, TKey> valueToKey;
        
        
        /*
         * Constructors
         */
        /// <summary>
        /// Constructs a new <see cref="BiDictionary{TKey, TValue}"/>
        /// containing the contents of the given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="pairs">The enumerable containing entries for the
        /// new dictionary</param>
        public BiDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs){
            int count = pairs.Count();
            keyToValue = new Dictionary<TKey, TValue>(count);
            valueToKey = new Dictionary<TValue, TKey>(count);
            foreach(KeyValuePair<TKey, TValue> pair in pairs){
                Add(pair.Key, pair.Value);
            }
        }
        
        /// <summary>
        /// Constructs a new <see cref="BiDictionary{TKey, TValue}"/>
        /// containing the contents of the given <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictIn">The dictionary containing entries for the
        /// new dictionary</param>
        public BiDictionary(IDictionary<TKey, TValue> dictIn)
            :this((IEnumerable<KeyValuePair<TKey, TValue>>)dictIn){}
        
        /// <summary>
        /// Constructs a new, empty <see cref="BiDictionary{TKey, TValue}"/>
        /// with the given capacity pre-allocated.
        /// </summary>
        /// <param name="capacity">The number of entries to pre-allocate
        /// memory for</param>
        public BiDictionary(int capacity)
            :this(new Dictionary<TKey, TValue>(capacity)){}

        /// <summary>
        /// Constructs a new, empty <see cref="BiDictionary{TKey, TValue}"/>.
        /// </summary>
        public BiDictionary()
            :this(new Dictionary<TKey, TValue>()){}


        /*
         * Properties
         */
        /// <inheritdoc/>
        public TValue this[TKey key]{
            get => keyToValue[key];
            set{
                keyToValue[key] = value;
                valueToKey[value] = key;
            }
        }

        /// <summary>
        /// Gets or sets the key of the element with the specified value.
        /// </summary>
        /// <param name="val">The value of the element to get or set</param>
        /// <returns>The key for the given value</returns>
        public TKey this[TValue val]{
            get => valueToKey[val];
            set{
                valueToKey[val] = value;
                keyToValue[value] = val;
            }
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys => keyToValue.Keys;

        /// <inheritdoc/>
        public ICollection<TValue> Values => valueToKey.Keys;

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
            return valueToKey.ContainsKey(value);
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
            if(!valueToKey.ContainsKey(value)){
                return false;
            }
            TKey key = valueToKey[value];
            if(valueToKey.Remove(value) != keyToValue.Remove(key)){
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
            return valueToKey.TryGetValue(value, out key);
        }


        /*
         * IDictionary methods
         */
        /// <inheritdoc/>
        public void Add(TKey key, TValue value){
            keyToValue.Add(key, value);
            valueToKey.Add(value, key);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key){
            return keyToValue.ContainsKey(key);
        }

        /// <inheritdoc/>
        public bool Remove(TKey key){
            if(!keyToValue.ContainsKey(key)){
                return false;
            }
            TValue value = keyToValue[key];
            if(keyToValue.Remove(key) != valueToKey.Remove(value)){
                throw new Exception("BiDictionary internal dictionaries are out of sync, something has gone very wrong");
            }
            return true;
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value){
            return keyToValue.TryGetValue(key, out value);
        }
        

        /*
         * ICollection methods
         */
        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item){
            keyToValue.Add(item.Key, item.Value);
            valueToKey.Add(item.Value, item.Key);
        }
        
        /// <inheritdoc/>
        public void Clear(){
            keyToValue.Clear();
            valueToKey.Clear();
        }
        
        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item){
            if(keyToValue.ContainsKey(item.Key)){
                return keyToValue[item.Key].Equals(item.Value);
            }
            return false;
        }
        
        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex){
            int i=arrayIndex;
            foreach(KeyValuePair<TKey, TValue> pair in keyToValue){
                array[i++] = pair;
            }
        }
        
        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item){
            if(keyToValue.Remove(item.Key) && valueToKey.Remove(item.Value)){
                return true;
            }
            return false;
        }


        /*
         * IEnumerable methods
         */
        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator(){
            return keyToValue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
    }
}
