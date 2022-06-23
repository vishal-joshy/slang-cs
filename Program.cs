using System.Collections;
using SLANG;

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

    static void RunCode(string code, string mode)
    {
      Console.WriteLine("code\n-------- \n" + code);

      RDParser parser = new RDParser(code);
      TModule module = parser.DoParse();
      if(mode == "i"){
        RuntimeContext rtx = new RuntimeContext(module);
        Symbol symbol = module.Execute(rtx);
      } else if (mode == "c")
      {
        if (module.CreateExecutable("test.exe"))
        {
          Console.WriteLine("Executable Created");
          return;
        }
      }

    }

    static void Main(string[] args)
    {
      // if (args.Length == 0)
      // {
      //   Console.WriteLine("Usage: dotnet run <filename>");
      //   return;
      // }
      string code;
      string filename = "test.sl";
      try
      {
        code = ReadFile(filename);
      }
      catch
      {
        throw new Exception("Could not read file " + filename);
      }
      string mode = args.Length > 0 ? args[0] : "i";
      RunCode(code, mode);
      Console.Read();
    }
  }
}