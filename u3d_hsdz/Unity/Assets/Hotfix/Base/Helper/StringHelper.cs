using System;
using System.Text.RegularExpressions;

namespace ETHotfix
{
	public static class StringHelper
	{
        /// <summary>
        /// 显示带正负符号的数字字符串，不区分大于1000数字的显示
        /// </summary>
        public static string GetSignedString(int num, bool sgold=true)
        {
            if (num == 0)
            {
                return "0";
            }

            string realvalue = "";
            if (sgold)
            {
                realvalue = ShowGold(num, false);
            }
            else
            {
                realvalue = num.ToString();
            }
                

            if (num > 0)
            {
                return $"+{realvalue}";
            }
            else
            {
                return $"{realvalue}";
            }
        }

        /// <summary>
        /// 不显示符号，默认都是正数
        /// </summary>
        public static string GetShortString(int num)
        {
            return ShowGold(num, true);
        }


        /// <summary>
        /// 显示带正负符号的数字字符串  >10w
        /// </summary>
        public static string GetShortSignedString(int num)
        {
            if (num == 0)
            {
                return "0";
            }
            int absValue = Math.Abs(num);

            //返回小数点
            string realvalue = ShowGold(absValue, true);
            if (num > 0)
            {
                return $"+{realvalue}";
            }
            else
            {
                return $"-{realvalue}";
            }
        }

        /// <summary>
        /// 是否包含特殊字符
        /// </summary>
        public static bool IsContainSpecialCharacter(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            Regex regex = new Regex("^[a-zA-Z0-9\u4e00-\u9fa5]+$");
            return !regex.IsMatch(text);
        }

        public static bool isNumberic(string message, out int result)
        {
            //判断是否为整数字符串
            //是的话则将其转换为数字并将其设为out类型的输出值、返回true, 否则为false
            result = -1;   //result 定义为out 用来输出值
            try
            {
                result = Convert.ToInt32(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

       

        //显示小数USDT
        public static string ShowGold(int bean, bool bUnit=false)
        {
            if (bean == 0)
            {
                return "0";
            }

            string result = "";
            double realValue = (double)(bean / 100.0);
            if (bUnit && realValue > 10000)
            {
                realValue = realValue / 1000;
                result = string.Format("{0:f2}k", realValue);
            }
            else
            {
                result = string.Format("{0}", realValue);
            }

            return $"{result}";
        }

        public static int GetRateGold(int bean)
        {
            return bean / 100;
        }

        //显示小数USDT带正负号
        public static string ShowSignedGold(int bean, bool bUnit)
        {
            string result = "";
            float realValue = (float)(bean / 100.0);

            if (bUnit && realValue > 10000)
            {
                realValue = realValue / 1000;
                result = string.Format("{0:f2}k", realValue);
            }
            else
            {
                //result = string.Format("{0:f2}", realValue);
                result = string.Format("{0}", realValue);
            }


            if (realValue > 0)
            {
                return $"+{result}";

            }
            else if (realValue < 0)
            {
                return $"{result}";
            }

            return result;
        }

        //获取实际USDT
        public static int GetIntGold(float bean)
        {
            int result = Convert.ToInt32(bean*100);
            return result;
        }

        //显示大小盲和前注
        public static string ShowBlinds(string small, string big, string qianzhu, string zhuatou = "")
        {
            string result = small;
            if (big != "")
            {
                result = string.Format("{0}/{1}", small, big);
            }

            if (zhuatou != "")
            {
                result = string.Format("{0}/{1}", result, zhuatou);
            }

            if (qianzhu != "0")
            {
                result = string.Format("{0}({1})", result, qianzhu);
            }
          
            return result;
        }

        public static string ShowBlindName(string blindName)
        {
            var strs = blindName.Split('/');
            int small = int.Parse(strs[0]);
            int big = int.Parse(strs[1]);
            if (strs.Length == 3)
            {
                int qianzhu = int.Parse(strs[2]);
                return string.Format("{0}/{1}/{2}", ShowGold(small), ShowGold(big), ShowGold(qianzhu));
            }
            else if(strs.Length == 2)
            {
                return string.Format("{0}/{1}", ShowGold(small), ShowGold(big));
            }

            return blindName;

        }
    }
}