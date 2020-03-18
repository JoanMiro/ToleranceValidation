namespace ToleranceValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ValidationResult
    {
        public string FieldName { get; }
        
        public ICollection<Tuple<string,string>> ValidationErrors { get; }

        public bool HasErrors { get; }

        public ValidationResult(string fieldName, ICollection<Tuple<string,string>> validationErrors, bool hasErrors)
        {
            this.FieldName = fieldName;
            this.ValidationErrors = validationErrors;
            this.HasErrors = hasErrors;
        }

        public string Message()
        {
            return string.Join(Environment.NewLine, this.ValidationErrors.Select(error => $"{this.FieldName}: {error}"));
        }
    }
}