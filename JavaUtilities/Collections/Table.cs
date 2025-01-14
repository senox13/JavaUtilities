using System;
using System.Collections;
using System.Collections.Generic;


namespace JavaUtilities.Collections{
    /// <summary>
    /// Provides a container which associates an ordered pair of keys to a
    /// value. This can be visualized as a 2D table, with the pair of keys
    /// being the row and column indices and the values being table cells.
    /// Functionally however, this is simply a <see cref="Dictionary{TKey, TValue}"/>
    /// with two keys per value.
    /// </summary>
    /// <typeparam name="R">The type of the row keys</typeparam>
    /// <typeparam name="C">The type of the column keys</typeparam>
    /// <typeparam name="V">The type of the table values</typeparam>
    public class Table<R, C, V> : IEnumerable<Tuple<R, C, V>>{
        /*
         * Fields
         */
        private const string ERR_BAD_KEY = "The provided key pair could not be found in the table";
        private readonly Dictionary<Tuple<R, C>, V> tableStorage;
        
        
        /*
         * Constructors
         */
        /// <summary>
        /// Constructs a new, empty table.
        /// </summary>
        public Table(){
            tableStorage = [];
        }
        
        /// <summary>
        /// Constructs a new <see cref="Table{R, C, V}"/> with capacity
        /// pre-allocated for the specified number of rows and columns. Note
        /// that this assumes that all cells in the table will be filled. If
        /// this will not be the case, <see cref="Table(int)"/> should be used
        /// instead.
        /// </summary>
        /// <param name="expectedRows">The number of rows expected to be in
        /// this table</param>
        /// <param name="expectedColumns">The number of columns expected to be
        /// in this table</param>
        public Table(int expectedRows, int expectedColumns){
            tableStorage = new(expectedRows * expectedColumns);
        }

        /// <summary>
        /// Constructs a new <see cref="Table{R, C, V}"/> with capacity
        /// pre-allocated for the specified number of cells.
        /// </summary>
        /// <param name="expectedCells">The number of cells expected to be in
        /// this table</param>
        public Table(int expectedCells){
            tableStorage = new Dictionary<Tuple<R, C>, V>(expectedCells);
        }
        
        /// <summary>
        /// Constructs a new <see cref="Table{R, C, V}"/> using the given
        /// <see cref="IEnumerable{T}"/> of <see cref="Tuple{R, C, V}"/>s
        /// containing a row, column, and cell value, respectively.
        /// </summary>
        /// <param name="values">The contents of the table to be constructed</param>
        public Table(IEnumerable<Tuple<R, C, V>> values) : this(){
            AddAll(values);
        }
        
        
        /*
         * Properties
         */
        /// <summary>
        /// Gets or sets the item with the given pair of keys in this table.
        /// If the specified key is not found when getting, throws a
        /// <see cref="KeyNotFoundException"/>.
        /// </summary>
        /// <param name="rowKey">The row key of the cell to get or set</param>
        /// <param name="colKey">The column key of the cell to get or set</param>
        public V this[R rowKey, C colKey]{
            get{
                Tuple<R, C> keyPair = new(rowKey, colKey);
                if(!tableStorage.TryGetValue(keyPair, out V? value))
                    throw new KeyNotFoundException(ERR_BAD_KEY);
                return value;
            }
            set{
                Tuple<R, C> keyPair = new(rowKey, colKey);
                tableStorage[keyPair] = value;
            }
        }
        
        /// <summary>
        /// Gets the number of entries in this table.
        /// </summary>
        public int Count => tableStorage.Count;
        
        /// <summary>
        /// Gets an <see cref="IEnumerable{R}"/> containing the unique row
        /// keys in this <see cref="Table{R, C, V}"/>.
        /// </summary>
        public IEnumerable<R> RowKeys{
            get{
                HashSet<R> rowVals = [];
                foreach(Tuple<R, C> pair in tableStorage.Keys){
                    rowVals.Add(pair.Item1);
                }
                return rowVals;
            }
        }
        
        /// <summary>
        /// Gets an <see cref="IEnumerable{C}"/> containing the unique column
        /// keys in this <see cref="Table{R, C, V}"/>.
        /// </summary>
        public IEnumerable<C> ColKeys{
            get{
                HashSet<C> rowVals = [];
                foreach(Tuple<R, C> pair in tableStorage.Keys){
                    rowVals.Add(pair.Item2);
                }
                return rowVals;
            }
        }

        /// <summary>
        /// Gets an <see cref="ICollection{V}"/> containing all of the cell
        /// values stored in this <see cref="Table{R, C, V}"/>.
        /// </summary>
        public ICollection<V> Values{
            get{
                List<V> vals = [..tableStorage.Values];
                return vals;
            }
        }
        
        
        /*
         * Public methods
         */
        /// <summary>
        /// Checks if the given pair of keys is present in the table.
        /// </summary>
        /// <param name="rowKey">The row key to check for</param>
        /// <param name="colKey">The column key to check for</param>
        /// <returns>True if the given pair of keys is found, otherwise false</returns>
        public bool Contains(R rowKey, C colKey){
            return tableStorage.ContainsKey(new Tuple<R, C>(rowKey, colKey));
        }
        
        /// <summary>
        /// Checks if the given row key is present in the table.
        /// </summary>
        /// <param name="key">The row key to check for</param>
        /// <returns>True if the given key is found, otherwise false</returns>
        public bool ContainsRow(R key){
            foreach(Tuple<R, C> pair in tableStorage.Keys){
                if(pair.Item1?.Equals(key) ?? key == null)
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// Checks if the given column key is present in the table.
        /// </summary>
        /// <param name="key">The column key to check for</param>
        /// <returns>True if the given key is found, otherwise false</returns>
        public bool ContainsColumn(C key){
            foreach(Tuple<R, C> pair in tableStorage.Keys){
                if(pair.Item2?.Equals(key) ?? key == null)
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// Checks if the given value is present in the table.
        /// </summary>
        /// <param name="value">The value to check for</param>
        /// <returns>True if the given value is found, otherwise false</returns>
        public bool ContainsValue(V value){
            return tableStorage.ContainsValue(value);
        }
        
        /// <summary>
        /// Removes all values from the table, resetting it to an empty state.
        /// </summary>
        public void Clear(){
            tableStorage.Clear();
        }
        
        /// <summary>
        /// Adds the given value to the table, associated with the given pair
        /// of keys. Throws an <see cref="ArgumentException"/> if the given
        /// pair of keys is already present in the table.
        /// </summary>
        /// <param name="rowKey">The row key to store the new value with</param>
        /// <param name="colKey">The column key to store the new value with</param>
        /// <param name="val">The value to add to the table</param>
        public void Add(R rowKey, C colKey, V val){
            tableStorage.Add(new Tuple<R, C>(rowKey, colKey), val);
        }

        /// <summary>
        /// Adds all values in the given <see cref="IEnumerable{T}"/> of
        /// <see cref="Tuple{R, C, V}"/>s to the table. Values should be in
        /// row, column, value order.
        /// </summary>
        /// <param name="values">The mappings to add to the table</param>
        public void AddAll(IEnumerable<Tuple<R, C, V>> values){
            foreach(Tuple<R, C, V> cell in values){
                Add(cell.Item1, cell.Item2, cell.Item3);
            }
        }
        
        /// <summary>
        /// Attempts to remove a row, column pair from the <see cref="Table{R, C, V}"/>.
        /// </summary>
        /// <param name="row">The row key to be removed</param>
        /// <param name="col">The column key to be removed</param>
        /// <returns><c>true</c> if the value at the given row and column was
        /// successfully removed, otherwise <c>false</c></returns>
        public bool Remove(R row, C col){
            return tableStorage.Remove(new Tuple<R, C>(row, col));
        }
        
        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }

        /// <summary>
        /// Gets and <see cref="IEnumerable{T}"/> of the <see cref="Tuple{R, C, V}"/>
        /// row, column, value mappings in this <see cref="Table{R, C, V}"/>.
        /// </summary>
        /// <returns>The mappings in this table</returns>
        public IEnumerator<Tuple<R, C, V>> GetEnumerator(){
            foreach(KeyValuePair<Tuple<R, C>, V> kvPair in tableStorage){
                yield return new Tuple<R, C, V>(kvPair.Key.Item1, kvPair.Key.Item2, kvPair.Value);
            }
        }
    }
}
