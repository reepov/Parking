using System;
using System.Windows.Forms;

namespace Parking
{
    public partial class Start : Form
    {
        public static Start _startForm;
        public static Start StartForm { get => _startForm ?? new Start(); }
        public Start()
        {
            InitializeComponent();
        }

        private void Start_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Считывание введенных данных
            bool row = int.TryParse(textBox1.Text, out int rows);
            bool column = int.TryParse(textBox2.Text, out int columns);
            bool stream = int.TryParse(textBox3.Text, out int streams);
            bool start = int.TryParse(textBox4.Text, out int starts);
            bool end = int.TryParse(textBox5.Text, out int ends);
            //Обработка корректности введенных данных
            if (row && (rows > 10 || rows < 1)) MessageBox.Show("Слишком много или мало рядов парковки. Введите одно число от 1 до 10", "Ошибка");
            else if (!row) MessageBox.Show("Некорректное количество рядов парковки. Введите число от 1 до 10", "Ошибка");
            else if (column && (columns > 20 || columns < 1)) MessageBox.Show("Слишком много или мало мест в ряду парковки. Введите одно число от 1 до 20", "Ошибка");
            else if (!column) MessageBox.Show("Некорректное количество мест в ряду парковки. Введите число от 1 до 20", "Ошибка");
            else if (stream && (streams > 10 || streams < 0)) MessageBox.Show("Оценка машинопотока вне границ оценивания. Введите одно число от 0 до 10", "Ошибка");
            else if (!stream) MessageBox.Show("Некорректная оценка машинопотока. Введите число от 0 до 10", "Ошибка");
            else if (start && (starts > 24 || starts < 0)) MessageBox.Show("Время начала работы вне границ часов дня. Введите число от 0 до 24", "Ошибка");
            else if (!start) MessageBox.Show("Некорректное время начала работы. Введите число от 0 до 24", "Ошибка");
            else if (end && (ends > 24 || ends < 0)) MessageBox.Show("Время конца работы вне границ часов дня. Введите число от 0 до 24", "Ошибка");
            else if (!end) MessageBox.Show("Некорректное время конца работы. Введите число от 0 до 24", "Ошибка");
            else if (starts > ends) MessageBox.Show("Время начала работы парковки не может быть после ее закрытия. Введите начало работы, меньшее, чем конец работы", "Ошибка");
            else if (starts == ends) MessageBox.Show("Время начала работы парковки не может равно времени закрытия, так как в таком случае рабочий день длится 0 часов. Введите начало работы, меньшее, чем конец работы", "Ошибка");
            else
            {
                //Передача данных в форму симуляции работы парковки
                Form1 generate = new Form1(rows, columns, streams, starts, ends);
                generate.Show();
                _startForm = this;
                _startForm.Hide();
            } 
        }
    }
}
