using System;
using System.Diagnostics;


namespace JavaUtilities{
    /// <summary>
    /// Provides utilities for getting the current time
    /// </summary>
    public static class TimeUtils{
        /// <summary>
        /// Gets the current time in miliseconds. Note that the value returned
        /// by this method will increase at the expected rate, but has no
        /// correlation to the actual system time.
        /// </summary>
        /// <returns>A milisecond representation of the current time</returns>
        public static long MiliTime(){
            long nano = Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            return nano;
        }

        /// <summary>
        /// Gets the current time in nanoseconds. Note that the value returned
        /// by this method will increase at the expected rate, but has no
        /// correlation to the actual system time.
        /// </summary>
        /// <returns>A nanosecond representation of the current time</returns>
        public static long NanoTime(){
            return MiliTime() * 100L;
        }
    }
}
