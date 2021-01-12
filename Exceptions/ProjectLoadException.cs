using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerationRoslynTest.Exceptions
{
    public class ProjectLoadException : Exception
    {
        public ProjectLoadException(string message) : base(message)
        {

        }

        public ProjectLoadException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
