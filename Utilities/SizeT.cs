namespace Utilities
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Permite mapear o tipo de variável size_t num contexto de interoperabilidade.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SizeT
    {
        /// <summary>
        /// O apontador que contém o valor do size_t.
        /// </summary>
        private IntPtr pointer;

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="SizeT"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        public SizeT(int value)
        {
            this.pointer = new IntPtr(value);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SizeT"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        public SizeT(uint value)
        {
            this.pointer = new IntPtr((int)value);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SizeT"/>.
        /// </summary>
        /// <param name="value">O valor.</param>
        public SizeT(long value)
        {
            this.pointer = new IntPtr(value);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SizeT"/>.
        /// </summary>
        /// <param name="value">O valor</param>
        public SizeT(ulong value)
        {
            this.pointer = new IntPtr((long)value);
        }

        /// <summary>
        /// Define a conversão implícita entre um objecto do tipo <see cref="SizeT"/> e um número inteiro.
        /// </summary>
        /// <param name="value">O valor a ser convertido.</param>
        /// <returns>O valor convertido.</returns>
        public static implicit operator int(SizeT value)
        {
            return value.pointer.ToInt32();
        }

        /// <summary>
        /// Define a conversão implícita entre um objecto do tipo <see cref="SizeT"/> e um número inteiro 
        /// sem sinal.
        /// </summary>
        /// <param name="value">O valor a ser convertido.</param>
        /// <returns>O valor convertido.</returns>
        public static implicit operator uint(SizeT value)
        {
            return (uint)((int)value.pointer);
        }

        /// <summary>
        /// Define a conversão implícita entre um objecto do tipo <see cref="SizeT"/> e um número inteiro longo.
        /// </summary>
        /// <param name="value">O valor a ser convertido.</param>
        /// <returns>O valor convertido.</returns>
        public static implicit operator long(SizeT value)
        {
            return value.pointer.ToInt64();
        }

        /// <summary>
        /// Define a conversão implícita entre um objecto do tipo <see cref="SizeT"/> e um número inteiro longo
        /// sem sinal.
        /// </summary>
        /// <param name="value">O valor a ser convertido.</param>
        /// <returns>O valor convertido.</returns>
        public static implicit operator ulong(SizeT value)
        {
            return (ulong)((long)value.pointer);
        }

        /// <summary>
        /// Define a conversão implícita entre um número inteiro e um objecto do tipo <see cref="SizeT"/>.
        /// </summary>
        /// <param name="value">O valor a ser convertido.</param>
        /// <returns>O valor convertido.</returns>
        public static implicit operator SizeT(int value)
        {
            return new SizeT(value);
        }

        /// <summary>
        /// Define a conversão implícita entre um número inteiro sem sinal e um objecto do tipo 
        /// <see cref="SizeT"/>.
        /// </summary>
        /// <param name="value">O valor a ser convertido.</param>
        /// <returns>O valor convertido.</returns>
        public static implicit operator SizeT(uint value)
        {
            return new SizeT(value);
        }

        /// <summary>
        /// Define a conversão implícita entre um número inteiro longo e um objecto do tipo <see cref="SizeT"/>.
        /// </summary>
        /// <param name="value">O valor a ser convertido.</param>
        /// <returns>O valor convertido.</returns>
        public static implicit operator SizeT(long value)
        {
            return new SizeT(value);
        }

        /// <summary>
        /// Define a conversão implícita entre um número inteiro longo sem sinal e um objecto do tipo
        /// <see cref="SizeT"/>.
        /// </summary>
        /// <param name="value">O valor a ser convertido.</param>
        /// <returns>O valor convertido.</returns>
        public static implicit operator SizeT(ulong value)
        {
            return new SizeT(value);
        }

        /// <summary>
        /// Sobre carrega o operador de diferença entre dois valores de size_t.
        /// </summary>
        /// <param name="first">O primeiro valor a ser comparado.</param>
        /// <param name="second">O segundo valor a ser comparado.</param>
        /// <returns>Verdadeiro se ambos os valores forem diferentes e falso caso contrário.</returns>
        public static bool operator !=(SizeT first, SizeT second)
        {
            return (first.pointer != second.pointer);
        }

        /// <summary>
        /// Sobrecarrega o operador de comparação de igualdade entre dois valores de size_t.
        /// </summary>
        /// <param name="first">O primeiro valor.</param>
        /// <param name="second">O segundo valor.</param>
        /// <returns>Verdadeiro caso os valores sejam iguais e falso caso contrário.</returns>
        public static bool operator ==(SizeT first, SizeT second)
        {
            return (first.pointer == second.pointer);
        }

        /// <summary>
        /// O valor zero.
        /// </summary>
        public static SizeT Zero
        {
            get
            {
                return new SizeT(0);
            }
        }

        /// <summary>
        /// Determina se o objecto proporcionado é igual ao objecto corrente.
        /// </summary>
        /// <param name="obj">O objecto a ser compardo.</param>
        /// <returns>Verdadeiro caso o objecto proporcionado seja igual ao objecto corrente e falso 
        /// caso contrário.</returns>
        public override bool Equals(object obj)
        {
            return this.pointer.Equals(obj);
        }

        /// <summary>
        /// Obtém o código confuso associado ao objecto corrente.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            return this.pointer.GetHashCode();
        }

        /// <summary>
        /// Obtém uma representação textual do objecto corrente.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return this.pointer.ToString();
        }
    }
}
