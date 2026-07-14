using System;

namespace IfaUdi.Parser
{
    public sealed class IfaUdiBuildException : Exception
    {
        public string Field { get; }
        public string Reason { get; }

        public IfaUdiBuildException(string message, string field, string reason) : base($"{message} (field {field})")
        {
            Field = field;
            Reason = reason;
        }
    }
}
