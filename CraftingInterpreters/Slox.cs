using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftingInterpreters
{
    internal class Slox
    {
        static bool hadError = false;
        static void Main(string[] args)
        {
            if(args.Length > 1) 
            {
                Console.WriteLine("Usage: slox [script]");
            } 
            else if(args.Length == 1) 
            {
                runFile(args[0]);
            }
            else 
            {
                runPrompt();
            }
        }
        static void runFile(string path)
        {
            //rodar arquivo de script
            StreamReader stream_reader = new StreamReader(path);
            string script = stream_reader.ReadToEnd().ToString();
            run(script);
            if (hadError)
            {
                //
            }
        }
        static void runPrompt()
        {
            string line;
            for (;;)
            {
                Console.Write(">> ");
                line = Console.ReadLine();
                if (line == null) { break; }
                run(line);
                hadError = false;
            }
        }
        static void run(string script)
        {
            Scanner scanner = new Scanner(script);
            List<Token> tolkens = scanner.scan_tokens();

            foreach(Token tolken in tolkens)
            {
                Console.WriteLine(tolken.to_string());
            }
        }
        public static void error(int line,  string message)
        {
            report(line, message);
        }
        private static void report(int line, string message, string where = "aa")
        {
            Console.WriteLine($"[line {line} ] Error {where} : {message}");
            hadError = true;
        }
    }
}
