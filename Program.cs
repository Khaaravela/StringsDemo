using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Text;

namespace StringsDemo
{
    class Program
    {
        private const string strVal = "this is one example of string manipulaton";
        private const string replacestr = "string";
        static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<StringBenchmarks>();
            //Console.WriteLine(string.Create(strVal.Length, strVal, (sp, val) => {
            //    val.AsSpan().TryCopyTo(sp);
            //    sp[val.IndexOf(replacestr)..(val.IndexOf(replacestr)+replacestr.Length)].Fill('#');
            //}));
        }
    }

    [MemoryDiagnoser] // required as to display how much memory has been utilised, in the summary.  
    public class StringBenchmarks
    {
        private const string strVal = "this is one example of string manipulaton";
        private const string replacestr = "string";

        [Benchmark]
        public string NativeCase()
        {
            var newstr = string.Empty;
            for (var x = 0; x < replacestr.Length; x++)
            {
                newstr += '#';
            }
            var begin = strVal.Replace(replacestr, newstr);
            return begin;
        }

        [Benchmark]
        public string StringBuilderCase()
        {
            var newstr = new StringBuilder();
            newstr.Append('#', replacestr.Length);
            var begin = strVal.Replace(replacestr, newstr.ToString());
            return begin;
        }

        [Benchmark]
        public string StringCreatorCase()
        {
            // basically breaks the term strings are immutable!
            return string.Create(strVal.Length, strVal, (sp, val) => {
                val.AsSpan().TryCopyTo(sp);
                sp[val.IndexOf(replacestr)..(val.IndexOf(replacestr) + replacestr.Length)].Fill('#');
            });
        }
    }
}
