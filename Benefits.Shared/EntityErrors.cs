using System;
using System.Collections.Generic;

namespace Benefits.Shared
{
    public class EntityError
    {
        public override string ToString() => FieldName + ": " + Message;

        public string FieldName { get; set; }
        public string Message { get; set; }
    }

    public class EntityErrors : List<EntityError>
    {
        public void Add(string fieldName, string message)
        {
            if (message != null)
                Add(new EntityError { FieldName = fieldName, Message = message });
        }
    }
}