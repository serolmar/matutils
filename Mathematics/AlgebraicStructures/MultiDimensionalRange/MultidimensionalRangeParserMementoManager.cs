﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics
{
    class MultidimensionalRangeParserMementoManager
    {
        private IMemento memento;

        private int level;

        private int currentReadedElements;

        public IMemento Memento
        {
            get
            {
                return this.memento;
            }
            set
            {
                this.memento = value;
            }
        }

        public int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        public int CurrentReadedElements
        {
            get
            {
                return this.currentReadedElements;
            }
            set
            {
                this.currentReadedElements = value;
            }
        }
    }
}
