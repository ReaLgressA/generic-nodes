using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

/* Based on the JSON parser from 
 * http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
 * 
 * I simplified it so that it doesn't throw exceptions
 * and can be used in Unity iPhone with maximum code stripping.
 */
/// <summary>
/// This class encodes and decodes JSON strings.
/// Spec. details, see http://www.json.org/
/// 
/// JSON uses Arrays and Objects. These correspond here to the datatypes ArrayList and Hashtable.
/// All numbers are parsed to doubles.
/// </summary>
public class MiniJSON {
    private const int TOKEN_NONE = 0;
    private const int TOKEN_CURLY_OPEN = 1;
    private const int TOKEN_CURLY_CLOSE = 2;
    private const int TOKEN_SQUARED_OPEN = 3;
    private const int TOKEN_SQUARED_CLOSE = 4;
    private const int TOKEN_COLON = 5;
    private const int TOKEN_COMMA = 6;
    private const int TOKEN_STRING = 7;
    private const int TOKEN_NUMBER = 8;
    private const int TOKEN_TRUE = 9;
    private const int TOKEN_FALSE = 10;
    private const int TOKEN_NULL = 11;
    private const int BUILDER_CAPACITY = 2000;

    /// <summary>
    /// On decoding, this value holds the position at which the parse failed (-1 = no error).
    /// </summary>
    protected static int lastErrorIndex = -1;
    protected static string lastDecode = "";


    /// <summary>
    /// Parses the string json into a value
    /// </summary>
    /// <param name="json">A JSON string.</param>
    /// <returns>An ArrayList, a Hashtable, a double, a string, null, true, or false</returns>
    public static object JsonDecode(string json) {
        // save the string for debug information
        MiniJSON.lastDecode = json;

        if (json != null) {
            char[] charArray = json.ToCharArray();
            int index = 0;
            bool success = true;
            object value = MiniJSON.ParseValue(charArray, ref index, ref success);

            MiniJSON.lastErrorIndex = success ? -1 : index;

            return value;
        } else {
            return null;
        }
    }


    /// <summary>
    /// Converts a Hashtable / ArrayList / Dictionary(string,string) object into a JSON string
    /// </summary>
    /// <param name="json">A Hashtable / ArrayList</param>
    /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
    public static string JsonEncode(object json, bool prettyPrint = false) {
        StringBuilder builder = new StringBuilder(BUILDER_CAPACITY);
        bool success = prettyPrint ? MiniJSON.SerializeValuePretty(json, builder, 0) : MiniJSON.SerializeValue(json, builder);

        return (success ? builder.ToString() : null);
    }


    /// <summary>
    /// On decoding, this function returns the position at which the parse failed (-1 = no error).
    /// </summary>
    /// <returns></returns>
    public static bool LastDecodeSuccessful() {
        return (MiniJSON.lastErrorIndex == -1);
    }


    /// <summary>
    /// On decoding, this function returns the position at which the parse failed (-1 = no error).
    /// </summary>
    /// <returns></returns>
    public static int GetLastErrorIndex() {
        return MiniJSON.lastErrorIndex;
    }


    /// <summary>
    /// If a decoding error occurred, this function returns a piece of the JSON string 
    /// at which the error took place. To ease debugging.
    /// </summary>
    /// <returns></returns>
    public static string GetLastErrorSnippet() {
        if (MiniJSON.lastErrorIndex == -1) {
            return "";
        } else {
            int startIndex = MiniJSON.lastErrorIndex - 5;
            int endIndex = MiniJSON.lastErrorIndex + 15;
            if (startIndex < 0) {
                startIndex = 0;
            }

            if (endIndex >= MiniJSON.lastDecode.Length) {
                endIndex = MiniJSON.lastDecode.Length - 1;
            }

            return MiniJSON.lastDecode.Substring(startIndex, endIndex - startIndex + 1);
        }
    }


    #region Parsing

    protected static Hashtable ParseObject(char[] json, ref int index) {
        Hashtable table = new Hashtable();
        int token;

        // {
        NextToken(json, ref index);

        bool done = false;
        while (!done) {
            token = LookAhead(json, index);
            if (token == MiniJSON.TOKEN_NONE) {
                return null;
            } else if (token == MiniJSON.TOKEN_COMMA) {
                NextToken(json, ref index);
            } else if (token == MiniJSON.TOKEN_CURLY_CLOSE) {
                NextToken(json, ref index);
                return table;
            } else {
                // name
                string name = ParseString(json, ref index);
                if (name == null) {
                    return null;
                }

                // :
                token = NextToken(json, ref index);
                if (token != MiniJSON.TOKEN_COLON) {
                    return null;
                }

                // value
                bool success = true;
                object value = ParseValue(json, ref index, ref success);
                if (!success) {
                    return null;
                }

                table[name] = value;
            }
        }

        return table;
    }


    protected static ArrayList ParseArray(char[] json, ref int index) {
        ArrayList array = new ArrayList();

        // [
        NextToken(json, ref index);

        bool done = false;
        while (!done) {
            int token = LookAhead(json, index);
            if (token == MiniJSON.TOKEN_NONE) {
                return null;
            } else if (token == MiniJSON.TOKEN_COMMA) {
                NextToken(json, ref index);
            } else if (token == MiniJSON.TOKEN_SQUARED_CLOSE) {
                NextToken(json, ref index);
                break;
            } else {
                bool success = true;
                object value = ParseValue(json, ref index, ref success);
                if (!success) {
                    return null;
                }

                array.Add(value);
            }
        }

        return array;
    }


    protected static object ParseValue(char[] json, ref int index, ref bool success) {
        switch (LookAhead(json, index)) {
            case MiniJSON.TOKEN_STRING:
                return ParseString(json, ref index);
            case MiniJSON.TOKEN_NUMBER:
                return ParseNumber(json, ref index);
            case MiniJSON.TOKEN_CURLY_OPEN:
                return ParseObject(json, ref index);
            case MiniJSON.TOKEN_SQUARED_OPEN:
                return ParseArray(json, ref index);
            case MiniJSON.TOKEN_TRUE:
                NextToken(json, ref index);
                return bool.Parse("TRUE");
            case MiniJSON.TOKEN_FALSE:
                NextToken(json, ref index);
                return bool.Parse("FALSE");
            case MiniJSON.TOKEN_NULL:
                NextToken(json, ref index);
                return null;
            case MiniJSON.TOKEN_NONE:
                break;
        }

        success = false;
        return null;
    }


    protected static string ParseString(char[] json, ref int index) {
        string s = "";
        char c;

        EatWhitespace(json, ref index);

        // "
        c = json[index++];

        bool complete = false;
        while (!complete) {
            if (index == json.Length) {
                break;
            }

            c = json[index++];
            if (c == '"') {
                complete = true;
                break;
            } else if (c == '\\') {
                if (index == json.Length) {
                    break;
                }

                c = json[index++];
                if (c == '"') {
                    s += '"';
                } else if (c == '\\') {
                    s += '\\';
                } else if (c == '/') {
                    s += '/';
                } else if (c == 'b') {
                    s += '\b';
                } else if (c == 'f') {
                    s += '\f';
                } else if (c == 'n') {
                    s += '\n';
                } else if (c == 'r') {
                    s += '\r';
                } else if (c == 't') {
                    s += '\t';
                } else if (c == 'u') {
                    int remainingLength = json.Length - index;
                    if (remainingLength >= 4) {
                        char[] unicodeCharArray = new char[4];
                        Array.Copy(json, index, unicodeCharArray, 0, 4);

                        // Drop in the HTML markup for the unicode character
                        //s += "&#x" + new string( unicodeCharArray ) + ";";
                        uint codePoint = uint.Parse(new string(unicodeCharArray), System.Globalization.NumberStyles.HexNumber);
                        // convert the integer codepoint to a unicode char and add to string
                        s += char.ConvertFromUtf32((int)codePoint);

                        // skip 4 chars
                        index += 4;
                    } else {
                        break;
                    }

                }
            } else {
                s += c;
            }

        }

        return !complete ? null : s;
    }


    protected static object ParseNumber(char[] json, ref int index) {
        EatWhitespace(json, ref index);

        int lastIndex = GetLastIndexOfNumber(json, index);
        int charLength = (lastIndex - index) + 1;
        char[] numberCharArray = new char[charLength];

        Array.Copy(json, index, numberCharArray, 0, charLength);
        index = lastIndex + 1;
        double val = double.Parse(new string(numberCharArray), CultureInfo.InvariantCulture);

        if ((val % 1) == 0) {
            if (val >= int.MinValue && val <= int.MaxValue) {
                return (int)val;
            }
        }

        return val;
    }


    protected static int GetLastIndexOfNumber(char[] json, int index) {
        int lastIndex;
        for (lastIndex = index; lastIndex < json.Length; lastIndex++) {
            if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1) {
                break;
            }
        }
        return lastIndex - 1;
    }


    protected static void EatWhitespace(char[] json, ref int index) {
        for (; index < json.Length; index++) {
            if (" \t\n\r".IndexOf(json[index]) == -1) {
                break;
            }
        }
    }


    protected static int LookAhead(char[] json, int index) {
        int saveIndex = index;
        return NextToken(json, ref saveIndex);
    }


    protected static int NextToken(char[] json, ref int index) {
        EatWhitespace(json, ref index);

        if (index == json.Length) {
            return MiniJSON.TOKEN_NONE;
        }

        char c = json[index];
        index++;
        switch (c) {
            case '{':
                return MiniJSON.TOKEN_CURLY_OPEN;
            case '}':
                return MiniJSON.TOKEN_CURLY_CLOSE;
            case '[':
                return MiniJSON.TOKEN_SQUARED_OPEN;
            case ']':
                return MiniJSON.TOKEN_SQUARED_CLOSE;
            case ',':
                return MiniJSON.TOKEN_COMMA;
            case '"':
                return MiniJSON.TOKEN_STRING;
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
            case '-':
                return MiniJSON.TOKEN_NUMBER;
            case ':':
                return MiniJSON.TOKEN_COLON;
        }
        index--;

        int remainingLength = json.Length - index;

        // false
        if (remainingLength >= 5) {
            if (json[index] == 'f' &&
                json[index + 1] == 'a' &&
                json[index + 2] == 'l' &&
                json[index + 3] == 's' &&
                json[index + 4] == 'e') {
                index += 5;
                return MiniJSON.TOKEN_FALSE;
            }
        }

        // true
        if (remainingLength >= 4) {
            if (json[index] == 't' &&
                json[index + 1] == 'r' &&
                json[index + 2] == 'u' &&
                json[index + 3] == 'e') {
                index += 4;
                return MiniJSON.TOKEN_TRUE;
            }
        }

        // null
        if (remainingLength >= 4) {
            if (json[index] == 'n' &&
                json[index + 1] == 'u' &&
                json[index + 2] == 'l' &&
                json[index + 3] == 'l') {
                index += 4;
                return MiniJSON.TOKEN_NULL;
            }
        }

        return MiniJSON.TOKEN_NONE;
    }

    #endregion


    #region Serialization

    protected static bool SerializeObjectOrArray(object objectOrArray, StringBuilder builder) {
        return objectOrArray is Hashtable
            ? SerializeObject((Hashtable)objectOrArray, builder)
            : objectOrArray is ArrayList ? SerializeArray((ArrayList)objectOrArray, builder) : false;
    }


    protected static bool SerializeObject(Hashtable anObject, StringBuilder builder) {
        builder.Append("{");

        IDictionaryEnumerator e = anObject.GetEnumerator();
        bool first = true;
        while (e.MoveNext()) {
            string key = e.Key.ToString();
            object value = e.Value;

            if (!first) {
                builder.Append(",");
            }

            SerializeString(key, builder);
            builder.Append(":");
            if (!SerializeValue(value, builder)) {
                return false;
            }

            first = false;
        }

        builder.Append("}");
        return true;
    }

    protected static bool SerializeObjectPretty(Hashtable anObject, StringBuilder builder, int depth) {
        builder.Append("{");

        IDictionaryEnumerator e = anObject.GetEnumerator();
        bool first = true;
        while (e.MoveNext()) {
            string key = e.Key.ToString();
            object value = e.Value;

            if (first) {
                builder.AppendLine();
            } else {
                builder.AppendLine(",");
            }

            AppendTabs(builder, depth + 1);
            SerializeString(key, builder);
            builder.Append(" : ");
            if (!SerializeValuePretty(value, builder, depth + 1)) {
                return false;
            }

            first = false;
        }

        builder.AppendLine();
        AppendTabs(builder, depth);
        builder.Append("}");
        return true;
    }


    protected static bool SerializeDictionary(Dictionary<string, string> dict, StringBuilder builder) {
        builder.Append("{");

        bool first = true;
        foreach (KeyValuePair<string, string> kv in dict) {
            if (!first) {
                builder.Append(",");
            }

            SerializeString(kv.Key, builder);
            builder.Append(":");
            SerializeString(kv.Value, builder);

            first = false;
        }

        builder.Append("}");
        return true;
    }

    protected static bool SerializeDictionaryPretty(Dictionary<string, string> dict, StringBuilder builder, int depth) {
        AppendTabs(builder, depth);
        builder.Append("{");

        bool first = true;
        foreach (KeyValuePair<string, string> kv in dict) {
            if (!first) {
                builder.Append(",");
            }

            AppendTabs(builder, depth + 1);
            SerializeString(kv.Key, builder);
            builder.Append(" : ");
            SerializeString(kv.Value, builder);

            first = false;
        }

        builder.AppendLine();
        AppendTabs(builder, depth);
        builder.Append("}");
        return true;
    }

    protected static bool SerializeDictionary(Dictionary<string, float> dict, StringBuilder builder) {
        builder.Append("{");

        bool first = true;
        foreach (KeyValuePair<string, float> kv in dict) {
            if (!first) {
                builder.Append(",");
            }

            SerializeString(kv.Key, builder);
            builder.Append(":");
            SerializeNumber(kv.Value, builder);

            first = false;
        }

        builder.Append("}");
        return true;
    }

    protected static bool SerializeDictionaryPretty(Dictionary<string, float> dict, StringBuilder builder, int depth) {
        AppendTabs(builder, depth);
        builder.Append("{");

        bool first = true;
        foreach (KeyValuePair<string, float> kv in dict) {
            if (!first) {
                builder.Append(",");
            }
            AppendTabs(builder, depth + 1);
            SerializeString(kv.Key, builder);
            builder.Append(" : ");
            SerializeNumber(kv.Value, builder);
            first = false;
        }
        builder.AppendLine();
        AppendTabs(builder, depth);
        builder.Append("}");
        return true;
    }

    protected static bool SerializeDictionary(Dictionary<string, object> dict, StringBuilder builder) {
        builder.Append("{");
        bool first = true;
        foreach (KeyValuePair<string, object> kv in dict) {
            if (!first) {
                builder.Append(",");
            }
            SerializeString(kv.Key, builder);
            builder.Append(":");
            SerializeValue(kv.Value, builder);
            first = false;
        }
        builder.Append("}");
        return true;
    }

    protected static bool SerializeDictionaryPretty(Dictionary<string, object> dict, StringBuilder builder, int depth) {
        builder.AppendLine();
        AppendTabs(builder, depth);
        builder.Append("{");
        bool first = true;
        foreach (KeyValuePair<string, object> kv in dict) {
            if (first) {
                builder.AppendLine();
            } else {
                builder.AppendLine(",");
            }
            AppendTabs(builder, depth + 1);
            SerializeString(kv.Key, builder);
            builder.Append(" : ");
            SerializeValuePretty(kv.Value, builder, depth + 1);
            first = false;
        }
        builder.AppendLine();
        AppendTabs(builder, depth);
        builder.Append("}");
        return true;
    }

    protected static bool SerializeArray(ArrayList anArray, StringBuilder builder) {
        builder.Append("[");

        bool first = true;
        for (int i = 0; i < anArray.Count; i++) {
            object value = anArray[i];

            if (!first) {
                builder.AppendLine(",");
            }

            if (!SerializeValue(value, builder)) {
                return false;
            }

            first = false;
        }

        builder.Append("]");
        return true;
    }

    protected static bool SerializeArrayPretty(ArrayList anArray, StringBuilder builder, int depth) {
        builder.AppendLine();
        AppendTabs(builder, depth);
        builder.Append("[");

        bool first = true;
        for (int i = 0; i < anArray.Count; i++) {
            object value = anArray[i];

            if (first) {
                builder.AppendLine();
            } else {
                builder.AppendLine(",");
            }

            AppendTabs(builder, depth + 1);
            if (!SerializeValuePretty(value, builder, depth + 1)) {
                return false;
            }

            first = false;
        }

        builder.AppendLine();
        AppendTabs(builder, depth);
        builder.Append("]");
        return true;
    }

    protected static bool SerializeValue(object value, StringBuilder builder) {
        /*
				if( value != null )
				{
					Type t = value.GetType();
					UnityEngine.Debug.Log("type: " + t.ToString() + " isArray: " + t.IsArray);
				}
				else
					UnityEngine.Debug.Log("null");
		*/
        if (value == null) {
            builder.Append("null");
        } else if (value.GetType().IsArray) {
            SerializeArray(new ArrayList((ICollection)value), builder);
        } else if (value is string) {
            SerializeString((string)value, builder);
        } else if (value is char) {
            SerializeString(Convert.ToString((char)value), builder);
        } else if (value is Hashtable) {
            SerializeObject((Hashtable)value, builder);
        } else if (value is Dictionary<string, string>) {
            SerializeDictionary((Dictionary<string, string>)value, builder);
        } else if (value is Dictionary<string, float>) {
            SerializeDictionary((Dictionary<string, float>)value, builder);
        } else if (value is Dictionary<string, object>) {
            SerializeDictionary((Dictionary<string, object>)value, builder);
        } else if (value is ArrayList) {
            SerializeArray((ArrayList)value, builder);
        } else if ((value is bool) && ((bool)value == true)) {
            builder.Append("true");
        } else if ((value is bool) && ((bool)value == false)) {
            builder.Append("false");
        } else if (IsPrimitive(value.GetType()) || value is Enum) {
            SerializeNumber(Convert.ToDouble(value), builder);
        } else if (value is JsonInterface) {
            Hashtable ht = new Hashtable();
            ((JsonInterface)value).ToJsonObject(ht);
            SerializeObject(ht, builder);
        } else if (value is IJsonInterface) {
            Hashtable ht = new Hashtable();
            ((IJsonInterface)value).ToJsonObject(ht);
            SerializeObject(ht, builder);
        } else if (value is IJsonPrimitiveInterface) {
            object obj = ((IJsonPrimitiveInterface)value).ToJsonObject();
            SerializeValue(obj, builder);
        } else if (value is ICollection) {
            SerializeArray(new ArrayList((ICollection)value), builder);
        } else if (value is IJsonRawInterface) {
            builder.Append(((IJsonRawInterface)value).ToJsonValue());
        } else {
            return false;
        }

        return true;
    }

    protected static bool SerializeValuePretty(object value, StringBuilder builder, int depth) {
        /*
                if( value != null )
                {
                    Type t = value.GetType();
                    UnityEngine.Debug.Log("type: " + t.ToString() + " isArray: " + t.IsArray);
                }
                else
                    UnityEngine.Debug.Log("null");
        */
        if (value == null) {
            builder.Append("null");
        } else if (value.GetType().IsArray) {
            SerializeArrayPretty(new ArrayList((ICollection)value), builder, depth);
        } else if (value is Enum) {
            SerializeString(value.ToString(), builder);
        } else if (value is string) {
            SerializeString((string)value, builder);
        } else if (value is char) {
            SerializeString(Convert.ToString((char)value), builder);
        } else if (value is Hashtable) {
            SerializeObjectPretty((Hashtable)value, builder, depth);
        } else if (value is Dictionary<string, string>) {
            SerializeDictionaryPretty((Dictionary<string, string>)value, builder, depth);
        } else if (value is Dictionary<string, float>) {
            SerializeDictionaryPretty((Dictionary<string, float>)value, builder, depth);
        } else if (value is Dictionary<string, object>) {
            SerializeDictionaryPretty((Dictionary<string, object>)value, builder, depth);
        } else if (value is ArrayList) {
            SerializeArrayPretty((ArrayList)value, builder, depth);
        } else if ((value is bool) && ((bool)value == true)) {
            builder.Append("true");
        } else if ((value is bool) && ((bool)value == false)) {
            builder.Append("false");
        } else if (IsPrimitive(value.GetType()) || value is Enum) {
            SerializeNumber(Convert.ToDouble(value), builder);
        } else if (value is JsonInterface) {
            Hashtable ht = new Hashtable();
            ((JsonInterface)value).ToJsonObject(ht);
            SerializeObjectPretty(ht, builder, depth);
        } else if (value is IJsonInterface) {
            Hashtable ht = new Hashtable();
            ((IJsonInterface)value).ToJsonObject(ht);
            SerializeObjectPretty(ht, builder, depth);
        } else if (value is IJsonPrimitiveInterface) {
            object obj = ((IJsonPrimitiveInterface)value).ToJsonObject();
            SerializeValuePretty(obj, builder, depth);
        } else if (value is ICollection) {
            SerializeArrayPretty(new ArrayList((ICollection)value), builder, depth);
        } else if (value is IJsonRawInterface) {
            builder.Append(((IJsonRawInterface)value).ToJsonValue());
        } else {
            return false;
        }

        return true;
    }


    protected static void SerializeString(string aString, StringBuilder builder) {
        builder.Append("\"");

        char[] charArray = aString.ToCharArray();
        for (int i = 0; i < charArray.Length; i++) {
            char c = charArray[i];
            if (c == '"') {
                builder.Append("\\\"");
            } else if (c == '\\') {
                builder.Append("\\\\");
            } else if (c == '\b') {
                builder.Append("\\b");
            } else if (c == '\f') {
                builder.Append("\\f");
            } else if (c == '\n') {
                builder.Append("\\n");
            } else if (c == '\r') {
                builder.Append("\\r");
            } else if (c == '\t') {
                builder.Append("\\t");
            } else {
                int codepoint = Convert.ToInt32(c);
                if ((codepoint >= 32) && (codepoint <= 126)) {
                    builder.Append(c);
                } else {
                    builder.Append("\\u" + Convert.ToString(codepoint, 16).PadLeft(4, '0'));
                }
            }
        }

        builder.Append("\"");
    }

    protected static void AppendTabs(StringBuilder builder, int depth) {
        for (int i = 0; i < depth; i++) {
            builder.Append("\t");
        }
    }

    protected static void SerializeNumber(double number, StringBuilder builder) {
        builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
    }

    protected static bool IsPrimitive(Type t) {
#if UNITY_METRO
		return t == typeof(Boolean) || t == typeof(Byte) || t == typeof(SByte) || t == typeof(Int16) || t == typeof(UInt16) || t == typeof(Int32) || t == typeof(UInt32) || t == typeof(Int64) || t == typeof(UInt64) || t == typeof(IntPtr) || t == typeof(UIntPtr) || t == typeof(Char) || t == typeof(Double) || t == typeof(Single);
#else
        return t.IsPrimitive;
#endif
    }

    #endregion

}



#region Extension methods

public static class MiniJsonExtensions {
    public static string ToJson(this Hashtable obj, bool shouldPrettify = false) {
        return MiniJSON.JsonEncode(obj, shouldPrettify);
    }

    public static string ToJson(this ArrayList obj, bool shouldPrettify = false) {
        return MiniJSON.JsonEncode(obj, shouldPrettify);
    }

    public static string ToJson(this Dictionary<string, string> obj, bool shouldPrettify = false) {
        return MiniJSON.JsonEncode(obj, shouldPrettify);
    }

    public static string ToJson(this Dictionary<string, object> obj, bool shouldPrettify = false) {
        return MiniJSON.JsonEncode(obj, shouldPrettify);
    }

    public static ArrayList ArrayListFromJson(this string json) {
        return MiniJSON.JsonDecode(json) as ArrayList;
    }


    public static Hashtable HashtableFromJson(this string json) {
        return MiniJSON.JsonDecode(json) as Hashtable;
    }

    public static T toObject<T>(this string json) where T : JsonInterface, new() {
        Hashtable ht = MiniJSON.JsonDecode(json) as Hashtable;
        T obj = new T();
        if (ht != null) {
            obj.FromJson(ht);
        }
        return obj;
    }

    public static T toObject<T>(this Hashtable ht) where T : JsonInterface, new() {
        if (ht == null) {
            return null;
        }

        T obj = new T();
        if (ht != null) {
            obj.FromJson(ht);
        }
        return obj;
    }

    public static T[] ToArray<T>(this ArrayList array) where T : JsonInterface, new() {
        if (array == null) {
            return null;
        }

        T[] res = new T[array.Count];

        for (int i = 0; i < array.Count; i++) {
            res[i] = new T();
            if (array[i] != null) {
                res[i].FromJson((Hashtable)array[i]);
            }
        }

        return res;
    }

    public static List<T> toList<T>(this ArrayList array) where T : JsonInterface, new() {
        if (array == null) {
            return null;
        }

        List<T> res = new List<T>(array.Count);

        for (int i = 0; i < array.Count; i++) {
            T obj = new T();
            if (array[i] != null) {
                obj.FromJson((Hashtable)array[i]);
            }
            res.Add(obj);
        }

        return res;
    }

    public static List<T> asList<T>(this ArrayList array) {
        if (array == null) {
            return null;
        }

        List<T> res = new List<T>(array.Count);

        for (int i = 0; i < array.Count; i++) {
            res.Add((T)array[i]);
        }

        return res;
    }

    public static T[] asArray<T>(this ArrayList array) {
        if (array == null) {
            return null;
        }

        T[] res = new T[array.Count];

        for (int i = 0; i < array.Count; i++) {
            res[i] = (T)array[i];
        }

        return res;
    }

    public static List<T> listFromJson<T>(this string json) where T : JsonInterface, new() {
        ArrayList al = MiniJSON.JsonDecode(json) as ArrayList;
        return al.toList<T>();
    }

    public static string GetString(this Hashtable ht, string key) {
        return ht.Contains(key) && ht[key] != null ? (string)ht[key] : null;
    }

    public static string GetStringSafe(this Hashtable ht, string key, string defaultValue = null) {
        return ht.Contains(key) && ht[key] is string ? ht[key] as string : defaultValue;
    }

    public static UnityEngine.Color GetColor(this Hashtable ht, string key, UnityEngine.Color defaultValue) {
        if (ht.Contains(key) && ht[key] is string) {
            UnityEngine.ColorUtility.TryParseHtmlString(ht[key] as string, out defaultValue);
        }
        return defaultValue;
    }

    public static void Write(this Hashtable ht, string key, UnityEngine.Color color) {
        ht[key] = ColorUtility.ToHtmlStringRGBA(color);
    }

    public static void Write(this Hashtable ht, string key, Vector3 value) {
        ht[key] = new SerializedVector3(value);
    }

    public static void Write(this Hashtable ht, string key, Vector2 value) {
        ht[key] = new SerializedVector2(value);
    }

    public static float GetFloat(this Hashtable ht, string key, float defaultValue = 0) {
        return ht.Contains(key) && ht[key] != null ? Convert.ToSingle(ht[key]) : defaultValue;
    }

    public static double GetDouble(this Hashtable ht, string key) {
        return ht.Contains(key) && ht[key] != null ? Convert.ToDouble(ht[key]) : 0;
    }

    public static int GetInt32(this Hashtable ht, string key) {
        return ht.Contains(key) && ht[key] != null ? Convert.ToInt32(ht[key]) : 0;
    }

    public static int GetInt32(this Hashtable ht, string key, int defaultValue) {
        return ht.Contains(key) && ht[key] != null ? Convert.ToInt32(ht[key]) : defaultValue;
    }

    public static uint GetUInt32(this Hashtable ht, string key, uint defaultValue = 0) {
        return ht.Contains(key) && ht[key] != null ? Convert.ToUInt32(ht[key]) : defaultValue;
    }

    public static long GetInt(this Hashtable ht, string key) {
        return ht.Contains(key) && ht[key] != null ? Convert.ToInt32(ht[key]) : 0;
    }

    public static Vector2 GetVector2(this Hashtable ht, string key) {
        return ht.GetAs<SerializedVector2>(key, SerializedVector2.Zero).Value;
    }

    public static Vector2 GetVector2(this Hashtable ht, string key, Vector2 defaultValue) {
        return ht.GetAs<SerializedVector2>(key, new SerializedVector2(defaultValue)).Value;
    }

    public static Vector3 GetVector3(this Hashtable ht, string key) {
        return ht.GetAs<SerializedVector3>(key, SerializedVector3.Zero).Value;
    }

    public static Vector3 GetVector3(this Hashtable ht, string key, Vector3 defaultValue) {
        return ht.GetAs<SerializedVector3>(key, new SerializedVector3(defaultValue)).Value;
    }

    public static long GetLong(this Hashtable ht, string key) {
        return ht.Contains(key) && ht[key] != null ? Convert.ToInt64(ht[key]) : 0;
    }

    public static bool GetBool(this Hashtable ht, string key) {
        return ht.Contains(key) && ht[key] != null ? Convert.ToBoolean(ht[key]) : false;
    }

    public static bool GetBool(this Hashtable ht, string key, bool defaultValue) {
        return ht.Contains(key) && ht[key] != null ? Convert.ToBoolean(ht[key]) : defaultValue;
    }

    public static Dictionary<K, V> HashtableToDictionary<K, V>(this Hashtable table) {
        Dictionary<K, V> dict = new Dictionary<K, V>();
        foreach (DictionaryEntry kvp in table) {
            dict.Add((K)kvp.Key, (V)kvp.Value);
        }
        return dict;
    }

    public static T GetEnum<T>(this Hashtable ht, string key, T defaultValue) {
        return ht.Contains(key) && ht[key] != null ? ParseEnum<T>((string)ht[key], defaultValue) : defaultValue;
    }

    private static T ParseEnum<T>(string value, T defaultValue) {
        Type type = typeof(T);
        try {
            return (T)Enum.Parse(type, value, true);
        } catch (ArgumentException) {
            Debug.LogErrorFormat("Invalid enum {0} value: {1}", type.Name, value);
        }
        return defaultValue;
    }

    public static T GetEnumFromIndex<T>(this Hashtable ht, string key, T defaultValue) {
        if (ht.Contains(key) && ht[key] != null) {
            string name = Enum.GetName(typeof(T), ht.GetInt32(key));
            return ParseEnum<T>(name, defaultValue);
        }
        return defaultValue;
    }

    public static string GetString(this Hashtable ht, string key, string defaultValue) {
        return ht.Contains(key) && ht[key] != null ? (string)ht[key] : defaultValue;
    }

    public static bool IsString(this Hashtable ht, string key) {
        return ht.Contains(key) && ht[key] != null && ht[key] is string;
    }

    public static Hashtable GetHashtable(this Hashtable ht, string key, Hashtable defaultValue = null) {
        return ht.Contains(key) && ht[key] != null ? (Hashtable)ht[key]: defaultValue;
    }

    public static T[] GetEnumArray<T>(this Hashtable ht, string key, T defaultValue, T[] defaultArray = null) {
        if (ht.Contains(key) && ht[key] != null) {
            ArrayList array = ht[key] as ArrayList;
            if (array == null) {
                return defaultArray;
            }
            T[] res = new T[array.Count];
            for (int i = 0; i < array.Count; i++) {
                if (array[i] != null) {
                    string enumName = array[i] as string;
                    if (enumName != null) {
                        res[i] = ParseEnum<T>(enumName, defaultValue);
                    } else {
                        res[i] = (T)array[i];
                    }
                }
            }
            return res;
        }
        return defaultArray;
    }

    public static T[] GetArray<T>(this Hashtable ht, string key, T[] defaultValue = null) where T : IJsonInterface, new() {
        if (ht.Contains(key) && ht[key] != null) {
            ArrayList array = ht[key] as ArrayList;

            if (array == null) {
                return defaultValue;
            }

            T[] res = new T[array.Count];

            for (int i = 0; i < array.Count; i++) {
                res[i] = new T();
                if (array[i] != null) {
                    res[i].FromJson((Hashtable)array[i]);
                }
            }

            return res;
        }

        return defaultValue;
    }

    public static int[] GetArray(this Hashtable ht, string key, int[] defaultValue = null) {
        if (ht.Contains(key) && ht[key] != null) {
            ArrayList array = ht[key] as ArrayList;
            if (array == null) {
                return defaultValue;
            }
            int[] res = new int[array.Count];
            for (int i = 0; i < array.Count; i++) {
                res[i] = (int)array[i];
            }
            return res;
        }
        return defaultValue;
    }

    public static string[] GetArray(this Hashtable ht, string key, string[] defaultValue = null) {
        if (ht.Contains(key) && ht[key] != null) {
            ArrayList array = ht[key] as ArrayList;
            if (array == null) {
                return defaultValue;
            }
            string[] res = new string[array.Count];
            for (int i = 0; i < array.Count; i++) {
                res[i] = (string)array[i];
            }
            return res;
        }
        return defaultValue;
    }

    public static List<T> GetList<T>(this Hashtable ht, string key, List<T> defaultValue) where T : IJsonInterface, new() {
        if (ht.Contains(key) && ht[key] != null) {
            if (ht[key] is ArrayList array) {
                List<T> result = new List<T>(array.Count);
                for (int i = 0; i < array.Count; i++) {
                    T item = new T();
                    if (array[i] != null) {
                        item.FromJson((Hashtable)array[i]);
                    }
                    result.Add(item);
                }
                return result;
            }
        }
        return defaultValue;
    }
    
    public static List<int> GetList(this Hashtable ht, string key, List<int> defaultValue) {
        if (ht.Contains(key) && ht[key] != null) {
            if (ht[key] is ArrayList array) {
                List<int> result = new List<int>(array.Count);
                for (int i = 0; i < array.Count; i++) {
                    if (array[i] != null) {
                        result.Add(Convert.ToInt32(array[i]));
                    }
                }
                return result;
            }
        }
        return defaultValue;
    }

    public static T GetAs<T>(this Hashtable ht, string key, T defaultValue) where T : IJsonInterface, new() {
        if (ht.ContainsKey(key) && ht[key] != null) {
            T value = new T();
            value.FromJson(ht[key] as Hashtable);
            return value;
        }
        return defaultValue;
    }

    public static List<T> ToList<T>(this ArrayList array) where T : IJsonInterface, new() {
        if (array == null) { return null; }

        List<T> res = new List<T>(array.Count);

        for (int i = 0; i < array.Count; i++) {
            T obj = new T();
            if (array[i] != null) {
                obj.FromJson((Hashtable)array[i]);
            }
            res.Add(obj);
        }

        return res;
    }

    public static T[] AsArray<T>(this Hashtable ht, string key, T[] defaultValue = null) {
        if (ht.Contains(key) && ht[key] != null) {
            ArrayList array = (ArrayList)ht[key];

            if (array == null) {
                return defaultValue;
            }

            T[] res = new T[array.Count];

            for (int i = 0; i < array.Count; i++) {
                res[i] = (T)array[i];
            }

            return res;
        }

        return defaultValue;
    }

    public static T ToObject<T>(this Hashtable ht) where T : IJsonInterface, new() {
        if (ht == null) {
            return default(T);
        }

        T obj = new T();
        if (ht != null) {
            obj.FromJson(ht);
        }

        return obj;
    }

    public static T ToObject<T>(this Hashtable ht, string key, T defaultValue) where T : IJsonInterface, new() {
        if (ht == null || !ht.ContainsKey(key)) {
            return defaultValue;
        }

        Hashtable objHt = ht[key] as Hashtable;

        if (objHt == null) {
            return defaultValue;
        }

        T obj = new T();
        if (objHt != null) {
            obj.FromJson(objHt);
        }

        return obj;
    }

    public static T ToObject<T>(this string json) where T : IJsonInterface, new() {
        Hashtable ht = MiniJSON.JsonDecode(json) as Hashtable;
        T obj = new T();
        if (ht != null) {
            obj.FromJson(ht);
        }
        return obj;
    }
}

#endregion
