using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomMethod 
{
    static public int RandomWeight<T>(List<T> weightRates) where T : struct
    {
        int cumulativeSum = 0;
        List<int> cumulativeRates = new();
        foreach (var rate in weightRates)
        {
            cumulativeSum += Convert.ToInt32(rate);
            cumulativeRates.Add(cumulativeSum); // 6, 9, 10
        }
        int randomValue = UnityEngine.Random.Range(1, cumulativeSum + 1);   // suppose random =  9
        // 1-10 : 1-6: 0, 7-9: 1, 10: 2

        for (int i = 0; i < cumulativeRates.Count; i++)
        {
            if (randomValue <= cumulativeRates[i])  // 9 <= 6: false, 9 <= 9: true, 10 <= 10: true
            {
                return i; // return the index of the selected rate => 9 => index 1
            }
        }
        return -1;
    }
}
