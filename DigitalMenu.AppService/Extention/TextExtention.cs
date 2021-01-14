using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DigitalMenu.AppService.Extention
{
    public static class TextExtention
    {
        public static string ToTitleForStore(this string input)
        {
            input = input.TrimStart().TrimEnd();
            input = Regex.Replace(input, @"\s", "_");
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            input = textInfo.ToTitleCase(input);
            
            return input;
        }

        public static string ToTitleForView(this string input)
        {            
            input = Regex.Replace(input, @"_", " ");                        

            return input;
        }
    }
}
