using PhoneNumbers;

namespace Common.Utilities
{
    public static class PhoneNumberValidation
    {
        public static bool IsValidMobilePhoneNumber(string phoneNumber, string countryCode)
        {
            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var phoneNumberParsed = phoneNumberUtil.Parse(phoneNumber, countryCode);
                return phoneNumberUtil.IsValidNumber(phoneNumberParsed) && phoneNumberUtil.GetNumberType(phoneNumberParsed) == PhoneNumberType.MOBILE;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool IsValidMobilePhoneNumber(string phoneNumber)
        {
            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var phoneNumberParsed = phoneNumberUtil.Parse(phoneNumber, null);
                var type1 = phoneNumberUtil.GetNumberType(phoneNumberParsed);
                var type2 = phoneNumberUtil.IsValidNumber(phoneNumberParsed);
                return type2 && type1 is PhoneNumberType.MOBILE or PhoneNumberType.FIXED_LINE_OR_MOBILE;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
