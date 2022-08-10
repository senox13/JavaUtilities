using System.Collections;
using System.Collections.Generic;


namespace JavaUtilities.Collections{
    /// <summary>
	/// Provides an implementation of <see cref="IList{T}"/> which is fully
    /// thread-safe. Can optionally wrap another implementation of
    /// <see cref="IList{T}"/> and provide thread-safe access to its
    /// interface methods.
    /// </summary>
    /// <typeparam name="T">The type contained in the list</typeparam>
    public class SynchronizedList<T> : IList<T>{
        /*
         * Fields
         */
        private readonly IList<T> list;
        private readonly object _lock = new object();


        /*
         * Properties
         */
        /// <inheritdoc/>
        public T this[int index]{
            get{
                lock(_lock){
                    return list[index];
                }
            }
            set{
                lock(_lock){
                    list[index] = value;
                }
            }
        }

        /// <inheritdoc/>
        public int Count{
            get{
                lock(_lock){
                    return list.Count;
                }
            }
        }

        /// <inheritdoc/>
        public bool IsReadOnly{
            get{
                lock(_lock){
                    return list.IsReadOnly;
                }
            }
        }
		
		
        /*
         * Constructor
         */
        /// <summary>
        /// Constructs a new, empty <see cref="SynchronizedList{T}"/>.
        /// </summary>
        public SynchronizedList() : this(new List<T>()){}
        
        /// <summary>
        /// Constructs a new, empty <see cref="SynchronizedList{T}"/> with
        /// the given capacity pre-allocated.
        /// </summary>
        /// <param name="capacity">The number of entries to pre-allocate
        /// memory for</param>
        public SynchronizedList(int capacity) : this(new List<T>(capacity)){}
        
        /// <summary>
        /// Constructs a new <see cref="SynchronizedList{T}"/> containing the
        /// entries provided by the given <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="collection">The collection of values to put in the
        /// new list</param>
        public SynchronizedList(IEnumerable<T> collection) : this(new List<T>(collection)){}

        /// <summary>
        /// Constructs a new <see cref="SynchronizedList{T}"/> to wrap the
        /// given <see cref="IList{T}"/> instance to allow thread-safe
        /// access to its interface methods.
        /// </summary>
        /// <param name="listIn">The list instance to wrap</param>
        public SynchronizedList(IList<T> listIn){
            list = listIn;
        }

        /*
         * Public functions
         */
        /// <inheritdoc/>
        public void Add(T item){
            lock(_lock){
                list.Add(item);
            }
        }

        /// <inheritdoc/>
        public void Clear(){
            lock(_lock){
                list.Clear();
            }
        }

        /// <inheritdoc/>
        public bool Contains(T item){
            lock(_lock){
                return list.Contains(item);
            }
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex) {
            lock(_lock){
                list.CopyTo(array, arrayIndex);
            }
        }

        /// <inheritdoc/>
		public int IndexOf(T item){
            lock(_lock){
                return list.IndexOf(item);
            }
        }

        /// <inheritdoc/>
        public void Insert(int index, T item){
            lock(_lock){
                list.Insert(index, item);
            }
        }

        /// <inheritdoc/>
        public bool Remove(T item){
            lock(_lock){
                return list.Remove(item);
            }
        }

        /// <inheritdoc/>
        public void RemoveAt(int index){
            lock(_lock){
                list.RemoveAt(index);
            }
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator(){
            lock(_lock){
                return list.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator(){
            return ((IEnumerable)list).GetEnumerator();
        }
    }
}
