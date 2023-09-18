﻿using PhoneNumbers;

namespace Common.Utilities
{
    public static class PhoneNumberValidation
    {
        public static bool IsValidMobilePhoneNumber(string phoneNumber, string countryCode)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var phoneNumberParsed = phoneNumberUtil.Parse(phoneNumber, countryCode);
            return phoneNumberUtil.IsValidNumber(phoneNumberParsed) && phoneNumberUtil.GetNumberType(phoneNumberParsed) == PhoneNumberType.MOBILE;
        }
    }

}
