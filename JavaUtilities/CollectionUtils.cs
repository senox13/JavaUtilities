using System;
using System.Collections.Generic;

namespace JavaUtilities{
    /// <summary>
    /// Provides utility methods for working with <see cref="IEnumerable{T}"/>
    /// instances and collections.
    /// </summary>
    public static class CollectionUtils{
        /// <summary>
        /// Compares the contents of the two given
        /// <see cref="IEnumerable{T}"/>s and returns <c>true</c> if their
        /// contents are equal, otherwise <c>false</c>.
        /// </summary>
        /// <typeparam name="T">The generic type of the two given enumerables</typeparam>
        /// <param name="a">The first enumerable to compare</param>
        /// <param name="b">The second enumerable to compare</param>
        /// <returns><c>true</c> if the given enumerables have equal contents,
        /// otherwise <c>false</c></returns>
        public static bool ContentsEqual<T>(IEnumerable<T> a, IEnumerable<T> b){
            if(a == b)
                return true;
            if(a == null || b == null)
                return false;
            using IEnumerator<T> enumA = a.GetEnumerator();
            using IEnumerator<T> enumB = b.GetEnumerator();
            while(true){
                bool nextA = enumA.MoveNext();
                bool nextB = enumB.MoveNext();
                if(!nextA && !nextB)
                    return true;
                if(!nextA || !nextB)
                    return false;
                if(!ObjectUtils.Equals(enumA.Current, enumB.Current))
                    return false;
            }
        }

        /// <summary>
        /// Returns a semi-unique integer based on the contents of the given
        /// <see cref="IEnumerable{T}"/>. The chance of hash collisions is
        /// determined by the hash function of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable. Should usually not
        /// need to be explicitly specified</typeparam>
        /// <param name="items">The array to hash</param>
        /// <returns>Returns a semi-unique integer based on the contents of
        /// the given enumerable</returns>
        public static int HashCode<T>(IEnumerable<T> items){
            if(items == null)
                return 0;
            int retVal = 1;
            foreach(T elem in items){
                retVal = 31 * retVal + ((elem == null) ? 0 : elem.GetHashCode());
            }
            return retVal;
        }

        /// <summary>
        /// Gets a random element from the given collection using the given
        /// <see cref="Random"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the given
        /// collection</typeparam>
        /// <param name="collection">The collection to pick an element from</param>
        /// <param name="rand">The random generator to use to pick an element</param>
        /// <returns>A randomly selected element from the given collection</returns>
        public static T GetRandom<T>(IList<T> collection, Random rand){
            return collection[rand.Next(collection.Count)];
        }
    }
}
