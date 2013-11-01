using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RichTextbox
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public string Text { get; set; }

    public MainWindow()
    {
      InitializeComponent();

      Text = @"<h2>HaHaaaaaa !!!</h2><br/><br/><br/><p>This is original text,
              this will be very long and <span style='color:red'>html format</span>. 
              So I'm display in a WebBrowser Control. You Can right click to check<p/>";

      Display();
    }

    private void EditClick(object sender, RoutedEventArgs e) {
      Edit();

      EditBtn.IsEnabled = false;
      SaveBtn.IsEnabled = true;
    }

    private void SaveClick(object sender, RoutedEventArgs e) {
      var content = Browser.InvokeScript("getContent");
      Text = content.ToString();
      MessageBox.Show(Text);
      Display();

      EditBtn.IsEnabled = true;
      SaveBtn.IsEnabled = false;
    }

    #region Processors
    private void Display()
    {
      WriteHtmlFile("index", Text);
      BrowserNavigate("index");
    }

    private void Edit()
    {
      WriteHtmlFile("edit", Text);
      BrowserNavigate("edit");
    }

    private void WriteHtmlFile(string template, string txt)
    {
      var reader = new StreamReader(Environment.CurrentDirectory + "\\WebResources\\" + template + ".txt");
      var cnt = reader.ReadToEnd();
      reader.Close();

      var writer = new StreamWriter(Environment.CurrentDirectory + "\\WebResources\\" + template + ".html");
      writer.Write(cnt.Replace("$HansTemplate$", txt));
      writer.Close();
    }

    private void BrowserNavigate(string name)
    {
      var url = string.Format("file:///{0}/WebResources/{1}.html", Environment.CurrentDirectory.Replace("\\", "/"), name);
      Console.WriteLine(url);
      Browser.Navigate(url);
    }
    #endregion
  }
}
