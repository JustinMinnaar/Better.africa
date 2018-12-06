using System;
using System.Runtime.Serialization;

namespace BetterAfrica.Shared
{
    [Serializable]
    public class BenefitsException : Exception
    {
        public BenefitsException()
        {
        }

        public BenefitsException(string message) : base(message)
        {
        }

        public BenefitsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BenefitsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}