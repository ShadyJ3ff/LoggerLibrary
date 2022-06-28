using System;
using System.Data.OleDb;
using System.IO;
using System.Reflection;



namespace Logger
{
    abstract public class Log //Пустой класс для наследования 
    {
        virtual public void Info(string msg)
        {
        }
        virtual public void Warn(string msg)
        {
        }
        virtual public void Debug(string msg)
        {
        }
        virtual public void Error(string msg)
        {
        }
    }
    public class file : Log//Класс для записи логов в текстовый файл
    {
        int i = 1;
        public void CreateLog()//класс для создания файла лога  для каждого отдельного запуска программы
        {
            while (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                    "\\" + DateTime.Now.ToShortDateString() + "-" + i + ".log.txt"))//если файл существует то прибавляется счетчик
            { i++; }
            using (File.Create(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +//Используем using при создании файла для автоматического закрытия потока
                "\\" + DateTime.Now.ToShortDateString() + "-" + i + ".log.txt"))
            { }
        }
        public override void Info(string msg)//Функция для записи логов типа Info в файл
        {
            using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
                "\\" + DateTime.Now.ToShortDateString() + "-"+i+".log.txt", true)) //открывается либо создается файл для хранения логов в месте хранения выполняемого кода
            {
                sw.WriteLine(String.Format("[INFO]{0,-23} {1}", DateTime.Now.ToString() + ":", msg));//записываем логи в файл
            }
        }
        public override void Warn(string msg)//Функция для записи логов типа Warn в файл
        {
            using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
                "\\" + DateTime.Now.ToShortDateString() + "-" + i + ".log.txt", true))
            {
                sw.WriteLine(String.Format("[WARN]{0,-23} {1}", DateTime.Now.ToString() + ":", msg));
            }
        }
        public override void Debug(string msg)//Функция для записи логов типа Debug в файл
        {
            using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
                "\\" + DateTime.Now.ToShortDateString() + "-" + i + ".log.txt", true))
            {
                sw.WriteLine(String.Format("[DEBUG]{0,-23} {1}", DateTime.Now.ToString() + ":", msg));
            }
        }
        public override void Error(string msg)//Функция для записи логов типа Error в файл
        {
            using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
                "\\" + DateTime.Now.ToShortDateString() + "-" + i + ".log.txt", true))
            {
                sw.WriteLine(String.Format("[ERROR]{0,-23} {1}", DateTime.Now.ToString() + ":", msg));
            }
        }
    }
    public class DB:Log//Класс для записи логов в базу данных Access
    {
        public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Logger.mdb;";//Строка с данными для подключения к бд
        private OleDbConnection myConnection;//поле - ссылка на объект класса OleDbConnection для соединения с БД

        public override void Info(string msg)//Функция для записи логов типа Info в бд
        {
            myConnection = new OleDbConnection(connectString);//Создаем объект класса OleDbConnection
            myConnection.Open();//Открываем подключение
            string query = $"INSERT INTO Логгер (Дата_Время, Лог) VALUES ('{DateTime.Now.ToString()}', '[INFO]{msg}')";//текст запроса
            OleDbCommand command = new OleDbCommand(query, myConnection);//Созадем объект для выполнения запроса
            command.ExecuteNonQuery();//Выполняем запрос 
            myConnection.Close();//Закрываем подключение
        }
        public override void Debug(string msg)//Функция для записи логов типа Debug в бд
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            string query = $"INSERT INTO Логгер (Дата_Время, Лог) VALUES ('{DateTime.Now.ToString()}', '[DEBUG]{msg}')";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            command.ExecuteNonQuery();
            myConnection.Close();
        }
        public override void Error(string msg)//Функция для записи логов типа Error в бд
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            string query = $"INSERT INTO Логгер (Дата_Время, Лог) VALUES ('{DateTime.Now.ToString()}', '[ERROR]{msg}')";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            command.ExecuteNonQuery();
            myConnection.Close();
        }
        public override void Warn(string msg)//Функция для записи логов типа Warn в бд
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            string query = $"INSERT INTO Логгер (Дата_Время, Лог) VALUES ('{DateTime.Now.ToString()}', '[WARN]{msg}')";
            OleDbCommand command = new OleDbCommand(query, myConnection);
            command.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}
