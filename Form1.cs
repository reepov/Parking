using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parking
{
    public partial class Form1 : Form
    {
        public int _start; // Начало работы
        public int _end; // Конец работы
        public int _row; // Ряды парковки
        public int _column; // Мест в ряде
        public int _streamRate; // Машинопоток
        List<Button>[] places; // Кнопки - парковочные места
        public Form1(int row, int column, int streamRate, int start, int end)
        {
            InitializeComponent();
            _start = start;
            _end = end;
            _row = row;
            _column = column;
            _streamRate = streamRate;
            // Настройка визуального отображения парковки
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = row + row / 2;
            tableLayoutPanel1.ColumnCount = column;
            for (int x = 0; x < tableLayoutPanel1.RowCount; x++) tableLayoutPanel1.RowStyles.Add(new RowStyle() { Height = 20, SizeType = SizeType.Percent });
            for (int x = 0; x < tableLayoutPanel1.ColumnCount; x++) tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle() { Width = 20, SizeType = SizeType.Percent });
            Button button = new Button() { AutoSize = true };
            places = new List<Button>[row];
            int k = 0;
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                if ((i - 1) % 3 != 0) places[k] = new List<Button>();
                for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    button.Width = tableLayoutPanel1.GetColumnWidths()[j];
                    button.Height = tableLayoutPanel1.GetRowHeights()[i];
                    button.Enabled = false;
                    button.BackColor = Color.LightGreen;
                    if ((i - 1) % 3 == 0) button.BackColor = Color.Gray;
                    else places[k].Add(button);
                    tableLayoutPanel1.Controls.Add(button, j, i);
                    button = new Button();
                }
                if ((i - 1) % 3 != 0) k++;
            }
            label1.Text = $"Всего мест\n{(row * column)}";
            label2.Text = $"Свободных мест\n{(row * column)}";
            label3.Text = $"Занятых мест\n0";
            label4.Text = $"График работы\nС {start}:00 до {end}:00";
            label5.Text = $"Время\n{start}:00";
            
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Start.StartForm.Show(); // Отображение скрытой формы задания параметров симуляции
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                Random rand = new Random();
                int wholeCarNow = 0; // Количество машин на парковке
                int[,] cars = new int[_row, _column]; // Матрица занятости парковки
                for (int i = _start + 1; i < _end + 1; i++)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(2000)); // Задание интервала в 2 секунды для обновления данных о парковке
                    wholeCarNow = 0;
                    for (int j = 0; j < _row; j++)
                    {
                        for (int k = 0; k < _column; k++)
                        {
                            cars[j,k] = rand.NextDouble() > 0.08 * (_streamRate + 1) ? 0 : 1; // Генерация машин на парковке
                            wholeCarNow += cars[j, k];
                            if(cars[j, k] == 1) places[j][k].BackColor = Color.Red;
                            else places[j][k].BackColor = Color.LightGreen;
                        }
                    }
                    // Вызов методов изменения отображения во внешних потоках, т.к. в рабочем потоке изменения не будут приняты
                    label2.Invoke((MethodInvoker)delegate {

                        if(_row * _column < wholeCarNow) label2.Text = $"Свободных мест\n0";
                        else label2.Text = $"Свободных мест\n{(_row * _column - wholeCarNow)}";
                    });
                    label3.Invoke((MethodInvoker)delegate {

                        if(wholeCarNow < 200) label3.Text = $"Занятых мест\n{wholeCarNow}";
                        else label3.Text = $"Занятых мест\n200";
                    });
                    label5.Invoke((MethodInvoker)delegate {

                        label5.Text = $"Время\n{i}:00";
                    });

                }
            }
            catch
            {

            }
        }
    }
}
