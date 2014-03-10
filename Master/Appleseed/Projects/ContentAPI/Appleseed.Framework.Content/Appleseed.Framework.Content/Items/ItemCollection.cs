using System;
//using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace Content.API
{
    [Serializable]
    public class ItemCollection : ICollection<Item>, IEnumerable<Item>
    {
        private Dictionary<long, Item> _collection;
        private List<long> _keys;
        private int _index = 0;
        private bool _readOnly = false;
        private bool _synchronized = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:ItemCollection"/> class.
        /// </summary>
        public ItemCollection()
        {
            _collection = new Dictionary<long, Item>();
            _keys = new List<long>();
        }

        #region ICollection<Item> Members
        /// <summary>
        /// Gets the items of the collection as a list.
        /// </summary>
        protected virtual Dictionary<long, Item> Items
        {
            get { return _collection; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public virtual void Add(Item item)
        {
            long id = item.ItemID;

            if (!_keys.Contains(id))
            {
                _collection.Add(id, item);
                _keys.Add(id);
                return;
            }

            if (_keys.Contains(id) && _collection[id].Equals(item))
            {
                throw new Exception("Item already in collection");
            }
            else
            {
                _collection.Remove(id);
                _collection.Add(id, item);
                return;
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
        public void Clear()
        {
            _collection.Clear();
            _keys.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether this instance has items.
        /// </summary>
        /// <value><c>true</c> if this instance has items; otherwise, <c>false</c>.</value>
        public bool HasItems
        {
            get
            {
                if (_keys.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// <remarks>
        /// This will cause a comparison of objects, and will be significantly slower than doing ContainsKey,
        /// however if you need more accuracty with comparing all properties in the objects, this will do it.
        /// </remarks>
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(Item item)
        {
            if (_collection.ContainsValue(item))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determines whether the collection contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(long key)
        {
            if (_keys.Contains(key))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(Item[] array, int arrayIndex)
        {
            int j = 0;
            for (int i = arrayIndex; i <= _keys.Count; i++)
            {
                array[j] = _collection[_keys[i]];
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
        [Browsable(false)]
        public virtual int Count
        {
            get { return _keys.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        [Browsable(false)]
        public bool IsReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is synchronized.
        /// 
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is synchronized; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public bool IsSynchronized
        {
            get { return _synchronized; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public bool Remove(Item item)
        {
            long id = item.ItemID;
            
            if (_keys.Contains(id))
            {
                _collection.Remove(id);
                _keys.Remove(id);
            }
            
            return true;
        }

        /// <summary>
        /// Gets an object that can be used to synchronize the collection.
        /// </summary>
        /// <value>The sync root.</value>
        [Browsable(false)]
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region IEnumerable<Item> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Item> GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        #endregion

        #region IEnumerable<Item> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
        {
            return _collection.Values.GetEnumerator();
        }

        #endregion
    }

    public class ItemCollection<T> : ItemCollection
    {

    }
}
