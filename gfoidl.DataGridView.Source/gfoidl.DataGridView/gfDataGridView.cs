using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace gfoidl.Windows.Forms
{
  // FROM https://www.codeproject.com/Articles/37087/DataGridView-that-Saves-Column-Order-Width-and-Vis?fid=1541926&df=90&mpp=25&sort=Position&spc=Relaxed&prof=True&view=Normal&fr=1#xx0xx
  //TODO 在Joy库中吸收此关于DataGridView的列显示(前后)顺序； 以及列排序 2021-10-02 02:07:53

  //TODO 注意解决由于代码编写过程中修改了列名称，导致保存的信息与窗体不一致时出现的异常情况处理
  /// <summary>
  /// 自定义DataGridView——增强了特性，能自动保存列Column的显示顺序以及排序状态
  /// </summary>
  [Description("DataGridView that Saves Column Order, Width and Visibility to user.config")]
  [ToolboxBitmap(typeof(System.Windows.Forms.DataGridView))]
  public class gfDataGridView : DataGridView
  {
    private string m_parentName;

    public gfDataGridView()
    {
      m_parentName = Parent == null ? string.Empty : Parent.Name;

      ColumnWidthChanged += new DataGridViewColumnEventHandler(DataGridViewEx_ColumnWidthChanged);
    }

    private void DataGridViewEx_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      SaveColumnOrder();
    }

    /// <summary>
    /// 设置列的显示顺序
    /// </summary>
    private void SetColumnOrder()
    {
      //!添加评论中的建议，解决不同的窗体里有相同的DataGridView控件导致的问题

      string key = m_parentName + Name;
      if (!gfDataGridViewSetting.Default.ColumnOrder.ContainsKey(key))
      {
        return;
      }

      //if (!gfDataGridViewSetting.Default.ColumnOrder.ContainsKey(this.Name))
      //  return;

      List<ColumnOrderItem> columnOrder = gfDataGridViewSetting.Default.ColumnOrder[key];

      if (columnOrder != null)
      {
        var sorted = columnOrder.OrderBy(i => i.DisplayIndex);
        foreach (var item in sorted)
        {
          Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
          Columns[item.ColumnIndex].Visible = item.Visible;
          Columns[item.ColumnIndex].Width = item.Width;
        }
      }
    }

    /// <summary>
    /// 设置列的升或降排序
    /// </summary>
    private void SetColumnSort()
    {
      string key = m_parentName + Name;
      if (!gfDataGridViewSetting.Default.ColumnSort.ContainsKey(key))
      {
        return;
      }

      List<ColumnSortItem> columnOrder = gfDataGridViewSetting.Default.ColumnSort[key];

      if (columnOrder != null)
      {
        var sorted = columnOrder.OrderBy(i => i.ColumnIndex);
        foreach (var item in sorted)
        {
          DataGridViewColumn dataGridViewColumn = Columns[item.ColumnIndex];

          //dataGridViewColumn.HeaderCell.SortGlyphDirection = item.IsAscending ? SortOrder.Ascending : SortOrder.Descending;
          if (item.IsSortedColumn)
          {
            var direction = item.IsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            Sort(dataGridViewColumn, direction);
          }
        }
      }
    }

    //---------------------------------------------------------------------
    private void SaveColumnOrder()
    {
      string key = m_parentName + Name;

      if (AllowUserToOrderColumns)
      {
        List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();
        DataGridViewColumnCollection columns = Columns;
        for (int i = 0; i < columns.Count; i++)
        {
          columnOrder.Add(new ColumnOrderItem
          {
            ColumnIndex = i,
            DisplayIndex = columns[i].DisplayIndex,
            Visible = columns[i].Visible,
            Width = columns[i].Width
          });
        }

        gfDataGridViewSetting.Default.ColumnOrder[key] = columnOrder;

        gfDataGridViewSetting.Default.Save();
      }
    }

    /// <summary>
    /// 保存列的排序
    /// </summary>
    private void SaveColumnSort()
    {
      string key = m_parentName + Name;

      List<ColumnSortItem> columnSort = new List<ColumnSortItem>();
      DataGridViewColumnCollection columns = Columns;
      for (int i = 0; i < columns.Count; i++)
      {
        columnSort.Add(new ColumnSortItem
        {
          ColumnIndex = i,
          IsSortedColumn = columns[i].Equals(SortedColumn),
          IsAscending = columns[i].HeaderCell.SortGlyphDirection == SortOrder.Ascending
        });
      }

      gfDataGridViewSetting.Default.ColumnSort[key] = columnSort;

      gfDataGridViewSetting.Default.Save();
    }

    //---------------------------------------------------------------------
    protected override void OnCreateControl()
    {
      base.OnCreateControl();

      //列显示顺序
      SetColumnOrder();

      //列排序
      SetColumnSort();
    }

    //---------------------------------------------------------------------
    protected override void Dispose(bool disposing)
    {
      SaveColumnOrder();
      SaveColumnSort();

      base.Dispose(disposing);
    }
  }

  //-------------------------------------------------------------------------
  internal sealed class gfDataGridViewSetting : ApplicationSettingsBase
  {
    private static gfDataGridViewSetting _defaultInstace = (gfDataGridViewSetting)Synchronized(new gfDataGridViewSetting());

    //---------------------------------------------------------------------
    public static gfDataGridViewSetting Default
    {
      get { return _defaultInstace; }
    }

    //---------------------------------------------------------------------
    // Because there can be more than one DGV in the user-application
    // a dictionary is used to save the settings for this DGV.
    // As key the name of the control is used.
    [UserScopedSetting]
    [SettingsSerializeAs(SettingsSerializeAs.Binary)]
    [DefaultSettingValue("")]
    public Dictionary<string, List<ColumnOrderItem>> ColumnOrder
    {
      get { return this["ColumnOrder"] as Dictionary<string, List<ColumnOrderItem>>; }
      set { this["ColumnOrder"] = value; }
    }

    //---------------------------------------------------------------------
    // Because there can be more than one DGV in the user-application
    // a dictionary is used to save the settings for this DGV.
    // As key the name of the control is used.
    [UserScopedSetting]
    [SettingsSerializeAs(SettingsSerializeAs.Binary)]
    [DefaultSettingValue("")]
    public Dictionary<string, List<ColumnSortItem>> ColumnSort
    {
      get { return this["ColumnSort"] as Dictionary<string, List<ColumnSortItem>>; }
      set { this["ColumnSort"] = value; }
    }
  }

  /// <summary>
  /// 列的显示顺序类
  /// </summary>
  [Serializable]
  public sealed class ColumnOrderItem
  {
    public int DisplayIndex { get; set; }
    public int Width { get; set; }
    public bool Visible { get; set; }
    public int ColumnIndex { get; set; }
  }

  //Note 2021-10-02增加列的排序
  /// <summary>
  /// 列的排序信息类
  /// </summary>
  [Serializable]
  public sealed class ColumnSortItem
  {
    /// <summary>
    /// 列序号
    /// </summary>
    public int ColumnIndex { get; set; }

    /// <summary>
    /// 是否升序
    /// </summary>
    public bool IsAscending { get; set; }

    /// <summary>
    /// 是否为当前排序的列
    /// </summary>
    public bool IsSortedColumn { get; set; }
  }
}