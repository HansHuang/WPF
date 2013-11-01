using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RichTextbox
{
  public class SilentWebBrowse : System.Windows.Forms.WebBrowser  
  {
    public SilentWebBrowse() {
      ScriptErrorsSuppressed = true;
      Document.Window.Error += Window_Error; 
 
    }
    //对错误进行处理  
    void Window_Error(object sender, HtmlElementErrorEventArgs e)
    {
      // 自己的处理代码  
      e.Handled = true;
    } 
  }
}
