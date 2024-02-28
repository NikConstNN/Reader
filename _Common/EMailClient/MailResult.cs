using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EMailClient
{
    /// <summary>
    /// Класс для хранения сообщения от почтового сервера
    /// </summary>
    public class MailResult
    {
        public string Source { get; set; }
        public bool IsError { get; set; }
        public string ServerMessage { get; set; }
        public string Body { get; set; }

        public MailResult() { }
        public MailResult(string source)
        {
            this.Source = source;
            // обрабатываем ответ
            this.IsError = source.StartsWith("-ERR"); // ошибка, или нет
            // получаем отдельно сообщение о результате выполнения команды
            Regex myReg = new Regex(@"(\+OK|\-ERR)\s{1}(?<msg>.*)?", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            if (myReg.IsMatch(source))
            {
                this.ServerMessage = myReg.Match(source).Groups["msg"].Value;
            }
            else
            {
                this.ServerMessage = source;
            }
            // если есть, получаем тело сообщения, удаляя сообщение сервера и лишние маркеры протокола
            if (source.IndexOf("\r\n") != -1)
            {
                this.Body = source.Substring(source.IndexOf("\r\n") + 2, source.Length - source.IndexOf("\r\n") - 2);
                if (this.Body.IndexOf("\r\n\r\n.\r\n") != -1)
                {
                    this.Body = this.Body.Substring(0, this.Body.IndexOf("\r\n\r\n.\r\n"));
                }
            }
            // --
        }
        public void ParseStat(out int messagesCount, out int messagesSize)
        {
            Regex myReg = new Regex(@"(?<count>\d+)\s+(?<size>\d+)");
            Match m = myReg.Match(this.Source);
            int.TryParse(m.Groups["count"].Value, out messagesCount);
            int.TryParse(m.Groups["size"].Value, out messagesSize);
        }

        public static implicit operator MailResult(string value)
        {
            return new MailResult(value);
        }
    }
}
