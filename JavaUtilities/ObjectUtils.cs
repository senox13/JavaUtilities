using System;

namespace JavaUtilities{
    /// <summary>
    /// Provides utility methods for handling objects and arrays of objects
    /// with null safety.
    /// </summary>
    public static class ObjectUtils{
        /// <summary>
        /// Calls the given <see cref="Action{T}"/> with the given <typeparamref name="T"/>
        /// instance, then returns the given <typeparamref name="T"/> instance.
        /// This method is primarily intended to allow for static initialization
        /// without a static block.
        /// </summary>
        /// <typeparam name="T">The type of the object being initialized</typeparam>
        /// <param name="instance">The instance to initialize then return</param>
        /// <param name="initializer">The method to initilized the given instance</param>
        /// <returns>The given <typeparamref name="T"/> instance</returns>
        public static T Make<T>(T instance, Action<T> initializer) where T : class{
            initializer(instance);
            return instance;
        }

        /// <summary>
        /// Checks if the given objects are equal. If both objects are null,
        /// returns <c>true</c>.
        /// </summary>
        /// <param name="a">The first object to compare</param>
        /// <param name="b">The second object to compare</param>
        /// <returns><c>true</c> if the given objects are equal, otherwise
        /// <c>false</c></returns>
        public static new bool Equals(object a, object b){
            if(a == b){
                return true;
            }
            return a != null && a.Equals(b);
        }

        /// <summary>
        /// If the given object is null, returns <c>0</c>. Otherwise, returns
        /// the given object's hashcode.
        /// </summary>
        /// <param name="o">The object to get the hashcode of</param>
        /// <returns>The hashcode of the given object if non-<c>null</c>,
        /// othwerise <c>0</c></returns>
        public static int HashCode(object o){
            if(o == null) return 0;
            return o.GetHashCode();
        }

        /// <summary>
        /// Returns a semi-unique integer based on the contents of the given
        /// objects. The chance of hash colisions is determined by the hash
        /// functions of the given objects.
        /// </summary>
        /// <param name="objects">The array of objects to hash together</param>
        /// <returns>A semi-unique integer based on the contents of the given
        /// array</returns>
        public static int Hash(params object[] objects){
            return ArrayUtils.HashCode(objects);
        }

        /// <summary>
        /// Returns the result of the given <see cref="object"/>'s
        /// <see cref="object.ToString"/> method, or <c>"null"</c> if the
        /// given object is <c>null</c>.
        /// </summary>
        /// <param name="o">The object to convert to a string</param>
        /// <returns>The string form of the given object</returns>
        public static string ToString(object o){
            return ToString(o, "null");
        }

        /// <summary>
        /// Returns the result of the given <see cref="object"/>'s
        /// <see cref="object.ToString"/> method, or the given default string
        /// if the given object is <c>null</c>.
        /// </summary>
        /// <param name="o">The object to convert to a string</param>
        /// <param name="nullDefault">The default string to return if the
        /// given object is null</param>
        /// <returns>The string form of the given object, or the given default
        /// if the given object is null</returns>
        public static string ToString(object o, string nullDefault){
            if(o == null)
                return nullDefault;
            return o.ToString();
        }
    }
}
