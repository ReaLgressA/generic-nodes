using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniJSON {
    public static class HashtableExtensions {
        public static T ReadAs<T>(this Hashtable ht, string key, Func<Hashtable, T> solver, T defaultValue = default) {
            if (ht.ContainsKey(key) && ht[key] != null) {
                T entry = solver((ht[key] as Hashtable));
                if (entry != null) {
                    return entry;
                }
            }

            return defaultValue;
        }

        public static List<T> ReadAsGenericList<T>(this Hashtable ht, string key, Func<Hashtable, T> genericSolver) {
            List<T> entries = new List<T>();
            if (ht.ContainsKey(key) && ht[key] != null) {
                ArrayList array = ht[key] as ArrayList;
                if (array == null) {
                    return entries;
                }

                for (int i = 0; i < array.Count; ++i) {
                    T entry = genericSolver((array[i] as Hashtable));
                    if (entry != null) {
                        entries.Add(entry);
                    }
                }
            }

            return entries;
        }
    }
}

