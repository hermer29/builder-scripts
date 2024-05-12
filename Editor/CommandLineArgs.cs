using System;
using JetBrains.Annotations;

namespace BuilderScript.Editor
{
    public class CustomCommandLineArgs
    {
        private static readonly string[] _args;

        static CustomCommandLineArgs()
        {
            _args = Environment.GetCommandLineArgs();
        }
        
        [CanBeNull]
        public static string GetArgumentValueEnsureNotNull(string key)
        {
            var argIndex = Array.IndexOf(_args, $"-{key}");
            if (argIndex == -1)
                throw new InvalidOperationException($"Command line argument (named {key}) not passed");
            return _args[argIndex + 1];
        }
    }
}