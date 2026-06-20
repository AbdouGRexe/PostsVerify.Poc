using System.Collections.ObjectModel;
using IARRv3.Api.Domain.Entities;

namespace IARRv3.Api.Domain;

internal static class StatisticalOperations
{
    internal static double GetStderr(List<double> samples)
    {
        var mean = samples.Average();
        var sum = samples.Sum(d => Math.Pow(d - mean, 2));
        return samples.Count == 0 ? double.NaN :  Math.Sqrt(sum / samples.Count);
    }

    internal static double GetSetsCombinedAverage((StatSet set1, StatSet set2) sets, StatSet intersectStatSet = null)
    {
        intersectStatSet ??= new StatSet(0, 1, 1);
        var sampleSize = (sets.set1.SampleSize + sets.set2.SampleSize - intersectStatSet.SampleSize);
        return sampleSize == 0 ? double.NaN : (sets.set1.Average * sets.set1.SampleSize + sets.set2.Average * sets.set2.SampleSize -
                intersectStatSet.Average * intersectStatSet.SampleSize) /
               sampleSize;
    }

    internal static double GetSetsCombinedStderr((StatSet set1, StatSet set2) sets, StatSet intersectStatSet = null)
    {
        intersectStatSet ??= new StatSet(0, 1, 1);
        var n = sets.set1.SampleSize;
        var m = sets.set2.SampleSize;
        var k = intersectStatSet.SampleSize;
        var z = n + m - k;
        var stderrX = sets.set1.Stderr;
        var stderrY = sets.set2.Stderr;
        var stderrW = intersectStatSet.Stderr;
        var meanX = sets.set1.Average;
        var meanY = sets.set2.Average;
        var meanW = intersectStatSet.Average;
        var meanZ = (meanX * n + meanY * m - meanW * k) / z;
        var variance = z == 0 ? double.NaN :  (Math.Pow(stderrX, 2) * n + Math.Pow(stderrY, 2) * m + Math.Pow(stderrW, 2) * k +
            Math.Pow(meanX, 2) * n +
            Math.Pow(meanY, 2) * m + Math.Pow(meanW, 2) * k - z * Math.Pow(meanZ, 2)) / z;
        return Math.Sqrt(Math.Max(0, variance));;
    }
}

internal record StatSet(int SampleSize, double Average = 0, double Stderr = 0);