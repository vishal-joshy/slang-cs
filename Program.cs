using System.Collections;
using SLANG;
using System.Text.Json;

namespace slang
{
  class Program
  {
    static string ReadFile(string fileName)
    {
      string result = "";
      StreamReader sr = new StreamReader(fileName);
      result = sr.ReadToEnd();
      sr.Close();
      return result;
    }

    static void TestFile(string filename)
    {
      string code;
      try
      {
        code = ReadFile(filename);
      }
      catch
      {
        throw new Exception("Could not read file " + filename);
      }

      Console.WriteLine("code\n-------- \n" + code);

      RDParser parser = new RDParser(code);
      CompilationContext ctx = new CompilationContext();
      RuntimeContext rtx = new RuntimeContext();
      ArrayList stmts = parser.Parse(ctx);

      Console.WriteLine("result\n-------");
      foreach (Object obj in stmts)
      {
        Stmt s = obj as Stmt;
        s.Execute(rtx);
      }
    }

    static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        Console.WriteLine("Usage: dotnet run <filename>");
        return;
      }
      TestFile(args[0]);
      Console.Read();
    }
  }
}