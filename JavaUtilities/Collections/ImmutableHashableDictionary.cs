using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace JavaUtilities.Collections{
    /// <summary>
    /// An immutable <see cref="IDictionary{TKey, TValue}"/> implementation
    /// which implements <see cref="Equals(object)"/> and <see cref="GetHashCode"/>
    /// based on the contents of the dictionary. Note that this type implements
    /// <see cref="Add(K, V)"/> and <see cref="Set(K, V)"/> methods, which
    /// return new <see cref="ImmutableHashableDictionary{K, V}"/> instances.
    /// </summary>
    /// <typeparam name="K">The type of the keys in the dictionary</typeparam>
    /// <typeparam name="V">The type of the values in the dictionary</typeparam>
    public class ImmutableHashableDictionary<K, V> : IReadOnlyDictionary<K, V> where K : notnull{
        /*
         * Fields
         */
        private readonly IDictionary<K, V> storage;


        /*
         * Properties
         */
        /// <summary>
        /// Gets the number of elements in this collection.
        /// </summary>
        public int Count => storage.Count;
        /// <summary>
        /// Gets an <see cref="IEnumerable{K}"/> for the set of unique keys in this collection.
        /// </summary>
        public IEnumerable<K> Keys => storage.Keys;
        /// <summary>
        /// Gets an <see cref="IEnumerable{V}"/> for the set of values in this collection.
        /// </summary>
        public IEnumerable<V> Values => storage.Values;

        /// <summary>
        /// Gets the given item in the dictionary. If the key is not found,
        /// throws a <see cref="KeyNotFoundException"/>.
        /// </summary>
        /// <param name="key">The key to get the corresponding value of</param>
        /// <returns>The value corresponding to the given key</returns>
        public V this[K key] => storage[key];


        /*
         * Constructor
         */
        /// <summary>
        /// Initializes a new ImmutableHashableDictionary containing the same
        /// set of key, value pairs as the given dictionary.
        /// </summary>
        /// <param name="dictIn"></param>
        public ImmutableHashableDictionary(IDictionary<K, V> dictIn){
            storage = new Dictionary<K, V>();
            foreach(KeyValuePair<K, V> pair in dictIn){
                storage.Add(pair);
            }
        }
        
        /// <summary>
        /// Initializes an empty ImmutableHashableDictionary.
        /// </summary>
        public ImmutableHashableDictionary()
            :this(new Dictionary<K, V>()){}
        
        
        /*
         * Methods
         */
        /// <summary>
        /// Returns a new <see cref="ImmutableHashableDictionary{K, V}"/> with
        /// the given value added to it. If the key is already present, throws
        /// an <see cref="ArgumentException"/>
        /// </summary>
        /// <param name="newKey">The new key to add to the dictionary</param>
        /// <param name="newValue">The new value for the given key</param>
        /// <returns>A new <see cref="ImmutableHashableDictionary{K, V}"/>
        /// instance containing the given key, value pair</returns>
        public ImmutableHashableDictionary<K, V> Add(K newKey, V newValue){
            Dictionary<K, V> newStorage = new(storage){
                [newKey] = newValue
            };
            return new ImmutableHashableDictionary<K, V>(newStorage);
        }
        
        /// <summary>
        /// Returns a new <see cref="ImmutableHashableDictionary{K, V}"/> with
        /// the given key set to the given value, regardless of whether it was
        /// already present.
        /// </summary>
        /// <param name="key">The key to add or update in the dictionary</param>
        /// <param name="newValue">The new value for the given key</param>
        /// <returns>A new <see cref="ImmutableHashableDictionary{K, V}"/>
        /// instance containing the given key, value pair</returns>        
        public ImmutableHashableDictionary<K, V> Set(K key, V newValue){
            Dictionary<K ,V> newStorage = new(storage){
                [key] = newValue
            };
            return new ImmutableHashableDictionary<K, V>(newStorage);
        }
        
        /// <summary>
        /// Checks if this <see cref="ImmutableHashableDictionary{K, V}"/>
        /// contains the given key.
        /// </summary>
        /// <param name="key">The key to check for</param>
        /// <returns>True if the given key was found, otherwise false</returns>
        public bool ContainsKey(K key){
            return storage.ContainsKey(key);
        }
        
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="value">When this method returns true, contains the
        /// value associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the value parameter.
        /// This parameter is passed uninitialized.</param>
        /// <returns>True if the <see cref="ImmutableHashableDictionary{K, V}"/>
        /// contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(K key, out V value){
            return storage.TryGetValue(key, out value!);
        }
        
        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
        
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{K, V}"/>s
        /// containing all key, value pairs in this
        /// <see cref="ImmutableHashableDictionary{K, V}"/>, in arbitrary order.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{K, V}"/>s
        /// containing all key, value pairs in this dictionary</returns>
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator(){
            return storage.GetEnumerator();
        }
        
        /// <summary>
        /// Returns a string representation of this dictionary's contents, in
        /// the format <c>{key=value, key=value, etc.}</c>
        /// </summary>
        /// <returns>A string representation of the contents of this dictionary</returns>
        public override string ToString(){
            StringBuilder builder = new("{");
            bool firstEntryAdded = false;
            foreach(KeyValuePair<K, V> pair in this){
                if(!firstEntryAdded){
                    firstEntryAdded = true;
                }else{
                    builder.Append(", ");
                }
                builder.Append($"{pair.Key}={pair.Value}");
            }
            return builder.Append('}').ToString();
        }
        
        /// <summary>
        /// Checks if this <see cref="ImmutableHashableDictionary{K, V}"/>
        /// is equal to the given one. Instances will evaluate to equal if
        /// they have the same contents, even if they are not the same object.
        /// </summary>
        /// <param name="obj">The <see cref="ImmutableHashableDictionary{K, V}"/>
        /// instance to compare to</param>
        /// <returns>True if this <see cref="ImmutableHashableDictionary{K, V}"/>
        /// is equal to the given one, otherwise false</returns>
        public override bool Equals(object? obj){
            if(obj == this)
                return true;
            if(obj is not ImmutableHashableDictionary<K, V> otherDict){
                return false;
            }
            foreach(KeyValuePair<K, V> pair in otherDict){
                if(!ContainsKey(pair.Key))
                    return false;
                V value = this[pair.Key];
                if(!pair.Value?.Equals(value) ?? value != null)
                    return false;
            }
            foreach(KeyValuePair<K, V> pair in this){
                if(!otherDict.ContainsKey(pair.Key))
                    return false;
                V otherValue = otherDict[pair.Key];
                if(!pair.Value?.Equals(otherValue) ?? otherValue != null)
                    return false;
            }
            return true;
        }
        
        /// <summary>
        /// Gets an integer that is reasonably unique for the contents of this
        /// <see cref="ImmutableHashableDictionary{K, V}"/>. Two instances with
        /// the same contents will always return the same hashcode. The risk of
        /// a hash collision is determined by the hash functions of
        /// <typeparamref name="K"/> and <typeparamref name="V"/>.
        /// </summary>
        /// <returns>A semi-unique integer for the contents of this dictionary</returns>
        public override int GetHashCode(){
            int hashCode = typeof(ImmutableHashableDictionary<K, V>).GetHashCode();
            foreach(KeyValuePair<K, V> pair in storage){
                hashCode ^= pair.GetHashCode();
            }
            return hashCode;
        }
    }
}
