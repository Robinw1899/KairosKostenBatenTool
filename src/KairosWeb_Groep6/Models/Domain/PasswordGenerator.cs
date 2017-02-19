using System;

namespace KairosWeb_Groep6.Models.Domain
{
    public class PasswordGenerator
    {
        public static string GeneratePassword(int length)
        {
            string randomstring = "";
            Random random = new Random();
            char[] charArray = "ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz0123456789".ToCharArray();

            for (var i = 0; i < length; i++)
            {
                int index = random.Next(0, charArray.Length);
                char kar = charArray[index];
                randomstring += kar;
            }
            return randomstring;
        }
    }
}
