using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestWinForms;

namespace gfoidl.DataGridView.Test
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();

      var datas = new List<Data>()
      {
        new Data(){Column1=1,Column2="a2",Column3="b3"},
        new Data(){Column1=2,Column2="a3",Column3="b2"},
        new Data(){Column1=3,Column2="a1",Column3="b1"},
        };

      this.gfDataGridView1.DataSource = new SortableBindingList<Data>(datas);
    }
  }

  internal class Data
  {
    public int Column1 { get; set; }
    public string Column2 { get; set; }
    public string Column3 { get; set; }
  }
}