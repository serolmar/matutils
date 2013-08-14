// -----------------------------------------------------------------------
// <copyright file="ElementToElementConversion.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public class ElementToElementConversion<ElementType> : IConversion<ElementType, ElementType>
    {
        public bool CanApplyDirectConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CanApplyInverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ElementType DirectConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return objectToConvert;
            }
        }

        public ElementType InverseConversion(ElementType objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else
            {
                return objectToConvert;
            }
        }
    }
}
