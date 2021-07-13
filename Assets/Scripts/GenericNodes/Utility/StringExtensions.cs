using System;

namespace GenericNodes.Utility {
    public static class StringExtensions {
        public static string Reverse(this string text)
        {
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return (new string(array));
        }
    }
}