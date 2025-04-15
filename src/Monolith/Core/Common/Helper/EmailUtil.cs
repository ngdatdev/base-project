using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper;

public class EmailUtil
{
    public static bool IsValid(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string Mask(string email)
    {
        var parts = email.Split('@');
        if (parts.Length != 2)
            return email;
        return parts[0].Substring(0, 1) + "***@" + parts[1];
    }
}
