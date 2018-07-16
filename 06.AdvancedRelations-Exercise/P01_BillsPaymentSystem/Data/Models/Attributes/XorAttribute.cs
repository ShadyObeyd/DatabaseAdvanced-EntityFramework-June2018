namespace P01_BillsPaymentSystem.Data.Models.Attributes
{
    using System.ComponentModel.DataAnnotations;

    public class XorAttribute : ValidationAttribute
    {
        private string xorTargetAttribute;

        public XorAttribute(string xorTargetAttribute)
        {
            this.xorTargetAttribute = xorTargetAttribute;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var targetAttritbute = validationContext.ObjectType.GetProperty(xorTargetAttribute).GetValue(validationContext.ObjectInstance);

            if ((targetAttritbute == null && value != null) || (targetAttritbute != null && value == null))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Properties \"BankAccountId\" and \"CreditCardId\": One has to be null and the other has to have a value!");
        }
    }
}
