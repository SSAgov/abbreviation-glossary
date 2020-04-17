using PrimitiveExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AbbreviationGlossary
{
    public class TermConversionResult
    {
        private string _input;
        private string _output;
        private Stopwatch sw;

        public TermConversionResult()
        {
            NodesFound = new HashSet<string>();
            NodesNotFound = new HashSet<string>();
            sw = new Stopwatch();
            _input = "";
            _output = "";
        }
        public TimeSpan ConversionTime { get; internal set; }

        public string Input
        {
            get => _input; internal set
            {
                _input = value;
                sw.Start();
            }
        }
        public string Output
        {
            get => _output;
            internal set
            {
                _output = value;
                sw.Stop();
                ConversionTime = sw.Elapsed;
            }
        }
        public TermConverterConfig Configuration { get; internal set; }
        public HashSet<string> NodesFound { get; set; }
        public HashSet<string> NodesNotFound { get; set; }
        public override string ToString()
        {
            return Output;
        }
    }
}
