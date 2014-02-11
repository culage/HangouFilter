using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace HangouFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            // �ݒ�t�@�C���ǂݍ���
            bool logOutput = false;
            List<string> filterList = new List<string>();
            StreamReader sr = new StreamReader(@"convert.filter.txt");
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line == "# LogOutput=1") { logOutput = true;  continue; }
                if (line == "") { continue; }
                if (line.StartsWith("#")) { continue; }
                filterList.Add(line);
            }
            sr.Close();

            // �������H
            string allArgs = "";
            foreach (string arg in args) { allArgs += " " + arg; }

            foreach (string filter in filterList)
            {
                Match m = Regex.Match(filter, "([^\t]+)\t+([^\t]+)");
                string pattern = m.Groups[1].Value;
                string replacement = m.Groups[2].Value;
                allArgs = Regex.Replace(allArgs, pattern, replacement);
            }

            // ���O�o��
            if (logOutput)
            {
                StreamWriter sw = new StreamWriter(@".\out.txt", true);
                sw.WriteLine(allArgs);
                sw.Close();
            }

            // �{����convert.exe���Ăяo��
            ProcessStartInfo s = new ProcessStartInfo();
            s.FileName = @".\convert.org.exe";
            s.Arguments = allArgs;
            s.CreateNoWindow = true;
            s.UseShellExecute = false;
            Process p = Process.Start(s);
            p.WaitForExit();
        }
    }
}
