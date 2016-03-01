namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class LabelsComparer : Comparer<Label>
    {
        /// <summary>
        /// Permite comparar duas referências com base na lista de bits contida na secção MTD, M e TM.
        /// </summary>
        /// <param name="x">A primeira referência a ser comparada.</param>
        /// <param name="y">A segunda referência a ser comparada.</param>
        /// <returns>
        /// O resultado da comparação, -1 caso x seja menor que y, 0 caso ambos sejam iguais e 1 caso seja seja
        /// superior a y.
        /// </returns>
        public override int Compare(Label x, Label y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            else if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            else
            {
                var compareValue = x.MtdBits.CompareTo(y.MtdBits);
                if (compareValue == 0)
                {
                    compareValue = x.MBits.CompareTo(y.MBits);
                    if (compareValue == 0)
                    {
                        return -x.TmBits.CompareTo(y.TmBits);
                    }
                    else
                    {
                        return -compareValue;
                    }
                }
                else
                {
                    return -compareValue;
                }
            }
        }
    }
}
