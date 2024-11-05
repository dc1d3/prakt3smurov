using System;
using System.IO;
using System.Windows;
using Lib_1;
using Microsoft.Win32;

namespace MatrixApp
{

    public partial class MainWindow : Window
    {
        private int[,] matrix;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("смуров богдан\nпрактическая номер 2\n Дана матрица размера M × N, найти произведение элементов каждого столбца.");
        }

        private void GenerateMatrix_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RowsTextBox.Text, out int rows) &&
                int.TryParse(ColsTextBox.Text, out int cols) &&
                int.TryParse(MinValueTextBox.Text, out int minValue) &&
                int.TryParse(MaxValueTextBox.Text, out int maxValue) &&
                rows > 0 && cols > 0)
            {
                matrix = MatrixOperations.GenerateMatrix(rows, cols, minValue, maxValue);
                OutputTextBox.Text = "матрица сгенерирована.";
            }
            else
            {
                MessageBox.Show("некорректные значения");
            }
        }
        private void SaveMatrixToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Matrix";
            saveFileDialog.DefaultExt = ".txt"; 
            saveFileDialog.Filter = "Text documents (.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        int rows = matrix.GetLength(0);
                        int cols = matrix.GetLength(1);

                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                writer.Write(matrix[i, j]);
                                if (j < cols - 1)
                                {
                                    writer.Write("\t"); 
                                }
                            }
                            writer.WriteLine();
                        }
                    }
                    MessageBox.Show("Матрица успешно сохранена в файл.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
                }
            }
        }
        private void OpenMatrixFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.Filter = "Text documents (.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string[] lines = File.ReadAllLines(openFileDialog.FileName);
                    int rowCount = lines.Length;
                    int colCount = lines[0].Split('\t').Length; 

                    matrix = new int[rowCount, colCount];


                    for (int i = 0; i < rowCount; i++)
                    {
                        string[] values = lines[i].Split('\t');
                        for (int j = 0; j < colCount; j++)
                        {
                            matrix[i, j] = int.Parse(values[j]); 
                        }
                    }

                    OutputTextBox.Text = "Матрица успешно загружена.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
                        }
            }
        }
                            private void CalculateProducts_Click(object sender, RoutedEventArgs e)
        {
            if (matrix != null)
            {
                int[] products = MatrixOperations.CalculateColumnProducts(matrix);
                OutputTextBox.Text = "произведения столбцов: " + string.Join(", ", products);
            }
            else
            {
                MessageBox.Show("сначала сгенерируйте матрицу");
            }
        }
    }

}