namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class FieldDrivenExpressionReader<ObjectType, InputType>
        : RingDrivenExpressionReader<ObjectType, InputType>
    {
        /// <summary>
        /// O corpo responsável pelas operações sobre os elementos.
        /// </summary>
        private IField<ObjectType> field;

        public FieldDrivenExpressionReader(
            IParse<ObjectType, string, string> objectParser,
            IField<ObjectType> field,
            IIntegerNumber<ObjectType> integerNumber = null) : base(objectParser, field, integerNumber)
        {
            // A verificação de nulidade do campo é levada a cabo na classe base.
            this.field = field;
        }

        /// <summary>
        /// Divide dois elementos.
        /// </summary>
        /// <param name="left">O primeiro elemento.</param>
        /// <param name="right">O segundo elemento.</param>
        /// <returns>O elemento resultante.</returns>
        protected override ObjectType Divide(ObjectType left, ObjectType right)
        {
            return this.field.Multiply(
                left,
                this.field.MultiplicativeInverse(right));
        }
    }
}
