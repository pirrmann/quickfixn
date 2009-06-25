﻿using System;

namespace QuickFIX.NET.Fields
{
    /// <summary>
    /// Base class for all field types
    /// </summary>
    /// <typeparam name="T">Internal storage type</typeparam>
    public abstract class FieldBase<T> : IField
    {
        public FieldBase(int tag, T obj)
        {
            _tag = tag;
            _obj = obj;
            _changed = true;
        }

        #region Properties
        public T Obj
        {
            get { return _obj; }
            set
            {
                _obj = value;
                _changed = true;
            }
        }

        public override int Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;
                _changed = true;
            }
        }
        #endregion

        /// <summary>
        /// returns full fix string: tag=val
        /// </summary>
        public override string toStringField()
        {
            if( _changed.Equals( true ))
                makeStringFields();
            return _stringField;
        }

        /// <summary>
        /// returns formatted string for fix
        /// </summary>
        public override string ToString()
        {
            if( _changed )
              makeStringFields();
            return _stringVal;
        }

        /// <summary>
        /// length of formatted field (including tag=val\001)
        /// </summary>
        public override int getLength()
        {
            return _stringField.Length + 1; // +1 for SOH
        }

        /// <summary>
        /// checksum
        /// </summary>
        public override int getTotal()
        {
            if( _changed )
                makeStringFields();

            int sum = 0;
            foreach (char c in _stringField)
            {
                sum += (int)c;
            }
            return( sum + 1 ); // +1 for SOH
        }

        protected abstract string makeString();

        /// <summary>
        /// returns tag=val
        /// </summary>
        private void makeStringFields()
        {
            _stringVal = makeString();
            _stringField = Tag + "=" + _stringVal;
            _changed = false;
        }

        #region Private members
        private string _stringField;
        private bool _changed;
        private T _obj;
        private int _tag;
        private string _stringVal;
        #endregion
    }
}