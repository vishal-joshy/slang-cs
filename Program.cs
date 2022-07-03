using System.Collections;
using SLANG;

namespace Program
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
        RUNTIME_CONTEXT rtx = new RUNTIME_CONTEXT(module);
        SYMBOL symbol = module.Execute(rtx,null);
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
      if (args.Length == 0)
      {
        Console.WriteLine("Usage: dotnet run <filename>");
        return;
      }
      string code;
      string filename = args[0];
      try
      {
        code = ReadFile(filename);
      }
      catch
      {
        throw new Exception("Could not read file " + filename);
      }
      string mode = args.Length > 1 ? args[1] : "i";
      RunCode(code, mode);
    }
  }
}