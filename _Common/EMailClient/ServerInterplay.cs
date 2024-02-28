using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace EMailClient
{
    /// <summary>
    /// Класс взаимодействия с почтовым сервером по протоколу POP3
    /// </summary>
    public class POPServerInterplay : IDisposable
    {
        private string _Host;
        private int _Port;
        private string _UserName;
        private string _Password;
        private Socket _Socket;
        private bool _IsConnected;
        private MailResult _ServerResponse;
        private int _MessageCount = 0;
        private int _MessagesSize = 0;
        //private NetworkStream _NetStream = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="host">Адрес pop3-сервера</param>
        /// <param name="port">Порт</param>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        public POPServerInterplay(string host, int port, string userName, string password)
        {       
            _Host = host;
            _Password = password;
            if (port > 0)
                _Port = port;
            else
                _Port = 110;
            _UserName = userName;
            _IsConnected = false;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public POPServerInterplay() : this(null, -1, null, null) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="host">Адрес pop3-сервера</param>
        /// <param name="userName">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        public POPServerInterplay(string host, string userName, string password) : this(host, -1, userName, password) { }

        /// <summary>
        /// Соеденение с почтовым сервером, авторизации пользователя
        /// </summary>
        public void Connect()
        {
            string mess = "Соеденение с почтовым сервером" + Environment.NewLine;
            if (string.IsNullOrEmpty(_Host)
                || string.IsNullOrEmpty(_UserName)
                || string.IsNullOrEmpty(_Password)
                )
                throw new ApplicationException(mess + string.Format("Не определены все параметры подключения. Необходимо указать:{0}{1}{2}{3}{4}{5}"
                    , new object[] 
                    { string.IsNullOrEmpty(_Host) ? Environment.NewLine : string.Empty, String.IsNullOrEmpty(_Host) ? "Адрес pop3-сервера"  : string.Empty,
                      string.IsNullOrEmpty(_UserName) ? Environment.NewLine : string.Empty, String.IsNullOrEmpty(_UserName) ? "Логин пользователя"  : string.Empty,
                      string.IsNullOrEmpty(_Password) ? Environment.NewLine : string.Empty, String.IsNullOrEmpty(_Password) ? "Пароль пользователя"  : string.Empty
                    }));
            if (_Port <= 0) _Port = 110;
            IPHostEntry myIPHostEntry;
            myIPHostEntry = Dns.GetHostEntry(_Host);
            if (myIPHostEntry == null || myIPHostEntry.AddressList == null || myIPHostEntry.AddressList.Length <= 0)
            {
                throw new ApplicationException(mess + string.Format("Не удалось определить IP-адрес по хосту '{0}'.", _Host));
            }
            if (_IsConnected)
                Disconnect();
            //IPEndPoint myIPEndPoint = new IPEndPoint(myIPHostEntry.AddressList[0], _Port);
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _Socket.ReceiveBufferSize = 512;
            try
            {
                _Socket.Connect(myIPHostEntry.AddressList, _Port);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format(mess + "Не удалось соединиться с сервером '{0}' порт {1}{2}{3}", _Host, _Port, Environment.NewLine, ex.Message));
            }
            //_NetStream = new NetworkStream(_Socket);
            string s = string.Empty;
            s = ReadTopLine();
            _IsConnected = true;
            SendCommand(string.Format("USER {0}", _UserName));
            s = ReadTopLine();
            SendCommand(string.Format("PASS {0}", _Password));
            s = string.Empty;
            while (s == string.Empty)
                s = ReadTopLine();
            _ServerResponse = s;
            if (_ServerResponse.IsError)
            {
                Disconnect();
                throw new ApplicationException(string.Format(mess + "Ошибка авторизации пользователя. Соединение закрыто.{0}{1}", Environment.NewLine, s));
            }
            SendCommand("STAT");
            _ServerResponse = ReadTopLine();
            if (_ServerResponse.IsError)
            {
                Disconnect();
                throw new ApplicationException(string.Format(mess + "Ошибка чтения статистики почтового ящика. Соединение закрыто.{0}{1}", Environment.NewLine, _ServerResponse.ServerMessage));
            }
            _MessageCount = 0;
            _MessagesSize = 0;
            _ServerResponse.ParseStat(out _MessageCount, out _MessagesSize);
        }

        /// <summary>
        /// Завершение работы с почтовым сервером. 
        /// </summary>
        public void Disconnect()
        {
            if (_IsConnected)
            {
                SendCommand("QUIT");
                //ReadTopLine();
                //if (s.StartsWith("-ERR")) { throw new ApplicationException(s); }
                _Socket.Close();
                _Socket.Dispose();
                _Socket = null;
                _IsConnected = false;
                //_IsUserActivate = false;
                //if (ListMessages != null)
                //    ListMessages.Clear();
            }
        }

        #region Методы обмена командами с сервером

        /// <summary>
        /// Отправить команду серверу
        /// </summary>
        /// <param name="cmd">Команда протокола POP3</param>
        public void SendCommand(string cmd)
        {
            string mess = "Работа с почтовым сервером" + Environment.NewLine;
            if (!_IsConnected) throw new ApplicationException(mess + "Соединение с почтовым сервером не установлено. Откройте соединение методом Connect.");
            byte[] b = System.Text.Encoding.ASCII.GetBytes(String.Format("{0}\r\n", cmd));
            try
            {
                if (_Socket.Send(b, b.Length, SocketFlags.None) != b.Length)
                {
                    throw new ApplicationException(mess + string.Format("При отправке данных почтовому серверу '{0}' произошла ошибка...", _Host));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(mess + string.Format("При отправке данных почтовому серверу '{0}' произошла ошибка.{1}{2}", _Host, Environment.NewLine, ex.Message));
            }
        }

        /// <summary>
        /// Получить одну строку из ответа сервера
        /// </summary>
        /// <returns></returns>
        public string ReadTopLine()
        {
            if (!_IsConnected)
                return string.Empty;
            byte[] b = new byte[_Socket.ReceiveBufferSize];
            StringBuilder result = new StringBuilder(_Socket.ReceiveBufferSize);
            int s = 0;
            while (_Socket.Poll(1000000, SelectMode.SelectRead) && (s = _Socket.Receive(b, _Socket.ReceiveBufferSize, SocketFlags.None)) > 0)
            {
                result.Append(System.Text.Encoding.ASCII.GetChars(b, 0, s));
            }
            return result.ToString().TrimEnd("\r\n".ToCharArray());
            /**/
            /*
            System.Text.ASCIIEncoding oEncodedData = new System.Text.ASCIIEncoding();
            byte[] ServerBuffer = new Byte[_Socket.ReceiveBufferSize]; // 1024
            NetworkStream NetStream = new NetworkStream(_Socket, System.IO.FileAccess.Read);
            //NetStream.Flush();
            int count = 0;

            //считываем данные из сетевого потока сервера, что бы потом их декодировать
            while (true)
            {
                byte[] buff = new Byte[2];
                int bytes = NetStream.Read(buff, 0, 1);
                if (bytes == 1)
                {
                    ServerBuffer[count] = buff[0];
                    count++;
                    if (buff[0] == '\n')
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            //возвращаем декодированный ответ сервера
            string ReturnValue = oEncodedData.GetString(ServerBuffer, 0, count);
            return ReturnValue;
            /**/
        }

        /// <summary>
        /// Получить все строки из ответа сервера
        /// </summary>
        /// <returns></returns>
        public string ReadAllLines()
        {
            if (!_IsConnected)
                return string.Empty;

            byte[] b = new byte[_Socket.ReceiveBufferSize];
            StringBuilder result = new StringBuilder(_Socket.ReceiveBufferSize);
            //NetworkStream NetStream = new NetworkStream(_Socket, System.IO.FileAccess.Read);
            int s = 0;
            string ss;
            //_Socket.Poll(-1, SelectMode.SelectRead) &&
            while (_Socket.Poll(1000000, SelectMode.SelectRead) && ((s = _Socket.Receive(b, _Socket.ReceiveBufferSize, SocketFlags.None)) > 0)) //1000000
            {
                result.Append(System.Text.Encoding.ASCII.GetChars(b, 0, s));
            }
            ss = System.Text.Encoding.ASCII.GetString(b, 0, s);
            return result.ToString();
            /**/
            /*
            string sResult = string.Empty;
            StringBuilder result = new StringBuilder(_Socket.ReceiveBufferSize);
            while (true)
            {

                sResult = ReadTopLine();
                if (!string.IsNullOrEmpty(sResult))
                    result.Append(sResult);
                if (sResult == ".\r\n")
                {
                    break;
                }
            }
            return result.ToString();
            /**/
        }
        #endregion

        #region Работа с письмами
        /// <summary>
        /// Формирует список писем
        /// </summary>
        public void GetListMail()
        {
            SendCommand("LIST");
        }
        /// <summary>
        /// Прочитать заголовок письма
        /// </summary>
        /// <param name="index">Индекс письма</param>
        /// <returns>Текст заголовка письма в MIME формате</returns>
        public string GetMailHeader(int index)
        {
            return GetMailOrMailHeader(index, true);
        }
        /// <summary>
        /// Прочитать письмо
        /// </summary>
        /// <param name="index">Индекс письма</param>
        /// <returns>Текст всего письма в MIME формате</returns>
        public string GetMail(int index)
        {
            return GetMailOrMailHeader(index, false);
        }
        /// <summary>
        /// Прочитать письмо или только его заголовок
        /// </summary>
        /// <param name="index">Индекс письма</param>
        /// <param name="OnlyHeader">true - только заголовок, false - письмо целиком</param>
        /// <returns>Текст заголовка письма или всего письма MIME формате</returns>
        private string GetMailOrMailHeader(int index, bool OnlyHeader)
        {
            if (this.MessageCount == 0)
                throw new ApplicationException("Нет писем");
            if (index > this.MessageCount || index <= 0)
            {
                throw new ApplicationException(String.Format("Передан индекс письма {0}{1}Индекс должен быть от 1 и не больше {2}", index, Environment.NewLine, MessageCount));
            }
            if (OnlyHeader)
                SendCommand(String.Format("TOP {0} 0", index));
            else
                SendCommand(String.Format("RETR {0}", index));
            _ServerResponse = ReadAllLines();
            if (_ServerResponse.IsError)
            {
                throw new ApplicationException(String.Format("Ошибка чтения письма № {0}{1}{2}", index, Environment.NewLine, _ServerResponse.ServerMessage));
            }
            return _ServerResponse.Body;
        }
        /// <summary>
        /// Удаление письма
        /// </summary>
        /// <param name="index">Индекс письма</param>
        public void Delete(int index)
        {
            if (this.MessageCount == 0)
                return;
            if (index > this.MessageCount || index <= 0)
            {
                throw new ApplicationException(String.Format("Передан индекс письма {0}{1}Индекс должен быть от 1 и не больше {3}", index, Environment.NewLine, MessageCount));
            }
            SendCommand(String.Format("DELE {0}", index));
            _ServerResponse = ReadTopLine();
            if (_ServerResponse.IsError)
            {
                throw new ApplicationException(String.Format("Ошибка удаления письма № {0}{1}{2}", index, Environment.NewLine, _ServerResponse.ServerMessage));
            }
            _MessageCount--;
        }
        #endregion

        #region public Properties

        /// <summary>
        /// Хост
        /// </summary>
        public string Host
        {
            get { return _Host; }
            set
            {
                if (_Host != value)
                {
                    _Host = value;
                    Disconnect();
                }
            }
        }

        /// <summary>
        /// Пользователь
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    Disconnect();
                }
            }
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    Disconnect();
                }
            }
        }

        /// <summary>
        /// Порт
        /// </summary>
        public int Port
        {
            get { return _Port; }
            set
            {
                if (_Port != value)
                {
                    _Port = value;
                    Disconnect();
                }
            }
        }

        /// <summary>
        /// Количество писем
        /// </summary>
        public int MessageCount
        {
            get { return _MessageCount; }
        }

        /// <summary>
        /// Размер писем
        /// </summary>
        public int MessagesSize
        {
            get { return _MessagesSize; }
        }

        /// <summary>
        /// Соединение установлено
        /// </summary>
        public bool IsConnected { get { return _IsConnected; } }

        #endregion

        #region интерфейс IDisposable

        /// <summary>
        /// Очистка.
        /// </summary>
        public virtual void Dispose()
        {
            Disconnect();            
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
