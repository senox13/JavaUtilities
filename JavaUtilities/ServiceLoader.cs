using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace JavaUtilities{
    /// <summary>
    /// Provides a facility for loading implementations of a service. A
    /// service is defined as a publicly visible interface or class. A
    /// service provider is defined as an implementation or subclass of the
    /// service type which contains either a public zero-argument constructor
    /// or a public static zero-argument factory method named <c>Provider</c>.
    /// </summary>
    /// <typeparam name="S">The type representing the service this loader
    /// loads</typeparam>
    public sealed class ServiceLoader<S> : IEnumerable<S>{
        /*
         * Fields
         */
        private readonly Type service;
        private readonly IEnumerator<Type> assemblyEnumerator;
        private readonly List<IProvider> loadedProviders = new List<IProvider>();
        private readonly List<S> instantiatedProviders = new List<S>();
        private bool loadedAllProviders;


        /*
         * Constructors
         */
        internal ServiceLoader(Assembly assembly){
            service = typeof(S);
            assemblyEnumerator = assembly.ExportedTypes.GetEnumerator();
        }


        /*
         * Private methods
         */
        /// <summary>
        /// Attempts to load the next valid service type from
        /// <c>assemblyEnumerator</c> and cache both the instance and the
        /// provider for that type.
        /// </summary>
        /// <returns><c>true</c> if a new service type was found and cached,
        /// otherwise <c>false</c></returns>
        private bool LoadAndCacheNextService(){
            if(loadedAllProviders)
                return false;
            //TODO: Make this whole class thread-safe at some point
            //May just be able to wrap this in a lock to be thread-safe-ish
            while(assemblyEnumerator.MoveNext()){
                Type cand = assemblyEnumerator.Current;
                if(service.IsAssignableFrom(cand) && !cand.IsAbstract){
                    //Type implements service, check for factory method
                    IProvider provider;
                    MethodInfo factoryMethod = cand.GetMethod("Provider", 0, BindingFlags.Public | BindingFlags.Static, null, Type.EmptyTypes, null);
                    if(factoryMethod != null){
                        provider = new ProviderImpl(cand, factoryMethod);
                    }else{
                        //Factory method not found, check for no-arg constructor
                        ConstructorInfo ctor = cand.GetConstructor(Type.EmptyTypes);
                        if(ctor != null){
                            provider = new ProviderImpl(cand, ctor);
                        }else{
                            //Failed to find valid factory method or constructor, skip this type
                            continue;
                        }
                    }
                    S instance = provider.Get();
                    //Add to caches
                    loadedProviders.Add(provider);
                    instantiatedProviders.Add(instance);
                    return true;
                }
            }
            loadedAllProviders = true;
            return false;
        }


        /*
         * Instance methods
         */
        /// <summary>
        /// Gets an <see cref="IEnumerable{IProvider}"/> of the providers
        /// which represents implementing instances of this service.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IProvider> GetSuppliers(){
            if(loadedAllProviders){
                return loadedProviders;
            }
            return new EnumeratorWrapper<IProvider>(() => new ServiceProviderEnumerator(this));
        }

        /// <summary>
        /// Returns an <see cref="Optional{S}"/> containing the first
        /// available implementation of this <see cref="ServiceLoader{S}"/>'s
        /// service, or an empty optional if no implementaion of the service
        /// is available. The order by which the first implementation is
        /// determined is completely arbitrary.
        /// </summary>
        /// <returns>The first available implementaiton of this loader's
        /// service</returns>
        public Optional<S> FindFirst(){
            IEnumerator<S> enumerator = GetEnumerator();
            if(enumerator.MoveNext()){
                return Optional.Of(enumerator.Current);
            }
            return Optional.Empty<S>();
        }

        /// <summary>
        /// Resets this <see cref="ServiceLoader{S}"/>, clearing its cache.
        /// </summary>
        public void Reload(){
            assemblyEnumerator.Reset();
            loadedAllProviders = false;
            instantiatedProviders.Clear();
            loadedProviders.Clear();
        }


        /*
         * IEnumerable implementation
         */
        /// <inheritdoc/>
        public IEnumerator<S> GetEnumerator(){
            if(loadedAllProviders){
                return instantiatedProviders.GetEnumerator();
            }
            return new ServiceEnumerator(this);
        }
        
        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }


        /*
         * Object override methods
         */
        /// <inheritdoc/>
        public override string ToString(){
            return $"ServiceLoader[{service.Name}]";
        }


        /*
         * Nested types
         */
        /// <summary>
        /// Represents a provider for a service implementation.
        /// </summary>
        public interface IProvider{
            /// <summary>
            /// Gets the most-derived implementing <see cref="Type"/> of the
            /// service represented by this <see cref="IProvider"/>.
            /// </summary>
            /// <returns>The type of this service implementation</returns>
            Type GetServiceImplType();

            /// <summary>
            /// Gets the instance of this service implementation.
            /// </summary>
            /// <returns>The instance of this service represented by this
            /// provider.</returns>
            S Get();
        }

        private sealed class ProviderImpl : IProvider{
            /*
             * Fields
             */
            private readonly Type type;
            private readonly MethodInfo factoryMethod;
            private readonly ConstructorInfo ctor;


            /*
             * Constructor
             */
            public ProviderImpl(Type typeIn, MethodInfo factoryMethodIn){
                type = typeIn;
                factoryMethod = factoryMethodIn;
            }

            public ProviderImpl(Type typeIn, ConstructorInfo ctorIn){
                type = typeIn;
                ctor = ctorIn;
            }


            /*
             * IProvider implementation
             */
            public Type GetServiceImplType(){
                return type;
            }

            public S Get(){
                if(factoryMethod != null){
                    return (S)factoryMethod.Invoke(null, null);
                }
                return (S)ctor.Invoke(null);
            }
        }

        private sealed class EnumeratorWrapper<T> : IEnumerable<T>{
            /*
             * Fields
             */
            private readonly Func<IEnumerator<T>> enumeratorSupplier;


            /*
             * Constructor
             */
            public EnumeratorWrapper(Func<IEnumerator<T>> supplierIn){
                enumeratorSupplier = supplierIn;
            }


            /*
             * IEnumerable implementation
             */
            public IEnumerator<T> GetEnumerator(){
                return enumeratorSupplier.Invoke();
            }

            IEnumerator IEnumerable.GetEnumerator(){
                return GetEnumerator();
            }
        }

        private sealed class ServiceEnumerator : IEnumerator<S>{
            /*
             * Fields
             */
            private readonly ServiceLoader<S> sl;
            private int index = -1;


            /*
             * Properties
             */
            public S Current{get{
                if(index < 0)
                    throw new InvalidOperationException("MoveNext must be called before accessing enumerator's current value");
                if(index >= sl.instantiatedProviders.Count)
                    throw new InvalidOperationException("End of enumerator exceeded");
                return sl.instantiatedProviders[index];
            }}

            object IEnumerator.Current => Current;


            /*
             * Constructor
             */
            public ServiceEnumerator(ServiceLoader<S> serviceLoader){
                sl = serviceLoader;
            }


            /*
             * IEnumerator implementation
             */
            public bool MoveNext(){
                index++;
                if(index < sl.instantiatedProviders.Count){
                    return true;
                }
                return sl.LoadAndCacheNextService();
            }
            
            public void Reset(){
                index = -1;
            }

            public void Dispose(){}
        }

        private sealed class ServiceProviderEnumerator : IEnumerator<IProvider>{
            /*
             * Fields
             */
            private readonly ServiceLoader<S> sl;
            private int index = -1;


            /*
             * Properties
             */
            public IProvider Current{get{
                if(index < 0)
                    throw new InvalidOperationException("MoveNext must be called before accessing enumerator's current value");
                if(index >= sl.loadedProviders.Count)
                    throw new InvalidOperationException("End of enumerator exceeded");
                return sl.loadedProviders[index];
            }}

            object IEnumerator.Current => Current;


            /*
             * Constructor
             */
            public ServiceProviderEnumerator(ServiceLoader<S> serviceLoader){
                sl = serviceLoader;
            }


            /*
             * IEnumerator implementation
             */
            public bool MoveNext(){
                index++;
                if(index < sl.instantiatedProviders.Count){
                    return true;
                }
                return sl.LoadAndCacheNextService();
            }
            
            public void Reset(){
                index = -1;
            }

            public void Dispose(){}
        }
    }

    /// <summary>
    /// Provides static methods for creating <see cref="ServiceLoader{S}"/>
    /// instances.
    /// </summary>
    /// <seealso cref="ServiceLoader{S}"/>
    public static class ServiceLoader{
        /*
         * Static methods
         */
        /// <summary>
        /// Creates a new <see cref="ServiceLoader{S}"/> for service
        /// implementations within the calling assembly. This is equivalent to
        /// <c>ServiceLoader.Load&lt;S&gt;(Assembly.GetCallingAssembly())</c>.
        /// </summary>
        /// <typeparam name="S">The service type to load</typeparam>
        /// <returns>A new ServiceLoader instance for the given generic type</returns>
        public static ServiceLoader<S> Load<S>(){
            return new ServiceLoader<S>(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Creates a new <see cref="ServiceLoader{S}"/> for service
        /// implementations within the given assembly.
        /// </summary>
        /// <typeparam name="S">The service type to load</typeparam>
        /// <param name="assembly">The assembly to load services from</param>
        /// <returns>A new ServiceLoader instance for the given generic type
        /// and assembly</returns>
        public static ServiceLoader<S> Load<S>(Assembly assembly){
            return new ServiceLoader<S>(assembly);
        }
    }
}
