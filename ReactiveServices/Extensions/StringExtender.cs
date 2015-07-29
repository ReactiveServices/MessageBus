using System.Text;

namespace ReactiveServices.Extensions
{
    public sealed class StringExtender
    {
        private StringExtender()
        {
            CompleteChar = ' ';
        }

        private string Value { get; set; }

        public static StringExtender ForString(string str)
        {
            return new StringExtender
            {
                Value = str,
                Length = str.Length
            };
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return obj.ToString() == ToString();
        }

        public static implicit operator string(StringExtender extender)
        {
            return extender.ToString();
        }

        public override string ToString()
        {
            var result = new StringBuilder(Value);
            if (Length > Value.Length)
            {
                while (result.Length < Length)
                {
                    if (UseRightOrientation)
                        result.Append(CompleteChar);
                    else
                        result.Insert(0, CompleteChar);
                }
            }
            return result.ToString();
        }

        private int Length { get; set; }

        public StringExtender WithLength(int length)
        {
            Length = length;
            return this;
        }

        private char CompleteChar { get; set; } 

        public StringExtender CompletedWith(char completeChar)
        {
            CompleteChar = completeChar;
            return this;
        }

        /// <summary>
        ///  Se for verdadeiro alinha o texto a direita, senão alinha o texto à esquerda
        /// </summary>
        private bool UseRightOrientation { get; set; } 

        public StringExtender AtLeft() 
        {
            UseRightOrientation = false; 
            return this;
        }
        public StringExtender AtRight() 
        {
            UseRightOrientation = true; 
            return this;
        }
    }
}
