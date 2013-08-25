namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LegendreJacobiSymbolAlgorithm : IAlgorithm<int, int, int>
    {
        /// <summary>
        /// Calcula o valor do símbolo de Jacobi para inteiros.
        /// </summary>
        /// <param name="topSymbolValue">O valor superior do símbolo.</param>
        /// <param name="bottomSymbolValue">O valor inferior do símbolo.</param>
        /// <returns>O valor numérico do símbolo.</returns>
        public int Run(int topSymbolValue, int bottomSymbolValue)
        {
            if (bottomSymbolValue == 0)
            {
                throw new ArgumentException("The bottom symbol value mustn't be zero.");
            }
            else if (topSymbolValue == 0)
            {
                return 0;
            }
            else if (topSymbolValue % 2 == 0 && bottomSymbolValue % 2 == 0)
            {
                return 0;
            }
            else
            {
                var result = 1;
                var innerBottomSymbolValue = bottomSymbolValue;

                // Uma vez que J(p,2) = J(2,p)
                var power = 0;
                while (innerBottomSymbolValue % 2 == 0)
                {
                    ++power;
                    innerBottomSymbolValue = innerBottomSymbolValue / 2;
                }

                var topRemainder = 0;
                if (power % 2 != 0)
                {
                    topRemainder = topSymbolValue % 8;
                    if (topRemainder == 3 || topRemainder == 5)
                    {
                        result = -result;
                    }
                }

                if (innerBottomSymbolValue != 1)
                {
                    var innerTopSymbolValue = topSymbolValue % bottomSymbolValue;
                    var state = 0;
                    while (state != -1)
                    {
                        if (innerTopSymbolValue == 0)
                        {
                            result = 0;
                            state = -1;
                        }
                        else if (innerTopSymbolValue == 1)
                        {
                            state = -1;
                        }
                        else
                        {
                            power = 0;
                            while (innerTopSymbolValue % 2 == 0)
                            {
                                ++power;
                                innerTopSymbolValue = innerTopSymbolValue / 2;
                            }

                            var bottomRemainder = 0;
                            if (power % 2 != 0)
                            {
                                bottomRemainder = innerBottomSymbolValue % 8;
                                if (bottomRemainder == 3 || bottomRemainder == 5)
                                {
                                    result = -result;
                                }
                            }

                            if (innerTopSymbolValue != 1)
                            {
                                bottomRemainder = innerBottomSymbolValue % 4;
                                topRemainder = innerTopSymbolValue % 4;
                                if (bottomRemainder == 3 && topRemainder == 3)
                                {
                                    result = -result;
                                }

                                var temporaryBottomSymbolValue = innerBottomSymbolValue;
                                innerBottomSymbolValue = innerTopSymbolValue;
                                innerTopSymbolValue = temporaryBottomSymbolValue % innerTopSymbolValue;
                            }
                        }
                    }
                }

                return result;
            }
        }
    }
}
