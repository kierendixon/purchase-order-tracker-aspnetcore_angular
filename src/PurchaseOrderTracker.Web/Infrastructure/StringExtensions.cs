﻿namespace PurchaseOrderTracker.Infrastructure
{
    public static class StringExtensions
    {
        public static string FirstCharacterToLower(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
                return str;

            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}