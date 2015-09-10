using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace CodeDOMExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string value = "My Sample Value";
            int indexOfp = value.IndexOf('p'); // returns 6
            int lastIndexOfm = value.LastIndexOf('m'); // returns 5

            Console.WriteLine(indexOfp);
            Console.WriteLine(lastIndexOfm);

            // Expression tree
            BlockExpression blockExpr = Expression.Block( Expression.Call(null,
              typeof(Console).GetMethod("Write", new Type[] { typeof(String) }),
              Expression.Constant("Hello ")
             ),
              Expression.Call(
                  null,
                  typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) }),
                  Expression.Constant("World!")
                  )
             );

            Expression.Lambda<Action>(blockExpr).Compile()();

            Console.Read();
        }

        private static void NewMethod()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace myNamespace = new CodeNamespace("MyNamespace");
            myNamespace.Imports.Add(new CodeNamespaceImport("System"));
            CodeTypeDeclaration myClass = new CodeTypeDeclaration("MyClass");
            CodeEntryPointMethod start = new CodeEntryPointMethod();
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("Console"),
                "WriteLine", new CodePrimitiveExpression("Hello World!"));

            compileUnit.Namespaces.Add(myNamespace);
            myNamespace.Types.Add(myClass);
            myClass.Members.Add(start);
            start.Statements.Add(cs1);

            CSharpCodeProvider provider = new CSharpCodeProvider();

            using (StreamWriter sw = new StreamWriter("HelloWorld.cs", false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");
                provider.GenerateCodeFromCompileUnit(compileUnit, tw,
                     new CodeGeneratorOptions());
                tw.Close();
            }
        }
    }
}
