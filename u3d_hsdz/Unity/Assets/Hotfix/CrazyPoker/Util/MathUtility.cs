using System.Collections.Generic;

public class MathUtility
{

    /// <summary>
    /// 交换两个变量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a"></param>
    /// <param name="b"></param>
    private static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    /// <summary>
    /// 递归算法求数组的组合
    /// </summary>
    /// <typeparam name="T">返回的范型</typeparam>
    /// <param name="list">所求数组</param>
    /// <param name="t"></param>
    /// <param name="n"></param>
    /// <param name="m"></param>
    /// <param name="b"></param>
    /// <param name="M"></param>
    private static void GetCombination<T>(ref List<T[]> list, T[] t, int n, int m, int[] b, int M)
    {
        for (int i = n; i >= m; i--)
        {
            b[m - 1] = i - 1;
            if (m > 1)
            {
                GetCombination<T>(ref list, t, i - 1, m - 1, b, M);
            }
            else
            {
                if (null == list)
                {
                    list = new List<T[]>();
                }
                T[] temp = new T[M];
                for (int j = 0, k = b.Length; j < k; j++)
                {
                    temp[j] = t[b[j]];
                }

                list.Add(temp);
            }
        }
    }

    /// <summary>
    /// 递归算法求排列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">返回的列表</param>
    /// <param name="t">所求数组</param>
    /// <param name="startIndex">起始标号</param>
    /// <param name="endIndex">结束标号</param>
    private static void GetPermutation<T>(ref List<T[]> list, T[] t, int startIndex, int endIndex)
    {
        if (startIndex == endIndex)
        {
            if (null == list)
            {
                list = new List<T[]>();
            }
            T[] temp = new T[t.Length];
            t.CopyTo(temp, 0);
            list.Add(temp);
        }
        else
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                Swap<T>(ref t[startIndex], ref t[i]);
                GetPermutation<T>(ref list, t, startIndex + 1, endIndex);
                Swap(ref t[startIndex], ref t[i]);
            }
        }
    }

    /// <summary>
    /// 求从起始标号到结束标号的排列，其余元素不变
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">所求数组</param>
    /// <param name="startIndex">起始标号</param>
    /// <param name="endIndex">结束标号</param>
    /// <returns>从起始标号到结束标号排列的范型</returns>
    public static List<T[]> GetPermutation<T>(T[] t, int startIndex, int endIndex)
    {
        if (startIndex < 0 || endIndex > t.Length - 1)
        {
            return null;
        }

        List<T[]> list = new List<T[]>();
        GetPermutation(ref list, t, startIndex, endIndex);
        return list;
    }

    /// <summary>
    /// 返回数组所有元素的全排列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">所求数组</param>
    /// <returns>全排列的范型</returns>
    public static List<T[]> GetPermutation<T>(T[] t)
    {
        return GetPermutation(t, 0, t.Length - 1);
    }

    /// <summary>
    /// 求数组中n个元素的排列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">所求数组</param>
    /// <param name="n">元素个数</param>
    /// <returns>数组中n个元素的排列</returns>
    public static List<T[]> GetPermutation<T>(T[] t, int n)
    {
        if (n > t.Length)
        {
            return null;
        }

        List<T[]> list = new List<T[]>();
        List<T[]> c = GetCombination(t, n);
        for (int i = 0, nn = c.Count; i < nn; i++)
        {
            List<T[]> l = new List<T[]>();
            GetPermutation(ref l, c[i], 0, n - 1);
            list.AddRange(l);
        }

        return list;
    }

    /// <summary>
    /// 求数组中n个元素的组合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">所求数组</param>
    /// <param name="n">元素个数</param>
    /// <returns>数组中n个元素的组合的范型</returns>
    public static List<T[]> GetCombination<T>(T[] t, int n)
    {
        if (t.Length < n)
        {
            return null;
        }
        int[] temp = new int[n];
        List<T[]> list = new List<T[]>();
        GetCombination(ref list, t, t.Length, n, temp, n);
        return list;
    }
}
