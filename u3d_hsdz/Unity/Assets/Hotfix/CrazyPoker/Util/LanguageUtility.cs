namespace ETHotfix
{
    public class LanguageUtility
    {
        /// <summary>
        /// 对应配表文件名
        /// </summary>
        public enum LanguageType
        {
            /// <summary>
            /// 简体
            /// </summary>
            USER_ZH = 0,
            /// <summary>
            /// 英文
            /// </summary>
            USER_EN = 1,
            /// <summary>
            /// 繁体
            /// </summary>
            USER_TW = 2,
        }

        public static string LanguageIdToName(int id)
        {
            return ((LanguageType) id).ToString();
        }
    }
}
