namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class Label
    {
        private BitList mtdBits;

        private BitList mBits;

        private BitList tmBits;

        private double price;

        private double rate;

        private double carsNumber;

        public Label()
        {
            this.mtdBits = new BitList();
            this.mBits = new BitList();
            this.tmBits = new BitList();
        }

        public BitList MtdBits
        {
            get
            {
                return this.mtdBits;
            }
        }

        public BitList MBits
        {
            get
            {
                return this.mBits;
            }
        }

        public BitList TmBits
        {
            get
            {
                return this.tmBits;
            }
        }

        public double Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }

        public double Rate
        {
            get
            {
                return this.rate;
            }
            set
            {
                this.rate = value;
            }
        }

        public double CarsNumber
        {
            get
            {
                return this.carsNumber;
            }
            set
            {
                this.carsNumber = value;
            }
        }
    }
}
