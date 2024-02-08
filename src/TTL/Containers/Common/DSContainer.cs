﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ProcessDashboard.src.TTL.Containers.Common
{
    public class DSContainer<T>
    {
        private T _ds11;
        private T _ds12;
        private T _ds21;
        private T _ds22;

        public T DS11
        {
            get => _ds11;
            set
            {
                _ds11 = value;
                UpdateElementsList();
            }
        }

        public T DS12
        {
            get => _ds12;
            set
            {
                _ds12 = value;
                UpdateElementsList();
            }
        }

        public T DS21
        {
            get => _ds21;
            set
            {
                _ds21 = value;
                UpdateElementsList();
            }
        }

        public T DS22
        {
            get => _ds22;
            set
            {
                _ds22 = value;
                UpdateElementsList();
            }
        }

        /// <summary>
        /// List of Type <b>T</b> that consists of DS props so you can use loops to operate with props
        /// </summary>
        public List<T> Elements { get; set; }

        public int Count { get; } = 4;

        public DSContainer()
        {
            _ds11 = default;
            _ds12 = default;
            _ds21 = default;
            _ds22 = default;
            UpdateElementsList();
        }

        internal protected void UpdateElementsList()
        {
            Elements = new List<T> { DS11, DS12, DS21, DS22 };
        }

        /// <summary>
        /// Performs the action on each of the props (DS11, DS12, ...)
        /// </summary>
        /// <param name="action">Void function that takes <b>T</b> as argument and performs operations on prop</param>
        public void Apply(Action<T> action)
        {
            if (action == null) return;

            action(DS11);
            action(DS12);
            action(DS21);
            action(DS22);
        }

        /// <summary>
        /// Set the value of prop and element in Elements based on the index.
        /// </summary>
        /// <param name="idx">Index of element in Elements (0-3)</param>
        /// <param name="value">Desired value of type T</param>
        public void Set(int idx,  T value)
        {
            if (idx == 0) DS11 = value;
            if (idx == 1) DS12 = value;
            if (idx == 2) DS21 = value;
            if (idx == 3) DS22 = value;
        }

        /// <summary>
        /// Set the value of prop and element in Elements based on TrackID and PressID
        /// </summary>
        /// <param name="track">TrackID from the unit</param>
        /// <param name="press">PressID from the unit</param>
        /// <param name="value">Desired value of type T</param>
        public void Set(int track, int press, T value)
        {
            if (track == 1 && press == 1) DS11 = value;
            if (track == 1 && press == 2) DS12 = value;
            if (track == 2 && press == 1) DS21 = value;
            if (track == 2 && press == 2) DS22 = value;
        }
    }
}
