using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Exceptions.Doctor
{
    public class PhotoFileContentException : Exception
    {
        public string PropertyName { get; set; }
        public PhotoFileContentException(string propName,string? message) : base(message)
        {
            PropertyName = propName;
        }
    }
}
