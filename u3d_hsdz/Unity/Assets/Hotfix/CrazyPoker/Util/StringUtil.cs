using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StringUtil
{

    static public string FilterLetter(string strFilter) {

        string str = System.Text.RegularExpressions.Regex.Replace(strFilter, @"[^0-9]+", "");
        if(str.Length >= 2)
        {
            int num;
            if(int.TryParse(str, out num))
            {
                return num.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        return str;
    }

    /// <summary>
    /// 获取带汉字的字符长度
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    static public int GetStringLength(string str) {

        if (str.Length == 0)
            return 0;
        ASCIIEncoding ascii = new ASCIIEncoding();
        int tempLen = 0;
        byte[] s = ascii.GetBytes(str);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                tempLen += 2;
            }
            else
            {
                tempLen += 1;
            }
        }
        return tempLen;
    }
}