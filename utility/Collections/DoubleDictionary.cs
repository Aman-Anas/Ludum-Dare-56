namespace Utilities.Collections;

using System.Collections;
using System.Collections.Generic;

// Credit to GodotUtilities! https://github.com/firebelley/GodotUtilities/
public class DoubleDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> _keyToValue = [];
    private readonly Dictionary<TValue, TKey> _valueToKey = [];

    public TValue this[TKey key]
    {
        get => _keyToValue[key];
        set
        {
            if (_keyToValue.TryGetValue(key, out TValue oldVal))
            {
                _valueToKey.Remove(oldVal);
            }
            _keyToValue[key] = value;
            _valueToKey[value] = key;
        }
    }

    public TKey this[TValue val]
    {
        get => _valueToKey[val];
        set
        {
            if (_valueToKey.TryGetValue(val, out TKey oldVal))
            {
                _keyToValue.Remove(oldVal);
            }
            _valueToKey[val] = value;
            _keyToValue[value] = val;
        }
    }

    public ICollection<TKey> Keys => _keyToValue.Keys;

    public ICollection<TValue> Values => _valueToKey.Keys;

    public int Count => _keyToValue.Count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        _keyToValue[key] = value;
        _valueToKey[value] = key;
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _keyToValue[item.Key] = item.Value;
        _valueToKey[item.Value] = item.Key;
    }

    public void Clear()
    {
        _keyToValue.Clear();
        _valueToKey.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) =>
        _keyToValue.ContainsKey(item.Key) && _valueToKey.ContainsKey(item.Value);

    public bool ContainsKey(TKey key) => _keyToValue.ContainsKey(key);

    public bool ContainsKey(TValue value) => _valueToKey.ContainsKey(value);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) { }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyToValue.GetEnumerator();

    public bool Remove(TKey key)
    {
        if (_keyToValue.TryGetValue(key, out TValue val))
        {
            _keyToValue.Remove(key);
            _valueToKey.Remove(val);
            return true;
        }
        return false;
    }

    public bool Remove(TValue value)
    {
        if (_valueToKey.TryGetValue(value, out TKey key))
        {
            _valueToKey.Remove(value);
            _keyToValue.Remove(key);
            return true;
        }
        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (_keyToValue.TryGetValue(key, out value))
        {
            return true;
        }
        value = default;
        return false;
    }

    public bool TryGetValue(TValue value, out TKey key)
    {
        if (_valueToKey.TryGetValue(value, out key))
        {
            return true;
        }
        key = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => _keyToValue.GetEnumerator();
}
