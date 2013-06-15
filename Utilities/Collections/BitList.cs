using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    class BitList : IList<int>
    {
        #region fields
        /// <summary>
        /// Number of bits contained in a byte.
        /// </summary>
        public static readonly int byteNumber = 8;

        /// <summary>
        /// The number of bits in an array variable.
        /// </summary>
        public static readonly int bitNumber = byteNumber * sizeof(ulong);

        /// <summary>
        /// The variable array.
        /// </summary>
        private List<ulong> elements;

        /// <summary>
        /// The number of bits setted.
        /// </summary>
        private int countBits = 0;

        /// <summary>
        /// A mask that contains variables with only one bit setted in each position of variable.
        /// </summary>
        private ulong[] maskPositionBits;

        #endregion

        public BitList()
        {
            this.elements = new List<ulong>();
            InitMask();
        }

        public BitList(int capacity)
        {
            Reserve(capacity);
            InitMask();
        }

        public BitList(int numberOfBits, bool set)
        {
            Reserve(numberOfBits);
            this.countBits = numberOfBits;
            InitMask();
            SetAllBits(set);
        }

        #region IList<int> Members

        public int IndexOf(int item)
        {
            int result = -1;
            int pointer = 0;
            while (result == -1 && pointer < this.elements.Count)
            {
                result = FindFirstBitInVariable(item, this.elements[pointer]);
            }
            return result;
        }

        public void Insert(int index, int item)
        {
            if (index < 0 || index > countBits)
            {
                throw new ArgumentOutOfRangeException("Index must be within the bounds of the List." +
                Environment.NewLine + "Parameter name: index");
            }
            int minPointer = index / bitNumber;
            int rem = index % bitNumber;
            ++countBits;
            if (countBits / bitNumber >= elements.Count)
            {
                elements.Add(0L);
            }
            int pointer = elements.Count - 1;
            while (minPointer < pointer)
            {
                ulong lastBitInPrevious = maskPositionBits[bitNumber - 1] & elements[pointer - 1];
                elements[pointer] = elements[pointer] << 1;
                if (lastBitInPrevious == 0)
                {
                    elements[pointer] = elements[pointer] & ~(1UL);
                }
                else
                {
                    elements[pointer] = elements[pointer] | 1UL;
                }
                --pointer;
            }
            elements[pointer] = RotateVariableBetweenIndices(rem, bitNumber - 1, elements[pointer], 1, false);
            if (item == 0)
            {
                elements[pointer] = elements[pointer] & ~maskPositionBits[rem];
            }
            else
            {
                elements[pointer] = elements[pointer] | maskPositionBits[rem];
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= countBits)
            {
                throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection." +
                Environment.NewLine + "Parameter name: index");
            }
            int pointer = index / bitNumber;
            int rem = index % bitNumber;
            elements[pointer] = RotateVariableBetweenIndices(rem, bitNumber - 1, this.elements[pointer], 1, true);
            for (int i = pointer + 1; i < elements.Count; ++i)
            {
                ulong firstBitInCurrent = this.maskPositionBits[0] & this.elements[pointer];
                if (firstBitInCurrent == 0)
                {
                    this.elements[i - 1] = this.elements[i - 1] & (maskPositionBits[bitNumber - 1] - 1);
                }
                else
                {
                    this.elements[i - 1] = this.elements[i - 1] | maskPositionBits[bitNumber - 1];
                }
                elements[i] = elements[i] << 1;
            }
            --countBits;
            UpdateList();
        }

        /// <summary>
        /// Returns the bit in the list position specified by index.
        /// </summary>
        /// <param name="index">The position index in the list.</param>
        /// <returns>A reference for that value in the list</returns>
        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= countBits)
                {
                    throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection." +
                    Environment.NewLine + "Parameter name: index");
                }
                int pointer = index / bitNumber;
                int rem = index % bitNumber;
                return GetBitFromVariable(this.elements[pointer], rem);
            }
            set
            {
                if (index < 0 || index >= countBits)
                {
                    throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection." +
                    Environment.NewLine + "Parameter name: index");
                }
                int pointer = index / bitNumber;
                int rem = index % bitNumber;
                this.elements[pointer] = SetBitInVariable(this.elements[pointer], value, rem);
            }
        }

        #endregion

        #region ICollection<int> Members

        /// <summary>
        /// Adds a bit to list of bits. Value 0 maps to off bit and other integer maps to on bit.
        /// </summary>
        /// <param name="item">The bit to add.</param>
        public void Add(int item)
        {
            long elementCounter = countBits % bitNumber;
            if (elementCounter == 0)
            {
                this.elements.Add(0);
            }
            if (item != 0)
            {
                this.elements[this.elements.Count - 1] |= maskPositionBits[elementCounter];
            }
            ++countBits;
        }

        public void Clear()
        {
            this.elements.Clear();
            this.countBits = 0;
        }

        public bool Contains(int item)
        {
            if (item == 0)
            {
                foreach (ulong val in this.elements)
                {
                    if (val != ulong.MaxValue)
                    {
                        return true;
                    }
                }
                return false;
            }
            if (item == 1)
            {
                foreach (ulong val in this.elements)
                {
                    if (val != 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Value cannot be null." + Environment.NewLine + "Parameter name: dest");
            }
            int numberOfOperations = countBits + arrayIndex;
            if (numberOfOperations > array.Length)
            {
                throw new ArgumentException("Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
            }
            int bitPointer = 0;
            int variablePointer = 0;
            int arrayPointer = arrayIndex;
            while (variablePointer < elements.Count && arrayPointer < countBits)
            {
                array[arrayPointer] = (elements[variablePointer] & maskPositionBits[bitPointer]) == 0UL ? 0 : 1;
                ++arrayPointer;
                ++bitPointer;
                if (bitPointer >= bitNumber)
                {
                    bitPointer = 0;
                    ++variablePointer;
                }
            }
        }

        public int Count
        {
            get { return this.countBits; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(int item)
        {
            if(countBits <= 0)
            {
                return false;
            }
            ulong comparer = item != 0 ? 0L : ulong.MaxValue;
            int pointer = 0;
            int bitPointer = 0;
            while (pointer < this.elements.Count - 1 && this.elements[pointer] == comparer)
            {
                ++pointer;
                bitPointer += bitNumber;
            }
            if (pointer == this.elements.Count)
            {
                return false;
            }
            int index = FindFirstBitInVariable(item, this.elements[pointer]);
            bitPointer += index;
            if (bitPointer >= countBits)
            {
                return false;
            }
            elements[pointer] = RotateVariableBetweenIndices(index, bitNumber - 1, this.elements[pointer], 1, true);
            for (int i = pointer + 1; i < elements.Count; ++i)
            {
                ulong firstBitInCurrent = this.maskPositionBits[0] & this.elements[pointer];
                if (firstBitInCurrent == 0)
                {
                    this.elements[pointer - 1] = this.elements[pointer - 1] & (maskPositionBits[bitNumber - 1] - 1);
                }
                else
                {
                    this.elements[pointer - 1] = this.elements[pointer - 1] | maskPositionBits[bitNumber - 1];
                }
                elements[pointer] = elements[pointer] << 1;
            }
            --countBits;
            UpdateList();
            return true;
        }

        #endregion

        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(ref this.elements, ref this.countBits, bitNumber, maskPositionBits);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Override members
        public override string ToString()
        {
            return "[" + GetBitsFromLongArray(elements.ToArray(), countBits) + "]";
        }
        #endregion

        /// <summary>
        /// Adds a bit list content to the end of this bit list. It behaves like a merge.
        /// </summary>
        /// <param name="range">The bit list range to add.</param>
        public void AddRange(BitList range)
        {
            int rem = this.countBits % BitList.bitNumber;
            int quo = this.countBits / BitList.bitNumber;
            ++quo;
            //ulong[] temporary = range.elements.ToArray();
            this.elements.AddRange(range.elements);
            if (rem != 0)
            {
                //ulong tempMaskHigh = maskPositionBits[bitNumber - rem] - 1;
                for (int i = quo; i < this.elements.Count; ++i)
                {
                    ulong tempMask = this.elements[i] << rem;
                    this.elements[i - 1] = (this.elements[i - 1] & (maskPositionBits[rem] - 1)) | tempMask;
                    this.elements[i] = this.elements[i] >> (bitNumber - rem);
                }
            }
            this.countBits += range.countBits;
            UpdateList();
        }

        /// <summary>
        /// Inserts another bit list begining at specified index.
        /// </summary>
        /// <param name="index">The index where to start.</param>
        /// <param name="range">The bit list range.</param>
        public void InsertRange(int index, BitList range)
        {
            if (index < 0 || index > countBits)
            {
                throw new ArgumentOutOfRangeException("Index must be within the bounds of the List." +
                Environment.NewLine + "Parameter name: index");
            }
            BitList tempBitList = this.GetSubBitList(0, index);
            tempBitList.AddRange(range);
            tempBitList.AddRange(this.GetSubBitList(index, this.countBits - index));
            this.elements = tempBitList.elements;
            this.countBits = tempBitList.countBits;
        }

        //public void InsertRangeOtherVersion(int index, BitList range)
        //{
        //    if (index < 0 || index > countBits)
        //    {
        //        throw new ArgumentOutOfRangeException("Index must be within the bounds of the List." +
        //        Environment.NewLine + "Parameter name: index");
        //    }
        //    if (range.countBits == 0)
        //    {
        //        return;
        //    }
        //    int indexQuo = index / countBits;
        //    int indexRem = index % countBits;
        //    int indexPointer = 0;
        //    int variablePointer = 0;
        //    if (indexRem == 0)
        //    {
        //        while (indexPointer < range.countBits)
        //        {
        //            this.elements.Insert(indexQuo++, range.elements[variablePointer]);
        //            ++variablePointer;
        //            indexRem += bitNumber;
        //            indexPointer += bitNumber;
        //        }
        //    }
        //    else
        //    {
        //        ulong indexVariableResidue = (~(maskPositionBits[indexRem] - 1) & this.elements[indexQuo]) >> indexRem;
        //        ulong temp = (~(maskPositionBits[bitNumber - indexRem] - 1) & range.elements[0]) << indexRem;
        //        this.elements[indexQuo] = temp | (this.elements[indexQuo] & (maskPositionBits[indexRem] - 1));

        //        while (indexPointer < range.countBits)
        //        {
        //            this.elements.Insert(indexQuo++, range.elements[variablePointer]);

        //            ++variablePointer;
        //            indexRem += bitNumber;
        //            indexPointer += bitNumber;
        //        }
        //    }
        //    this.countBits += range.countBits;
        //}

        public BitList GetSubBitList(int startIndex, int numberOfBits)
        {
            if (startIndex + numberOfBits > this.countBits)
            {
                numberOfBits = this.countBits - startIndex;
            }
            if (numberOfBits < 0)
            {
                throw new ArgumentOutOfRangeException("Start index must be inside the size of bit list and number of bits must be non negative.");
            }
            return GetUnckeckedSubBitList(startIndex, numberOfBits);
        }

        /// <summary>
        /// Get the sub bit list without check for bounds.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="numberOfBits">The number of bits.</param>
        /// <returns>The sub bit list.</returns>
        private BitList GetUnckeckedSubBitList(int startIndex, int numberOfBits)
        {
            BitList bitlist = new BitList();
            List<ulong> temporaryConstructionList = new List<ulong>();
            int startRemainder = startIndex % bitNumber;
            int startQuo = startIndex / bitNumber;
            int numberOfBitsQuo = (startRemainder + numberOfBits) / bitNumber + 1;
            if (startRemainder == 0)
            {
                for (int i = startQuo; i < numberOfBitsQuo; ++i)
                {
                    bitlist.elements.Add(this.elements[i]);
                }
            }
            else
            {
                BitList tempBitList = new BitList();
                tempBitList.countBits = 0;
                ulong valueToAdd = (~(maskPositionBits[startRemainder] - 1) & this.elements[startQuo]) >> startRemainder;
                tempBitList.elements.Add(valueToAdd);
                tempBitList.countBits = bitNumber - startRemainder;
                bitlist.AddRange(tempBitList);
                for (int i = startQuo + 1; i < numberOfBitsQuo; ++i)
                {
                    tempBitList = new BitList();
                    tempBitList.elements.Add(this.elements[i]);
                    tempBitList.countBits = bitNumber;
                    bitlist.AddRange(tempBitList);
                }
            }
            bitlist.countBits = numberOfBits;
            bitlist.UpdateList();
            return bitlist;
        }

        private void InitMask()
        {
            ulong powerOfTwo = 1;
            maskPositionBits = new ulong[bitNumber];
            maskPositionBits[0] = powerOfTwo;
            for (int i = 1; i < bitNumber; ++i)
            {
                powerOfTwo = powerOfTwo << 1;
                maskPositionBits[i] = powerOfTwo;
            }
        }

        /// <summary>
        /// This function updates the list of elements after a set of operations
        /// </summary>
        private void UpdateList()
        {
            if (countBits / bitNumber < elements.Count - 1)
            {
                int numberOfVariablesNeeded = countBits / bitNumber + 1;
                while (this.elements.Count > numberOfVariablesNeeded)
                {
                    this.elements.RemoveAt(this.elements.Count - 1);
                }
            }
        }

        /// <summary>
        /// Rotates the bits of a variable comprehended between the specified indices. Indices are included in rotation.
        /// </summary>
        /// <param name="index1">The left most index.</param>
        /// <param name="index2">The right most index.</param>
        /// <param name="variable">The variable to rotate.</param>
        /// <param name="rotationNumber">The number of rotation places.</param>
        /// <param name="direction">True for rotate left and false for rotate right.</param>
        /// <returns>The variable with bits rotated.</returns>
        private ulong RotateVariableBetweenIndices(int index1, int index2, ulong variable, int rotationNumber, bool direction)
        {
            if (index1 > index2)
            {
                return variable;
            }
            ulong mask = ~(maskPositionBits[index1] - 1);
            if (index2 < bitNumber - 1)
            {
                mask &= (maskPositionBits[index2] - 1);
            }
            else
            {
                mask &= ulong.MaxValue;
            }
            ulong result = variable & mask;
            if (direction)
            {
                result = result >> rotationNumber;
            }
            else
            {
                result = result << rotationNumber;
            }
            return (variable & ~mask) | (result & mask);
        }

        /// <summary>
        /// Find the first bit in an array variable.
        /// </summary>
        /// <param name="bitToFind">The value of bit to find. A bit is represent either by a zero or a value different from zero.</param>
        /// <param name="variable">The variable to search.</param>
        /// <returns>The index of first occurence and -1 otherwise.</returns>
        private int FindFirstBitInVariable(int bitToFind, ulong variable)
        {
            for (int i = 0; i < bitNumber; ++i)
            {
                ulong temporary = variable & maskPositionBits[i];
                if (bitToFind == 0 && temporary == 0)
                {
                    return i;
                }
                if (bitToFind != 0 && temporary != 0)
                {
                    return i;
                }
            }
            return -1;
        }

        private ulong SetBitInVariable(ulong variable, int bitToSet, int index)
        {
            if (index < 0 || index > bitNumber)
            {
                return variable;
            }
            ulong result = variable;
            if (bitToSet == 0)
            {
                result = result & ~maskPositionBits[index];
            }
            else
            {
                result = result | maskPositionBits[index];
            }
            return result;
        }

        /// <summary>
        /// Gets a bit from variable specified by index. Out of bound errors will by the class interface functions.
        /// Do not forget to account for index bounds when this function is used.
        /// </summary>
        /// <param name="variable">The variable from where to get the bit.</param>
        /// <param name="index">The zero-based index position for bit retrieving.</param>
        /// <returns></returns>
        private int GetBitFromVariable(ulong variable, int index)
        {
            return (maskPositionBits[index] & variable) == 0 ? 0 : 1;
        }

        /// <summary>
        /// Gets a string with all bits in the array.
        /// </summary>
        /// <param name="array">The array to extract the bit string.</param>
        /// <param name="countBits">How many bits are there in the array.</param>
        /// <returns>The string with the specified bits.</returns>
        private string GetBitsFromLongArray(ulong[] array, long countBits)
        {
            if (countBits / bitNumber > array.Length)
            {
                return "Bit overflow";
            }
            StringBuilder resultBuilder = new StringBuilder();
            for (long i = 0; i < countBits; ++i)
            {
                long quo = i / bitNumber;
                long rem = i % bitNumber;
                resultBuilder.Append((array[quo] & maskPositionBits[rem]) != 0 ? 1 : 0);
            }
            return resultBuilder.ToString();
        }

        private void SetAllBits(bool set)
        {
            if (countBits == 0)
            {
                return;
            }
            int variableIndex = countBits / bitNumber;
            int indexInLastVariable = countBits % bitNumber;
            if (indexInLastVariable == 0)
            {
                variableIndex--;
            }
            if (set)
            {
                for (int i = 0; i < variableIndex + 1 && i < this.elements.Count; ++i)
                {
                    this.elements[i] = ulong.MaxValue;
                }
                for (int i = this.elements.Count; i < variableIndex + 1; ++i)
                {
                    this.elements.Add(ulong.MaxValue);
                }
            }
            else
            {
                for (int i = 0; i < variableIndex + 1 && i < this.elements.Count; ++i)
                {
                    this.elements[i] = 0;
                }
                for (int i = this.elements.Count; i < variableIndex + 1; ++i)
                {
                    this.elements.Add(0);
                }
            }
        }

        private void Reserve(int capacity)
         {
             int listCapacity = capacity / bitNumber;
             if (capacity % bitNumber != 0)
             {
                 ++listCapacity;
             }
             elements = new List<ulong>(listCapacity);
         }

        /// <summary>
        /// Dumps all bits from a variable.
        /// </summary>
        /// <param name="variable">The variable containing the bits to dump.</param>
        /// <returns>A string with the dumped bits.</returns>
        private string DumpVariable(ulong variable)
        {
            StringBuilder resultBuilder = new StringBuilder();
            for (int i = 0; i < bitNumber; ++i)
            {
                resultBuilder.Append((variable & maskPositionBits[i]) != 0 ? 1 : 0);
            }
            return resultBuilder.ToString();
        }

        private class Enumerator : IEnumerator<int>
        {
            #region fields
            /// <summary>
            /// The number of bits in a variable.
            /// </summary>
            int bitNumber;

            ulong[] maskPositionBits;

            /// <summary>
            /// The bitlist elements.
            /// </summary>
            List<ulong> elements;

            /// <summary>
            /// The bitlist number of bits.
            /// </summary>
            int countBits;

            /// <summary>
            /// Pointer for each of list elements.
            /// </summary>
            int variablePointer;

            /// <summary>
            /// Pointer for each bit in variable and is less than bit number.
            /// </summary>
            int bitPointer;

            /// <summary>
            /// General controling pointer.
            /// </summary>
            int elementPointer;
            #endregion

            public Enumerator(ref List<ulong> elements, ref int countBits, int bitNumber, ulong[] maskPositionBits)
            {
                this.elements = elements;
                this.countBits = countBits;
                this.bitNumber = bitNumber;
                this.maskPositionBits = maskPositionBits;
                this.variablePointer = 0;
                this.bitPointer = -1;
                this.elementPointer = -1;
            }

            #region IEnumerator<int> Members

            public int Current
            {
                get
                {
                    if (elementPointer < 0 || elementPointer >= countBits)
                    {
                        return 0;
                    }
                    return (maskPositionBits[bitPointer] & this.elements[variablePointer]) == 0 ? 0 : 1;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                this.elements = null;
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (elementPointer < 0 || elementPointer >= countBits)
                    {
                        return 0;
                    }
                    return (maskPositionBits[bitPointer] & this.elements[variablePointer]) == 0 ? 0 : 1;
                }
            }

            public bool MoveNext()
            {
                ++elementPointer;
                ++bitPointer;
                if (bitPointer >= bitNumber)
                {
                    ++variablePointer;
                    bitPointer = 0;
                }
                if (elementPointer >= countBits)
                {
                    return false;
                }
                return true;
            }

            public void Reset()
            {
                elementPointer = -1;
                bitPointer = -1;
                variablePointer = 0;
            }
            #endregion
        }
    }
}
