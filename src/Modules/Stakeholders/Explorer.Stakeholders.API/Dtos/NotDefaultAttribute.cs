using System;
using System.ComponentModel.DataAnnotations;

namespace Explorer.Stakeholders.API.Dtos
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotDefaultAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be the default value.";

        public NotDefaultAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            var type = value.GetType();
            if (type.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(type);
                return !value.Equals(defaultValue);
            }

            return true;
        }
    }
}
