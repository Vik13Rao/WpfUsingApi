using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfUsingApi.Models;

namespace WpfUsingApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        public MainWindow()
        {
            client.BaseAddress = new Uri("https://localhost:7118/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
            InitializeComponent();
        }

        private void btnLoadStudents_Click(object sender, RoutedEventArgs e)
        {
            this.GetStudents();
        }

        private async void GetStudents()
        {
           

            var response = await client.GetStringAsync("students");
            var students = JsonConvert.DeserializeObject<List<Student>>(response);
            dgStudent.DataContext = students;
        }

        private async void SaveStudent(Student student)
        {
            await client.PostAsJsonAsync("students", student);
        }

        private async void UpdateStudent(Student student)
        {
            await client.PutAsJsonAsync("students/" +student.Id, student);
        }

        private async void DeleteStudent(int Id)
        {
            await client.DeleteAsync("students/"+ Id);
        }

        private void btnSaveStudent_Click(object sender, RoutedEventArgs e)
        {
            var student = new Student()
            {
                Id = Convert.ToInt32(txtId.Text),
                Name = txtName.Text,
                Roll = Convert.ToInt32(txtRoll.Text)
            };

            if(student.Id == 0)
            {
                this.SaveStudent(student);
            }

            else
            {
                this.UpdateStudent(student);
            }

            txtId.Text = 0.ToString();
            txtName.Text = "";
            txtRoll.Text = 0.ToString();
        }

        void btnEditStudent(object sender, RoutedEventArgs e)
        {
            Student student = ((FrameworkElement)sender).DataContext as Student;
            txtId.Text = student.Id.ToString();
            txtName.Text = student.Name;
            txtRoll.Text = student.Roll.ToString();
        }

        void btnDeleteStudent(object sender, RoutedEventArgs e)
        {
            Student student = ((FrameworkElement)sender).DataContext as Student;
            this.DeleteStudent(student.Id);
        }
    }
}
