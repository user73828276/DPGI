using Lab4.Task1.DAL;
using Lab4.Task1.Model;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab4.Task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BookRepository _bookRepository = new();
        private List<Book> books;

        public MainWindow()
        {
            InitializeComponent();
            Loadbooks();
        }

        private void Loadbooks()
        {
            books = _bookRepository.GetAllBooks();
            list.ItemsSource = books;
            ResetFields();
        }

        private void ResetFields()
        {
            ISBNTextBox.Text = "";
            TitleTextBox.Text = "";
            AuthorsTextBox.Text = "";
            PublisherTextBox.Text = "";
            PublicationYearTextBox.Text = "";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(PublicationYearTextBox.Text, out int publicationYear))
            {
                Book newbook = new()
                {
                    ISBN = ISBNTextBox.Text,
                    Title = TitleTextBox.Text,
                    Authors = AuthorsTextBox.Text,
                    Publisher = PublisherTextBox.Text,
                    PublicationYear = publicationYear
                };
                ResetFields();
                _bookRepository.CreateBook(newbook);
                Loadbooks();
            }
            else
            {
                MessageBox.Show("Перевірте валідність даних");
            }
        }      

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            Loadbooks();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(IDTextBox.Text, out int id) && Int32.TryParse(PublicationYearTextBox.Text, out int publicationYear))
            {
                Book selectedbook = new()
                {
                    ID = id,
                    ISBN = ISBNTextBox.Text,
                    Title = TitleTextBox.Text,
                    Authors = AuthorsTextBox.Text,
                    Publisher = PublisherTextBox.Text,
                    PublicationYear = publicationYear
                };
                _bookRepository.UpdateBook(selectedbook);
                Loadbooks();
            }
            else
            {
                MessageBox.Show("Перевірте валідність даних");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (list.SelectedItem != null)
            {
                Book selectedbook = list.SelectedItem as Book;
                _bookRepository.DeleteBook(selectedbook.ID);
                Loadbooks();
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть книгу для видалення.");
            }
        }
    }
}