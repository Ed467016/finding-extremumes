using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace FindingExtremumes.Compiler
{
    /// <summary>
    /// Used to compile string expression to function pointer.
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// Shortcuts for formula expression inputs.
        /// </summary>
        private static readonly Dictionary<string, string> _keys = new Dictionary<string, string>()
        {
            { "pow", "Math.Pow" },
            { "sqrt", "Math.Sqrt" },
            { "e", Math.E.ToString() },
            { "pi", Math.PI.ToString() }
        };

        private readonly CSharpCodeProvider m_provider = new CSharpCodeProvider();
        private readonly CompilerParameters m_parameters = new CompilerParameters();

        public Compiler()
        {
            m_parameters.GenerateInMemory = true;
            m_parameters.ReferencedAssemblies.Add("System.dll");
        }

        /// <summary>
        /// Compiles string and gets assembly compiled.
        /// </summary>
        /// <param name="code">Code to compile.</param>
        /// <returns>Result of compilation.</returns>
        public Task<CompilerResults> Compile(string code)
        {
            return Task.Run(() =>
            {
                CompilerResults results = m_provider.CompileAssemblyFromSource(m_parameters, GetCode(code));
                return results;
            });
        }

        /// <summary>
        /// Gets function pointer from compiled assembly.
        /// </summary>
        /// <param name="cr">Compiled assembly.</param>
        /// <returns>Function pointer.</returns>
        public Func<double, double> GetLambda(CompilerResults cr)
        {
            var asm = cr.CompiledAssembly.GetType("Dynamic.DynamicCode");
            var method = asm.GetMethod("DynamicMethod", BindingFlags.Static | BindingFlags.Public);
            var res = method.Invoke(null, null) as Func<double, double>;

            return res;
        }

        /// <summary>
        /// Used to replace shortcuts with valid C# math functions.
        /// </summary>
        private static string[] GetCode(string code)
        {
            foreach (var key in _keys)
                code = code.Replace(key.Key, key.Value);

            return new string[]
            {
                @"using System;
 
                namespace Dynamic
                {
                    public static class DynamicCode
                    {
                        public static Func<double, double> DynamicMethod()
                        {
                            return x => " + code + @";
                        }
                    }
                }"
            };
        }
    }
}
