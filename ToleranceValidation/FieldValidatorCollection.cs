namespace ToleranceValidation
{
    using System.Collections.Generic;

    public class FieldValidatorCollection : List<FieldValidator>
    {
        public bool HasErrors => !this.TrueForAll(fv => fv.HasErrors);
    }
}