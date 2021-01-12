using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerationRoslynTest.Exceptions
{
    public class CodeGenerationException : Exception
    {
        public CodeGenerationException(string message) : base(message)
        {

        }

        public CodeGenerationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
