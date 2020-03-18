namespace ToleranceValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class FieldValidator
    {
        public abstract Type Type { get; }
        public abstract bool HasErrors { get; }
    }

    public class FieldValidator<T> : FieldValidator
    {
        private readonly T _firstValue;
        private readonly T _secondValue;
        private readonly T _dataValue;
        private readonly string _fieldName;
        private readonly HashSet<Tuple<string,string>> _validationErrors;

        public FieldValidator(T firstValue, T secondValue, T dataValue, string fieldName)
        {
            _firstValue = firstValue;
            _fieldName = fieldName;
            _secondValue = secondValue;
            _dataValue = dataValue;
            _validationErrors = new HashSet<Tuple<string, string>>();
        }
        public override Type Type => typeof(T);

        public ValidationResult GetResult()
        {
            return new ValidationResult(_fieldName, _validationErrors, HasErrors);
        }

        public override bool HasErrors => _validationErrors.Any();

        public FieldValidator<T> AddRule(Func<T, T, T, bool> rule, string message)
        {
            if (!rule(_firstValue, _secondValue, _dataValue))
                _validationErrors.Add(new Tuple<string, string>(_fieldName, message));

            return this;
        }

        public FieldValidator<T> AddRule(Func<T, T, bool> rule, string message)
        {
            if (!rule(_firstValue, _dataValue))
                _validationErrors.Add(new Tuple<string, string>(_fieldName, message));

            return this;
        }
    }
}