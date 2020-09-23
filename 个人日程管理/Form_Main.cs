using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using LiZhiruiToolSet;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using 个人日程管理.Model;
using 个人日程管理.Service;
using CefSharp;
using CefSharp.WinForms;
using System.IO;
using CefSharp.Callback;
using CefSharp.Internals;
using System.Threading;
using System.Collections.Concurrent;
using System.Media;

namespace 个人日程管理
{
    public partial class Form_Main : Form
    {
        private const int controlInterval = 5;
        private MemoInfoService memoInfoService = new MemoInfoService();
        private EventInfoService eventInfoService = new EventInfoService();
        private TaskInfoService taskInfoService = new TaskInfoService();
        private RecordInfoService recordInfoService = new RecordInfoService();
        private bool memoModified = false;
        private Memo memoCurrentItem = null;
        private int oldFixedTaskHeightDelta = 0;
        private bool taskModified = false;
        private Model.Task taskCurrentItem = null;
        private TextEditor formulaEditor = null;
        private bool eventModified = false;
        private Event eventCurrentItem = null;
        private DateTime oldStartTime = DateTime.Now;
        private DateTime oldEndTime = DateTime.Now;
        public ChromiumWebBrowser scheduleTable = null;
        private Thread RemindThread = null;

        private enum ThreadControlCmd
        {
            Start,
            Pause,
            Stop,
            Started,
            Paused,
            Stopped
        }

        private struct ThreadControlMsg
        {
            public ThreadControlCmd cmd;
        }

        private ConcurrentQueue<ThreadControlMsg> RemindThreadMsgInQueue;
        private ConcurrentQueue<ThreadControlMsg> RemindThreadMsgOutQueue;

        class CefSharpSchemeHandler : IResourceHandler
        {
            private static readonly Dictionary<string,byte[]> resourceList;
            public static string scheduleTableHTML = "";
            
            private string mimeType;
            private MemoryStream stream;

            static CefSharpSchemeHandler()
            {
                resourceList = new Dictionary<string,byte[]>
                {
                    {"/bootstrap.min.css",Encoding.UTF8.GetBytes(Resource.bootstrap_min_css)},
                    {"/bootstrap.min.css.map",Encoding.UTF8.GetBytes(Resource.bootstrap_min_css_map)},
                    {"/bootstrap.min.js",Encoding.UTF8.GetBytes(Resource.bootstrap_min_js)},
                    {"/jquery-3.5.1.min.js",Encoding.UTF8.GetBytes(Resource.jquery_3_5_1_min_js)}
                };
            }

            public void Cancel()
            {
                
            }

            public void Dispose()
            {
                if(stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            public void GetResponseHeaders(IResponse response,out long responseLength,out string redirectUrl)
            {
                redirectUrl = null;
                response.MimeType = mimeType;

                if(stream != null)
                {
                    responseLength = stream.Length;
                    response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    response.StatusText = "OK";
                }
                else
                {
                    responseLength = 0;
                    response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                    response.StatusText = "NotFound";
                }
            }

            public bool Open(IRequest request,out bool handleRequest,ICallback callback)
            {
                handleRequest = true;
                return ProcessRequest(request,callback);
            }

            public bool ProcessRequest(IRequest request,ICallback callback)
            {
                var uri = new Uri(request.Url);
                var resourceName = uri.AbsolutePath;

                if(string.Equals(resourceName,"/scheduleTable.html",StringComparison.OrdinalIgnoreCase))
                {
                    mimeType = Cef.GetMimeType(".html");
                    stream = new MemoryStream(Encoding.UTF8.GetBytes(scheduleTableHTML));
                    callback.Continue();
                    return true;
                }

                byte[] resource;

                if(resourceList.TryGetValue(resourceName,out resource))
                {
                    stream = new MemoryStream(resource);
                    var fileExtension = Path.GetExtension(resourceName);
                    mimeType = Cef.GetMimeType(fileExtension);
                    callback.Continue();
                    return true;
                }
                else
                {
                    mimeType = Cef.GetMimeType(".html");
                    stream = null;
                    callback.Continue();
                }

                return false;
            }

            public bool Read(Stream dataOut,out int bytesRead,IResourceReadCallback callback)
            {
                callback.Dispose();

                if(stream == null)
                {
                    bytesRead = 0;
                    return false;
                }

                var buffer = new byte[dataOut.Length];
                bytesRead = stream.Read(buffer,0,buffer.Length);
                dataOut.Write(buffer,0,buffer.Length);
                return bytesRead > 0;
            }

            public bool ReadResponse(Stream dataOut,out int bytesRead,ICallback callback)
            {
                callback.Dispose();

                if(stream == null)
                {
                    bytesRead = 0;
                    return false;
                }

                var buffer = new byte[dataOut.Length];
                bytesRead = stream.Read(buffer,0,buffer.Length);
                dataOut.Write(buffer,0,buffer.Length);
                return bytesRead > 0;
            }

            public bool Skip(long bytesToSkip,out long bytesSkipped,IResourceSkipCallback callback)
            {
                bytesSkipped = 0;
                return true;
            }
        }

        class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory
        {
            public const string SchemeName = "local";
            

            public IResourceHandler Create(IBrowser browser,IFrame frame,string schemeName,IRequest request)
            {
                if(schemeName == SchemeName)
                {
                    return new CefSharpSchemeHandler();
                }
                else
                {
                    var mimeType = Cef.GetMimeType(".html");
                    return ResourceHandler.FromString("",mimeType : Cef.GetMimeType(".html"));
                }
            }
        }

        class JSGlobal
        {
            private Form_Main main = null;

            public JSGlobal(Form_Main main)
            {
                this.main = main;
            }

            public void viewEvent(int id)
            {
                main.viewEvent(id);
            }
        }

        public Form_Main()
        {
            InitializeComponent();
            formulaEditor = new TextEditor();
            formulaEditor.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            formulaEditor.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            formulaEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript");
            formulaEditor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            formulaEditor.FontSize = 13;
            formulaEditor.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff,0xc8,0xc8,0xc8));
            formulaEditor.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff,0x1e,0x1e,0x1e));
            formulaEditor.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
            formulaEditor.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
            formulaEditor.ShowLineNumbers = true;
            formulaEditor.WordWrap = true;
            formulaEditor.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff,0x1e,0x1e,0x1e));
            formulaEditor.BorderThickness = new System.Windows.Thickness(1);
            formulaEditor.TextChanged += FormulaEditor_TextChanged;
            formulaEditor.KeyDown += FormulaEditor_KeyDown;
            elementHost_Event_RemindFormula.Child = formulaEditor;
            var settings = new CefSettings();

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory(),
                IsSecure = false,
                IsLocal = false,
                IsStandard = true
            });

            Cef.Initialize(settings);
            scheduleTable = new ChromiumWebBrowser("about:blank");
            scheduleTable.Dock = DockStyle.None;
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
            scheduleTable.JavascriptObjectRepository.Register("global",new JSGlobal(this),isAsync : false,options : BindingOptions.DefaultBinder);
            tabPage_Schedule.Controls.Add(scheduleTable);
            RemindThreadMsgInQueue = new ConcurrentQueue<ThreadControlMsg>();
            RemindThreadMsgOutQueue = new ConcurrentQueue<ThreadControlMsg>();
            RemindThread = new Thread(new ThreadStart(RemindThreadEntry));
            RemindThread.Start();
            checkBox_Schedule_Remind_CheckedChanged(checkBox_Schedule_Remind,new EventArgs());
        }

        private void Form_Main_Load(object sender,EventArgs e)
        {
            oldFixedTaskHeightDelta = panel_Task_Data.ClientRectangle.Height - textBox_Task_Description.Bottom;
            Form_Main_SizeChanged(sender,e);
            UpdateMemoList();
            UpdateTaskList();
        }

        private void Form_Main_SizeChanged(object sender,EventArgs e)
        {
            tabControl_Main.Left = ClientRectangle.X;
            tabControl_Main.Top = ClientRectangle.Y;
            tabControl_Main.Width = ClientRectangle.Width;
            tabControl_Main.Height = ClientRectangle.Height;
        }

        private void tabPage_Memo_SizeChanged(object sender,EventArgs e)
        {
            listBox_Memo_List.Left = tabPage_Memo.ClientRectangle.X;
            listBox_Memo_List.Top = tabPage_Memo.ClientRectangle.Y;
            listBox_Memo_List.Width = (int)(tabPage_Memo.ClientRectangle.Width * 0.3);
            listBox_Memo_List.Height = tabPage_Memo.ClientRectangle.Height;
            textBox_Memo_Title.Left = listBox_Memo_List.Left + listBox_Memo_List.Width + controlInterval;
            textBox_Memo_Title.Top = listBox_Memo_List.Top;
            textBox_Memo_Title.Width = tabPage_Memo.ClientRectangle.Width - textBox_Memo_Title.Left - controlInterval;
            textBox_Memo_Description.Left = textBox_Memo_Title.Left;
            textBox_Memo_Description.Top = textBox_Memo_Title.Top + textBox_Memo_Title.Height + controlInterval;
            textBox_Memo_Description.Width = textBox_Memo_Title.Width;
            textBox_Memo_Description.Height = tabPage_Memo.ClientRectangle.Height - textBox_Memo_Description.Top - 50;
            button_Memo_Add.Left = textBox_Memo_Description.Left + (textBox_Memo_Description.Width >> 1) - button_Memo_Add.Width - 20;
            button_Memo_Add.Top = textBox_Memo_Description.Top + textBox_Memo_Description.Height + controlInterval;
            button_Memo_Update.Left = textBox_Memo_Description.Left + (textBox_Memo_Description.Width >> 1) + 20;
            button_Memo_Update.Top = button_Memo_Add.Top;
        }

        private void UpdateMemoList()
        {
            listBox_Memo_List.Items.Clear();

            foreach(var titem in memoInfoService.GetAllList())
            {
                listBox_Memo_List.Items.Add(titem);
            }
        }

        private void SelectMemo(Memo memo)
        {
            if(memo != null)
            {
                var index = 0;

                foreach(var item in listBox_Memo_List.Items)
                {
                    if((item as Memo).id == memo.id)
                    {
                        listBox_Memo_List.SelectedIndex = index;

                        if(!memoModified)
                        {
                            listBox_Memo_List_Click(listBox_Memo_List,new EventArgs());
                        }

                        break;
                    }

                    index++;
                }
            }
        }

        private void button_Memo_Add_Click(object sender,EventArgs e)
        {
            var memo = new Memo();
            memo.id = -1;
            memo.parentId = -1;
            memo.title = textBox_Memo_Title.Text;
            memo.description = textBox_Memo_Description.Text;
            memo.createdTime = DateTime.Now;
            
            if(memo.title == "" || memo.description == "")
            {
                Global.Error("标题或内容为空！");
            }

            memoInfoService.Add(memo);
            memoModified = false;
            UpdateMemoList();
            SelectMemo(memo);
        }

        private void listBox_Memo_List_Click(object sender,EventArgs e)
        {
            if(listBox_Memo_List.SelectedIndex >= 0)
            {
                if(!memoModified || Global.Confirm("是否要抛弃当前输入内容？"))
                {
                    textBox_Memo_Title.Text = (listBox_Memo_List.SelectedItem as Memo).title;
                    textBox_Memo_Description.Text = (listBox_Memo_List.SelectedItem as Memo).description;
                    memoCurrentItem = listBox_Memo_List.SelectedItem as Memo;
                    memoModified = false;
                }
                else
                {
                    SelectMemo(memoCurrentItem);
                }
            }
        }

        private void textBox_Memo_Title_TextChanged(object sender,EventArgs e)
        {
            memoModified = true;
        }

        private void textBox_Memo_Description_TextChanged(object sender,EventArgs e)
        {
            memoModified = true;
        }

        private void button_Memo_Update_Click(object sender,EventArgs e)
        {
            if(memoCurrentItem == null)
            {
                Global.Error("无可更新项！");
                return;
            }

            memoCurrentItem.title = textBox_Memo_Title.Text;
            memoCurrentItem.description = textBox_Memo_Description.Text;
            memoInfoService.Modify(memoCurrentItem);
            memoModified = false;
            UpdateMemoList();
            SelectMemo(memoCurrentItem);
        }

        private void listBox_Memo_List_KeyDown(object sender,KeyEventArgs e)
        {
            if(e.Modifiers == Keys.None)
            {
                switch(e.KeyCode)
                {
                    case Keys.Delete:
                        if(listBox_Memo_List.SelectedIndex >= 0)
                        {
                            if(Global.Confirm("是否删除？"))
                            {
                                if(memoCurrentItem != null && memoCurrentItem.id == (listBox_Memo_List.SelectedItem as Memo).id)
                                {
                                    memoCurrentItem = null;
                                }

                                memoInfoService.Delete((listBox_Memo_List.SelectedItem as Memo).id);
                                UpdateMemoList();
                            }
                        }

                        break;

                    case Keys.F5:
                        var sel = listBox_Memo_List.SelectedItem;
                        UpdateMemoList();
                        SelectMemo(sel as Memo);
                        break;
                }
            }
        }

        private void tabPage_Task_SizeChanged(object sender,EventArgs e)
        {
            treeView_Task_Dir.Left = tabPage_Task.ClientRectangle.X;
            treeView_Task_Dir.Top = tabPage_Task.ClientRectangle.Y;
            treeView_Task_Dir.Width = (int)(tabPage_Task.ClientRectangle.Width * 0.3);
            treeView_Task_Dir.Height = tabPage_Task.ClientRectangle.Height;
            listView_Task_Item.Left = treeView_Task_Dir.Left + treeView_Task_Dir.Width + controlInterval;
            listView_Task_Item.Top = treeView_Task_Dir.Top;
            listView_Task_Item.Width = tabPage_Task.ClientRectangle.Width - listView_Task_Item.Left - controlInterval;
            listView_Task_Item.Height = treeView_Task_Dir.Height;
            panel_Task_Data.Left = listView_Task_Item.Left;
            panel_Task_Data.Top = listView_Task_Item.Top;
            panel_Task_Data.Width = listView_Task_Item.Width;
            panel_Task_Data.Height = listView_Task_Item.Height;
        }

        void UpdateTaskList()
        {
            treeView_Task_Dir.Nodes.Clear();
            var nodesMap = new Dictionary<int,TreeNodeCollection>();
            var rootnode = new TreeNode();
            rootnode.Tag = null;
            rootnode.Name = "root";
            rootnode.Text = "root";
            nodesMap.Add(-1,rootnode.Nodes);
            treeView_Task_Dir.Nodes.Add(rootnode);
            
            foreach(var item in taskInfoService.GetAllChildList(-1))
            {
                var node = new TreeNode();
                node.Tag = item;
                node.Name = item.id + "";
                node.Text = item.title + " -- " + Math.Round(item.finishedProgress * 100.0d / item.totalProgress,2) + "%";
                nodesMap[item.id] = node.Nodes;
                nodesMap[item.parentId].Add(node);
            }
        }

        private void panel_Task_Data_SizeChanged(object sender,EventArgs e)
        {
            textBox_Task_Title.Width = panel_Task_Data.ClientRectangle.Width - controlInterval - textBox_Task_Title.Left;
            textBox_Task_Description.Width = panel_Task_Data.ClientRectangle.Width - controlInterval - textBox_Task_Description.Left;
            var oldDesHeight = textBox_Task_Description.Height;
            textBox_Task_Description.Height = panel_Task_Data.ClientRectangle.Height - oldFixedTaskHeightDelta - textBox_Task_Description.Top;
            var deltaHeight = textBox_Task_Description.Height - oldDesHeight;
            label_Task_Progress.Top += deltaHeight;
            label_Task_Separate.Top += deltaHeight;
            textBox_Task_FinishedProgress.Top += deltaHeight;
            textBox_Task_TotalProgress.Top += deltaHeight;
            textBox_Task_Unit.Top += deltaHeight;
            textBox_Task_ProgressPercent.Top += deltaHeight;
            button_Task_Add.Left = panel_Task_Data.ClientRectangle.Left + (panel_Task_Data.ClientRectangle.Width >> 1) - button_Task_Add.Width - 20;
            button_Task_Add.Top += deltaHeight;
            button_Task_Update.Left = panel_Task_Data.ClientRectangle.Left + (panel_Task_Data.ClientRectangle.Width >> 1) + 20;
            button_Task_Update.Top += deltaHeight;
        }

        private bool SelectTask_DFS(Model.Task task,TreeNodeCollection CurNodes,TreeNode ParentNode,bool expandDirectChild)
        {
            for(var i = 0;i < CurNodes.Count;i++)
            {
                if((task.id == -1 && CurNodes[i].Tag == null) || (CurNodes[i].Tag != null && (CurNodes[i].Tag as Model.Task).id == task.id))
                {
                    if(expandDirectChild && CurNodes[i].Nodes.Count > 0)
                    {
                        treeView_Task_Dir.SelectedNode = CurNodes[i].Nodes[0];
                        CurNodes[i].Expand();
                    }
                    else
                    {
                        treeView_Task_Dir.SelectedNode = CurNodes[i];

                        if(ParentNode != null)
                        {
                            ParentNode.Expand();
                        }
                    }
                    
                    treeView_Task_Dir.Select();
                    return true;
                }
                
                if(SelectTask_DFS(task,CurNodes[i].Nodes,CurNodes[i],expandDirectChild))
                {
                    return true;
                }
            }

            return false;
        }

        private void SelectTask(Model.Task task,bool expandDirectChild = false)
        {
            if(task != null)
            {
                SelectTask_DFS(task,treeView_Task_Dir.Nodes,null,expandDirectChild);
            }
        }

        private string GetSpeedString(double amount,string unit,TimeSpan ts)
        {
            const int roundLength = 6;

            if(ts.TotalMilliseconds < 1)
            {
                return "时间太短无法计算";
            }

            if(ts.TotalSeconds < 1)
            {
                return $"{Math.Round(amount / ts.TotalMilliseconds,roundLength)}{unit}/ms";
            }
            else if(ts.TotalMinutes < 1)
            {
                return $"{Math.Round(amount / ts.TotalSeconds,roundLength)}{unit}/s";
            }
            else if(ts.TotalHours < 1)
            {
                return $"{Math.Round(amount / ts.TotalMinutes,roundLength)}{unit}/min";
            }
            else if(ts.TotalDays < 1)
            {
                return $"{Math.Round(amount / ts.TotalHours,roundLength)}{unit}/h";
            }
            else
            {
                return $"{Math.Round(amount / ts.TotalDays,roundLength)}{unit}/day";
            }
        }

        private void UpdateTaskData(int parentId)
        {
            listView_Task_Item.Items.Clear();

            foreach(var titem in taskInfoService.GetAllDirectChildList(parentId))
            {
                var item = new ListViewItem();
                item.Tag = titem;
                item.Name = titem.id + "";
                item.Text = titem.title;
                listView_Task_Item.Items.Add(item);
                item.SubItems.Add(titem.finishedProgress + titem.progressUnit + "/" + titem.totalProgress + titem.progressUnit);
                item.SubItems.Add(Math.Round(titem.finishedProgress * 100.0d / titem.totalProgress,2) + "%");
                
                if(!titem._hasLinkedEvent)
                {
                    item.SubItems.Add("无对应事件");
                    item.SubItems.Add("无对应事件");
                    item.SubItems.Add("无对应事件");
                }
                else
                {
                    if(titem._hasChild)
                    {
                        item.SubItems.Add(GetSpeedString(titem.finishedProgress * 100.0d / titem.totalProgress,"%",DateTime.Now - titem._firstStartTime));
                        item.SubItems.Add(GetSpeedString(100.0d - titem.finishedProgress * 100.0d / titem.totalProgress,"%",titem._lastEndTime - DateTime.Now));
                        item.SubItems.Add(titem._lastEndTime.ToString("yyyy/MM/dd hh:mm:ss"));
                    }
                    else
                    {
                        item.SubItems.Add(GetSpeedString(titem.finishedProgress,titem.progressUnit,DateTime.Now - titem._firstStartTime));
                        item.SubItems.Add(GetSpeedString(titem.totalProgress - titem.finishedProgress,titem.progressUnit,titem._lastEndTime - DateTime.Now));
                    }

                    item.SubItems.Add(titem._lastEndTime.ToString("yyyy/MM/dd hh:mm:ss"));
                }

                item.SubItems.Add(titem.createdTime.ToString("yyyy/MM/dd hh:mm:ss"));
            }
        }

        private void treeView_Task_Dir_AfterSelect(object sender,TreeViewEventArgs e)
        {
            
        }

        private void treeView_Task_Dir_BeforeSelect(object sender,TreeViewCancelEventArgs e)
        {
            if(!taskModified || Global.Confirm("是否要抛弃当前输入内容？"))
            {
                if(e.Node != null && e.Node.Tag != null)
                {
                    var task = e.Node.Tag as Model.Task;
                    textBox_Task_Title.Text = task.title;
                    textBox_Task_Description.Text = task.description;
                    textBox_Task_FinishedProgress.Text = task.finishedProgress + "";
                    textBox_Task_TotalProgress.Text = task.totalProgress + "";
                    textBox_Task_Unit.Text = task.progressUnit;
                    taskCurrentItem = task;

                    if(panel_Task_Data.Visible)
                    {
                        panel_Task_Data.Tag = task.id;
                    }
                    else
                    {
                        UpdateTaskData(task.id);
                    }
                }
                else
                {
                    if(panel_Task_Data.Visible)
                    {
                        panel_Task_Data.Tag = -1;
                    }
                    else
                    {
                        UpdateTaskData(-1);
                    }

                    taskCurrentItem = null;
                }

                taskModified = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void treeView_Task_Dir_NodeMouseClick(object sender,TreeNodeMouseClickEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Left:
                    if(e.Node == treeView_Task_Dir.SelectedNode)
                    {
                        treeView_Task_Dir_BeforeSelect(sender,new TreeViewCancelEventArgs(e.Node,false,TreeViewAction.ByMouse));
                    }

                    break;

                case MouseButtons.Right:
                    panel_Task_Data.Visible = !panel_Task_Data.Visible;
                    break;
            }
        }

        private void button_Task_Add_Click(object sender,EventArgs e)
        {
            var task = new Model.Task();
            task.id = -1;
            task.parentId = treeView_Task_Dir.SelectedNode == null || treeView_Task_Dir.SelectedNode.Tag == null ? -1 : (treeView_Task_Dir.SelectedNode.Tag as Model.Task).id;
            task.title = textBox_Task_Title.Text;
            task.description = textBox_Task_Description.Text;
            task.finishedProgress = textBox_Task_FinishedProgress.Text.Trim().Length > 0 ? int.Parse(textBox_Task_FinishedProgress.Text.Trim()) : 0;
            task.totalProgress = textBox_Task_TotalProgress.Text.Trim().Length > 0 ? int.Parse(textBox_Task_TotalProgress.Text.Trim()) : 0;
            task.progressUnit = textBox_Task_Unit.Text;
            task.createdTime = DateTime.Now;

            if(task.title == "" || task.description == "")
            {
                Global.Error("标题或内容为空！");
            }

            taskInfoService.Add(task);
            taskModified = false;
            UpdateTaskList();
            SelectTask(task);
        }

        private void button_Task_Update_Click(object sender,EventArgs e)
        {
            if(taskCurrentItem == null)
            {
                Global.Error("无可更新项！");
                return;
            }
            
            taskCurrentItem.title = textBox_Task_Title.Text;
            taskCurrentItem.description = textBox_Task_Description.Text;
            taskCurrentItem.finishedProgress = textBox_Task_FinishedProgress.Text.Trim().Length > 0 ? int.Parse(textBox_Task_FinishedProgress.Text.Trim()) : 0;
            taskCurrentItem.totalProgress = textBox_Task_TotalProgress.Text.Trim().Length > 0 ? int.Parse(textBox_Task_TotalProgress.Text.Trim()) : 0;
            taskCurrentItem.progressUnit = textBox_Task_Unit.Text;
            taskInfoService.Modify(taskCurrentItem);
            taskModified = false;
            UpdateTaskList();
            SelectTask(taskCurrentItem);
        }

        private void textBox_Task_FinishedProgress_TextChanged(object sender,EventArgs e)
        {
            taskModified = true;
            IntegerInputLimit(sender as TextBox);
            taskProgressChanged();
        }

        private void textBox_Task_TotalProgress_TextChanged(object sender,EventArgs e)
        {
            taskModified = true;
            IntegerInputLimit(sender as TextBox);
            taskProgressChanged();
        }

        private void panel_Task_Data_VisibleChanged(object sender,EventArgs e)
        {
            if(!panel_Task_Data.Visible)
            {
                if(panel_Task_Data.Tag != null)
                {
                    UpdateTaskData((int)panel_Task_Data.Tag);
                    panel_Task_Data.Tag = null;
                }
            }
        }

        private void taskProgressChanged()
        {
            var finished = textBox_Task_FinishedProgress.Text.Trim().Length > 0 ? int.Parse(textBox_Task_FinishedProgress.Text.Trim()) : 0;
            var total = textBox_Task_TotalProgress.Text.Trim().Length > 0 ? int.Parse(textBox_Task_TotalProgress.Text.Trim()) : 0;
            textBox_Task_ProgressPercent.Text = (total == 0 ? 0.0d : Math.Round(finished * 100.0d / total,2)) + "%";
        }

        private void IntegerInputLimit(TextBox textBox)
        {
            var reg = new Regex("^[0-9]*$");
            var str = textBox.Text.Trim();
            var sb = new StringBuilder();

            if(!reg.IsMatch(str))
            {
                foreach(var ch in str)
                {
                    if(reg.IsMatch(ch.ToString()))
                    {
                        sb.Append(ch);
                    }
                }

                textBox.Text = sb.ToString();
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void textBox_Task_Title_TextChanged(object sender,EventArgs e)
        {
            taskModified = true;
        }

        private void textBox_Task_Description_TextChanged(object sender,EventArgs e)
        {
            taskModified = true;
        }

        private void treeView_Task_Dir_KeyDown(object sender,KeyEventArgs e)
        {
            if(e.Modifiers == Keys.None)
            {
                switch(e.KeyCode)
                {
                    case Keys.Delete:
                        if(treeView_Task_Dir.SelectedNode != null)
                        {
                            if(Global.Confirm("是否删除？"))
                            {
                                if(taskCurrentItem != null && treeView_Task_Dir.SelectedNode.Tag != null && taskCurrentItem.id == (treeView_Task_Dir.SelectedNode.Tag as Model.Task).id)
                                {
                                    taskCurrentItem = null;
                                }

                                var newtask = new Model.Task();
                                newtask.id = (treeView_Task_Dir.SelectedNode.Tag as Model.Task).parentId;
                                taskInfoService.Delete((treeView_Task_Dir.SelectedNode.Tag as Model.Task).id);
                                UpdateTaskList();
                                SelectTask(newtask,true);
                            }
                        }

                        break;

                    case Keys.F5:
                        var sel = listBox_Memo_List.SelectedItem;
                        UpdateMemoList();
                        SelectMemo(sel as Memo);
                        break;
                }
            }
        }

        private void treeView_Task_Dir_DrawNode(object sender,DrawTreeNodeEventArgs e)
        {
            if(e.State == TreeNodeStates.Selected)
            {
                var bgColor = Color.FromArgb(51,153,255);
                var brush = new SolidBrush(bgColor);
                e.Graphics.FillRectangle(brush,new Rectangle(e.Node.Bounds.Left - 1,e.Node.Bounds.Top - 1,e.Node.Bounds.Width + 1,e.Node.Bounds.Height + 1));
                TextRenderer.DrawText(e.Graphics,e.Node.Text,e.Node.TreeView.Font,new Rectangle(e.Node.Bounds.Left,e.Node.Bounds.Top - 1,e.Node.Bounds.Width,e.Node.Bounds.Height),Color.White);
                var pen = new Pen(Color.Black,1);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                pen.DashPattern = new float[]{1f,1f};
                pen.DashOffset = 1;
                e.Graphics.DrawRectangle(pen,new Rectangle(e.Node.Bounds.Left - 1,e.Node.Bounds.Top - 1,e.Node.Bounds.Width,e.Node.Bounds.Height));
                pen.DashOffset = 0;
                e.Graphics.DrawRectangle(pen,new Rectangle(e.Node.Bounds.Left - 1,e.Node.Bounds.Top - 1,e.Node.Bounds.Width,1));
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void tabPage_Record_SizeChanged(object sender,EventArgs e)
        {
            listView_Record_List.Width = tabPage_Record.ClientRectangle.Width - listView_Record_List.Left - controlInterval;
            listView_Record_List.Height = tabPage_Record.ClientRectangle.Height - listView_Record_List.Top;
        }

        private void button_Record_Search_Click(object sender,EventArgs e)
        {
            listView_Record_List.Items.Clear();

            foreach(var titem in recordInfoService.GetList(new DateTime(dateTimePicker_Record_StartTime.Value.Year,dateTimePicker_Record_StartTime.Value.Month,dateTimePicker_Record_StartTime.Value.Day,0,0,0),new DateTime(dateTimePicker_Record_EndTime.Value.Year,dateTimePicker_Record_EndTime.Value.Month,dateTimePicker_Record_EndTime.Value.Day,23,59,59)))
            {
                var item = new ListViewItem(titem.time.ToString("yyyy/MM/dd hh:mm:ss"));
                listView_Record_List.Items.Add(item);
                item.SubItems.Add(titem.Description);
            }
        }

        private void tabPage_Event_SizeChanged(object sender,EventArgs e)
        {
            listBox_Event_List.Left = tabPage_Event.ClientRectangle.X;
            listBox_Event_List.Width = (int)(tabPage_Event.ClientRectangle.Width * 0.3);
            listBox_Event_List.Height = tabPage_Event.ClientRectangle.Height;
            textBox_Event_Title.Left = listBox_Event_List.Left + listBox_Event_List.Width + controlInterval;
            textBox_Event_Title.Top = listBox_Event_List.Top;
            textBox_Event_Title.Width = tabPage_Event.ClientRectangle.Width - textBox_Event_Title.Left - controlInterval;
            textBox_Event_Description.Left = textBox_Event_Title.Left;
            textBox_Event_Description.Top = textBox_Event_Title.Top + textBox_Event_Title.Height + controlInterval;
            textBox_Event_Description.Width = textBox_Event_Title.Width;
            textBox_Event_Description.Height = (tabPage_Event.ClientRectangle.Height - textBox_Event_Description.Top - dateTimePicker_Event_StartTime.Height - controlInterval * 3 - button_Event_Add.Height) >> 1;
            dateTimePicker_Event_StartTime.Top = textBox_Event_Description.Bottom + controlInterval;
            dateTimePicker_Event_EndTime.Top = dateTimePicker_Event_StartTime.Top;
            dateTimePicker_Event_StartTime.Left = textBox_Event_Description.Left + (textBox_Event_Description.Width >> 1) - dateTimePicker_Event_StartTime.Width - 20;
            dateTimePicker_Event_EndTime.Left = textBox_Event_Description.Left + (textBox_Event_Description.Width >> 1) + 20;
            checkBox_Event_Enable.Left = dateTimePicker_Event_EndTime.Right + 20;
            checkBox_Event_Enable.Top = dateTimePicker_Event_EndTime.Top + 3;
            button_Event_ScriptTest.Left = checkBox_Event_Enable.Right + 20;
            button_Event_ScriptTest.Top = dateTimePicker_Event_StartTime.Top - 2;
            checkBox_Event_LinkTask.Left = button_Event_ScriptTest.Right + 20;
            checkBox_Event_LinkTask.Top = checkBox_Event_Enable.Top;
            elementHost_Event_RemindFormula.Left = textBox_Event_Title.Left;
            elementHost_Event_RemindFormula.Top = button_Event_ScriptTest.Bottom + controlInterval;
            elementHost_Event_RemindFormula.Width = textBox_Event_Title.Width;
            elementHost_Event_RemindFormula.Height = tabPage_Event.ClientRectangle.Height - textBox_Event_Description.Top - button_Event_ScriptTest.Height - controlInterval * 3 - button_Event_Add.Height - textBox_Event_Description.Height;
            button_Event_Add.Left = textBox_Event_Description.Left + (textBox_Event_Description.Width >> 1) - button_Event_Add.Width - 20;
            button_Event_Add.Top = elementHost_Event_RemindFormula.Bottom + controlInterval;
            button_Event_Update.Left = textBox_Event_Description.Left + (textBox_Event_Description.Width >> 1) + 20;
            button_Event_Update.Top = button_Event_Add.Top;
        }

        private void UpdateEventList(DateTime startTime,DateTime endTime)
        {
            oldStartTime = startTime;
            oldEndTime = endTime;
            listBox_Event_List.Items.Clear();

            foreach(var titem in eventInfoService.GetAllList(startTime,endTime))
            {
                listBox_Event_List.Items.Add(titem);
            }
        }

        private void UpdateEventList()
        {
            UpdateEventList(oldStartTime,oldEndTime);
        }

        private void SelectEvent(Event _event)
        {
            if(_event != null)
            {
                var index = 0;

                foreach(var item in listBox_Event_List.Items)
                {
                    if((item as Event).id == _event.id)
                    {
                        listBox_Event_List.SelectedIndex = index;

                        if(!eventModified)
                        {
                            listBox_Event_List_Click(listBox_Event_List,new EventArgs());
                        }

                        break;
                    }

                    index++;
                }
            }
        }

        private void button_Event_Search_Click(object sender,EventArgs e)
        {
            var startTime = dateTimePicker_Event_StartDate.Value;
            startTime = new DateTime(startTime.Year,startTime.Month,startTime.Day,0,0,0);
            var endTime = dateTimePicker_Event_EndDate.Value;
            endTime = new DateTime(endTime.Year,endTime.Month,endTime.Day,23,59,59);
            UpdateEventList(startTime,endTime);
        }

        private void button_Event_SearchAll_Click(object sender,EventArgs e)
        {
            UpdateEventList(DateTime.MinValue,DateTime.MaxValue);
        }

        private void button_Event_Add_Click(object sender,EventArgs e)
        {
            var _event = new Model.Event();
            _event.id = -1;
            _event.createdTime = DateTime.Now;
            _event.title = textBox_Event_Title.Text;
            _event.description = textBox_Event_Description.Text;
            _event.startTime = dateTimePicker_Event_StartTime.Value;
            _event.endTime = dateTimePicker_Event_EndTime.Value;
            _event.enabled = checkBox_Event_Enable.Checked;
            _event.remindFormula = formulaEditor.Text;
            _event.taskId = checkBox_Event_LinkTask.Tag == null ? -1 : (int)checkBox_Event_LinkTask.Tag;
            _event.type = checkBox_Event_LinkTask.Checked ? Event.Type.Task : Event.Type.GenericEvent;

            if(_event.title == "" || _event.description == "")
            {
                Global.Error("标题或内容为空！");
            }

            eventInfoService.Add(_event);
            eventModified = false;
            UpdateEventList();
            SelectEvent(_event);
        }

        private void button_Event_Update_Click(object sender,EventArgs e)
        {
            if(eventCurrentItem == null)
            {
                Global.Error("无可更新项！");
                return;
            }
            
            eventCurrentItem.title = textBox_Event_Title.Text;
            eventCurrentItem.description = textBox_Event_Description.Text;
            eventCurrentItem.startTime = dateTimePicker_Event_StartTime.Value;
            eventCurrentItem.endTime = dateTimePicker_Event_EndTime.Value;
            eventCurrentItem.enabled = checkBox_Event_Enable.Checked;
            eventCurrentItem.remindFormula = formulaEditor.Text;
            eventCurrentItem.taskId = checkBox_Event_LinkTask.Tag == null ? -1 : (int)checkBox_Event_LinkTask.Tag;
            eventCurrentItem.type = checkBox_Event_LinkTask.Checked ? Event.Type.Task : Event.Type.GenericEvent;
            eventInfoService.Modify(eventCurrentItem);
            eventModified = false;
            UpdateEventList();
            SelectEvent(eventCurrentItem);
        }

        private void listBox_Event_List_Click(object sender,EventArgs e)
        {
            if(listBox_Event_List.SelectedIndex >= 0)
            {
                if(!eventModified || Global.Confirm("是否要抛弃当前输入内容？"))
                {
                    var item = listBox_Event_List.SelectedItem as Event;
                    textBox_Event_Title.Text = item.title;
                    textBox_Event_Description.Text = item.description;
                    dateTimePicker_Event_StartTime.Value = item.startTime;
                    dateTimePicker_Event_EndTime.Value = item.endTime;
                    checkBox_Event_Enable.Checked = item.enabled;
                    checkBox_Event_LinkTask.Tag = item.taskId;
                    checkBox_Event_LinkTask.Checked = item.type == Event.Type.Task;
                    formulaEditor.Text = item.remindFormula;
                    eventCurrentItem = listBox_Event_List.SelectedItem as Event;
                    eventModified = false;
                }
                else
                {
                    SelectEvent(eventCurrentItem);
                }
            }
        }

        private void listBox_Event_List_KeyDown(object sender,KeyEventArgs e)
        {
            if(e.Modifiers == Keys.None)
            {
                switch(e.KeyCode)
                {
                    case Keys.Delete:
                        if(listBox_Event_List.SelectedIndex >= 0)
                        {
                            if(Global.Confirm("是否删除？"))
                            {
                                if(eventCurrentItem != null && eventCurrentItem.id == (listBox_Event_List.SelectedItem as Event).id)
                                {
                                    eventCurrentItem = null;
                                }

                                eventInfoService.Delete((listBox_Event_List.SelectedItem as Event).id);
                                UpdateEventList();
                            }
                        }

                        break;

                    case Keys.F5:
                        var sel = listBox_Event_List.SelectedItem;
                        UpdateEventList();
                        SelectEvent(sel as Event);
                        break;
                }
            }
        }

        private void textBox_Event_Title_TextChanged(object sender,EventArgs e)
        {
            eventModified = true;
        }

        private void textBox_Event_Description_TextChanged(object sender,EventArgs e)
        {
            eventModified = true;
        }

        private void dateTimePicker_Event_StartTime_ValueChanged(object sender,EventArgs e)
        {
            eventModified = true;
        }

        private void dateTimePicker_Event_EndTime_ValueChanged(object sender,EventArgs e)
        {
            eventModified = true;
        }

        private void checkBox_Event_Enable_CheckedChanged(object sender,EventArgs e)
        {
            eventModified = true;
        }

        private void FormulaEditor_TextChanged(object sender,EventArgs e)
        {
            eventModified = true;
        }

        private void checkBox_Event_LinkTask_CheckedChanged(object sender,EventArgs e)
        {
            
        }

        private void checkBox_Event_LinkTask_Click(object sender,EventArgs e)
        {
            eventModified = true;

            if(checkBox_Event_LinkTask.Tag == null)
            {
                checkBox_Event_LinkTask.Tag = -1;
            }

            var taskLinker = new Form_TaskLinker((int)checkBox_Event_LinkTask.Tag);
            taskLinker.ShowDialog();
            checkBox_Event_LinkTask.Tag = taskLinker.taskId;
            checkBox_Event_LinkTask.Checked = !(taskLinker.DialogResult == DialogResult.No || taskLinker.taskId == -1);
        }
        
        private bool EventDateCheckScriptCompile()
        {
            try
            {
                var fm = new FormulaManager(formulaEditor.Text);
                fm.CheckDate(DateTime.Now);
                var startDate = fm.GetStartDate().Date;
                var endDate = fm.GetEndDate().Date;

                if(startDate > endDate)
                {
                    Global.Error("开始日期大于结束日期！");
                    return false;
                }
                    
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        private void button_Event_ScriptTest_Click(object sender,EventArgs e)
        {
            if(EventDateCheckScriptCompile())
            {
                new Form_ScriptTest(formulaEditor.Text).ShowDialog(this);
            }
        }

        private void FormulaEditor_KeyDown(object sender,System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.F9)
            {
                new Form_ScriptHelper(formulaEditor).ShowDialog(this);
            }
        }

        private void tabPage_Schedule_SizeChanged(object sender,EventArgs e)
        {
            scheduleTable.Left = 6;
            scheduleTable.Top = 40;
            scheduleTable.Width = tabPage_Schedule.ClientRectangle.Width - scheduleTable.Left;
            scheduleTable.Height = tabPage_Schedule.ClientRectangle.Height - scheduleTable.Top;
        }

        struct Time
        {
            public int hour;
            public int minute;

            public override bool Equals(object obj)
            {
                if(obj.GetType() == typeof(Time))
                {
                    var time = (Time)obj;
                    return time.hour == hour && time.minute == minute;
                }

                return false;
            }

            public static bool operator ==(Time a,Time b)
            {
                return a.Equals(b);
            }

            public static bool operator !=(Time a,Time b)
            {
                return !a.Equals(b);
            }

            public static bool operator <(Time a,Time b)
            {
                return (a.hour < b.hour) || (a.hour == b.hour && a.minute < b.minute);
            }

            public static bool operator >(Time a,Time b)
            {
                return (a != b) && !(a < b);
            }

            public static bool operator <=(Time a,Time b)
            {
                return a < b || a == b;
            }

            public static bool operator >=(Time a,Time b)
            {
                return a > b || a == b;
            }

            public static implicit operator Time(DateTime dateTime)
            {
                return new Time{hour = dateTime.Hour,minute = dateTime.Minute};
            }

            public override int GetHashCode()
            {
                return hour * 100 + minute;
            }

            public override string ToString()
            {
                return Global.GetRightSubString("00" + hour,2) + ":" + Global.GetRightSubString("00" + minute,2);
            }
        }

        class TimeRange
        {
            public Time start;
            public Time end;
        }

        private void AddNewPointToTimeRangeLinkedList(LinkedList<TimeRange> list,Time time)
        {
            for(var curNode = list.First;curNode != null;curNode = curNode.Next)
            {
                if(curNode.Value.start == time)
                {
                    return;
                }
                else if(curNode.Value.start < time && time < curNode.Value.end)
                {
                    var newNode = new TimeRange{start = time,end = curNode.Value.end};
                    curNode.Value.end = time;
                    list.AddAfter(curNode,newNode);
                    return;
                }
            }

            throw new Exception("未知异常！");
        }

        class EventScheduleUnit
        {
            public Event parent;
            public int row;
            public int rowNum;
            public bool Nil;
        }

        private void button_Schedule_Search_Click(object sender,EventArgs e)
        {
            var eventList = eventInfoService.GetAllList(DateTime.MinValue,DateTime.MaxValue);
            var rowLabel = new LinkedList<TimeRange>();

            var curTime = DateTime.Now;
            var curTimeRow = -1;
            var curTimeCol = -1;
            var startDate = dateTimePicker_Schedule_StartDate.Value;
            startDate = new DateTime(startDate.Year,startDate.Month,startDate.Day,0,0,0);
            var endDate = dateTimePicker_Schedule_EndDate.Value;
            endDate = new DateTime(endDate.Year,endDate.Month,endDate.Day,23,59,59);
            var ts = endDate - startDate;
            var diffDays = (int)Math.Ceiling(ts.TotalDays);
            var scheduleList = new List<EventScheduleUnit>[diffDays];
            var curDate = startDate;

            rowLabel.AddLast(new TimeRange{start = new Time{hour = 0,minute = 0},end = new Time{hour = 24,minute = 0}});

            for(var i = 0;i < diffDays;i++,curDate = curDate.AddDays(1))
            {
                if(curDate.Year == curTime.Year && curDate.Month == curTime.Month && curDate.Day == curTime.Day)
                {
                    curTimeCol = i;
                }

                foreach(var item in eventList)
                {
                    if(item.enabled)
                    {
                        var fm = new FormulaManager(item.remindFormula);

                        if(fm.GetStartDate() <= curDate && curDate <= fm.GetEndDate() && fm.CheckDate(curDate))
                        {
                            AddNewPointToTimeRangeLinkedList(rowLabel,item.startTime);
                            AddNewPointToTimeRangeLinkedList(rowLabel,item.endTime);
                        }
                    }
                }
            }

            var rowLabelList = rowLabel.ToList();
            var startTimeMap = new Dictionary<Time,int>();
            var endTimeMap = new Dictionary<Time,int>();

            for(var i = 0;i < rowLabelList.Count;i++)
            {
                if((Time)curTime >= rowLabelList[i].start && (Time)curTime <= rowLabelList[i].end)
                {
                    curTimeRow = i;
                }

                startTimeMap[rowLabelList[i].start] = i;
                endTimeMap[rowLabelList[i].end] = i;
            }

            curDate = startDate;

            try
            {
                for(var i = 0;i < diffDays;i++,curDate = curDate.AddDays(1))
                {
                    scheduleList[i] = new List<EventScheduleUnit>();

                    foreach(var item in eventList)
                    {
                        if(item.enabled)
                        {
                            var fm = new FormulaManager(item.remindFormula);

                            if(fm.GetStartDate() <= curDate && curDate <= fm.GetEndDate() && fm.CheckDate(curDate))
                            {
                                scheduleList[i].Add(new EventScheduleUnit{parent = item,row = startTimeMap[item.startTime],rowNum = endTimeMap[item.endTime] - startTimeMap[item.startTime] + 1,Nil = false});
                            }
                        }
                    }

                    scheduleList[i].OrderBy(x => x.row);
                }
            }
            catch
            {
                
            }

            var tableData = new EventScheduleUnit[rowLabelList.Count,diffDays];

            for(var i = 0;i < rowLabelList.Count;i++)
            {
                for(var j = 0;j < diffDays;j++)
                {
                    tableData[i,j] = new EventScheduleUnit{Nil = true};
                }
            }

            for(var i = 0;i < scheduleList.Length;i++)
            {
                foreach(var item in scheduleList[i])
                {
                    tableData[item.row,i] = item;

                    if(item.rowNum > 1)
                    {
                        for(var j = 1;j < item.rowNum;j++)
                        {
                            tableData[item.row + j,i] = null;
                        }
                    }
                }
            }
            
            var hc = new HTMLConstructor();

            hc.AppendLine("<!DOCTYPE HTML>");
            hc.AddHTMLStartLabel("html");
                hc.AddHTMLStartLabel("head");
                    hc.AddHTMLLabel("meta","charset=\"UTF-8\"");
                    hc.AddHTMLLabel("link",$"rel=\"stylesheet\" href=\"local://local/bootstrap.min.css\"");
                    hc.AddHTMLStartLabel("script",$"src=\"local://local/jquery-3.5.1.min.js\"");
                    hc.AddHTMLEndLabel("script");
                    hc.AddHTMLStartLabel("script",$"src=\"local://local/bootstrap.min.js\"");
                    hc.AddHTMLEndLabel("script");
                    hc.AddHTMLStartLabel("style","type=\"text/css\"");
                        hc.AppendLine("th,td{text-align:center;vertical-align:middle!important;}.EnabledEvent:Hover{background-color:#ABC797;cursor:pointer;}.EnabledEvent{background-color:#5BC0DE;cursor:normal;}.ActiveEvent:Hover{background-color:#ABC797;cursor:pointer;}.ActiveEvent{background-color:#EFAD4D;cursor:normal;}.ActiveCell{background-color:#EFAD4D;cursor:normal;}");
                    hc.AddHTMLEndLabel("style");
                hc.AddHTMLEndLabel("head");
                hc.AddHTMLStartLabel("body");
                    hc.AddHTMLStartLabel("table","class=\"table table-bordered\"");
                        hc.AddHTMLStartLabel("thead");
                            hc.AddHTMLStartLabel("tr");
                                hc.AddHTMLStartLabel("th");
                                hc.AddHTMLEndLabel("th");

                                curDate = startDate;

                                for(var i = 0;i < diffDays;i++,curDate = curDate.AddDays(1))
                                {
                                    hc.AddHTMLStartLabel("th");
                                        hc.AppendLine(curDate.ToString("yyyy/MM/dd ddd"));
                                    hc.AddHTMLEndLabel("th");
                                }

                            hc.AddHTMLEndLabel("tr");
                        hc.AddHTMLEndLabel("thead");           
                        hc.AddHTMLStartLabel("tbody");

                            for(var i = 0;i < rowLabelList.Count;i++)
                            {
                                hc.AddHTMLStartLabel("tr");
                                    hc.AddHTMLStartLabel("td");

                                        if(rowLabelList[i].start == rowLabelList[i].end)
                                        {
                                            hc.AppendLine(rowLabelList[i].start.ToString());
                                        }
                                        else
                                        {
                                            hc.AppendLine("[" + rowLabelList[i].start.ToString() + "," + rowLabelList[i].end.ToString() + ")");
                                        }

                                    hc.AddHTMLEndLabel("td");

                                    for(var j = 0;j < diffDays;j++)
                                    {
                                        var item = tableData[i,j];

                                        if(item != null)
                                        {
                                            if(item.Nil)
                                            {
                                                hc.AddHTMLStartLabel("td",(curTimeRow == i && curTimeCol == j ? "class=\"ActiveCell\"" : ""));
                                            }
                                            else if(item.rowNum == 1)
                                            {
                                                hc.AddHTMLStartLabel("td",$"class=\"{(curTimeRow == i && curTimeCol == j ? "ActiveEvent" : "EnabledEvent")}\" onclick=\"javascript:global.viewEvent({item.parent.id});\"");
                                            }
                                            else
                                            {
                                                hc.AddHTMLStartLabel("td",$"class=\"{(curTimeRow == i && curTimeCol == j ? "ActiveEvent" : "EnabledEvent")}\" rowspan=\"{item.rowNum}\" onclick=\"javascript:global.viewEvent({item.parent.id});\"");
                                            }

                                                hc.AppendLine(item.Nil ? "" : item.parent.title);
                                            hc.AddHTMLEndLabel("td");
                                        }
                                    }

                                hc.AddHTMLEndLabel("tr");
                            }

                        hc.AddHTMLEndLabel("tbody");
                    hc.AddHTMLEndLabel("table");
                hc.AddHTMLEndLabel("body");
            hc.AddHTMLEndLabel("html");
            
            CefSharpSchemeHandler.scheduleTableHTML = hc.ToString();
            scheduleTable.Load("local://local/scheduleTable.html");
        }

        private void Form_Main_FormClosed(object sender,FormClosedEventArgs e)
        {
            Cef.Shutdown();

            if(RemindThread != null)
            {
                RemindThreadMsgInQueue.Enqueue(new ThreadControlMsg{cmd = ThreadControlCmd.Stop});
                var tick = Environment.TickCount;
                while(RemindThreadMsgOutQueue.Count == 0 && (Environment.TickCount - tick) < 2000);
            }

            Environment.Exit(0);
        }

        public void viewEvent(int id)
        {
            if(InvokeRequired)
            {
                Invoke(new Action(() => viewEvent(id)));
            }
            else
            {
                var item = eventInfoService.GetInfo(id);
            
                if(item == null)
                {
                    Global.Error("该事件不存在！");
                    return;
                }

                var stb = new StringBuilder();
                stb.AppendLine($"事件ID：{item.id}");
                stb.AppendLine($"事件名称：{item.title}");
                stb.AppendLine($"事件描述：{item.description}");
                stb.AppendLine($"事件状态：{(item.enabled ? "已启用" : "已禁用")}");
                stb.AppendLine($"事件发生时段：{item.startTime.ToString("HH:mm")} - {item.endTime.ToString("HH:mm")}");
                stb.AppendLine($"事件创建时间：{item.createdTime}");
                stb.AppendLine($"事件类型：{(item.type == Event.Type.GenericEvent ? "一般事件" : "从任务派生的事件")}");
            
                if(item.type == Event.Type.Task)
                {
                    stb.AppendLine($"任务ID：{item.taskId}");
                    var hasChild = false;
                    var task = taskInfoService.GetInfo(item.taskId,out hasChild);

                    if(task == null)
                    {
                        stb.AppendLine("错误：该任务不存在！");
                    }
                    else
                    {
                        stb.AppendLine($"任务名称：{task.title}");
                        stb.AppendLine($"任务描述：{task.description}");

                        if(!hasChild)
                        {
                            stb.AppendLine($"进度：{task.finishedProgress}{task.progressUnit}/{task.totalProgress}{task.progressUnit}");
                        }

                        stb.AppendLine($"进度百分比：{Math.Round(task.finishedProgress * 100.0d / task.totalProgress)}%");
                        stb.AppendLine($"创建时间：{task.createdTime}");
                    }
                }

                new Form_Info(stb.ToString(),false,null,null,false,null,null).ShowDialog(this);
            }
        }

        private void RemindThreadEntry()
        {
            var running = false;
            var stopped = false;
            var db = DbUtility.GetThreadInstance();

            try
            {
                db.Init(Global.connectionString,DbProviderType.MySql);
                db.Open();
            }
            catch
            {
                Invoke(new Action(() => {Global.Error("线程数据库连接失败！");Environment.Exit(-1);}));
                return;
            }

            var eventInfoService = new EventInfoService(db);
            var lastTime = DateTime.Now.AddMinutes(-1);

            while(!stopped)
            {
                if(RemindThreadMsgInQueue.Count > 0)
                {
                    ThreadControlMsg msg;

                    if(RemindThreadMsgInQueue.TryDequeue(out msg))
                    {
                        switch(msg.cmd)
                        {
                            case ThreadControlCmd.Start:
                                running = true;
                                RemindThreadMsgOutQueue.Enqueue(new ThreadControlMsg{cmd = ThreadControlCmd.Started});
                                break;

                            case ThreadControlCmd.Pause:
                                running = false;
                                RemindThreadMsgOutQueue.Enqueue(new ThreadControlMsg{cmd = ThreadControlCmd.Paused});
                                break;

                            case ThreadControlCmd.Stop:
                                running = false;
                                stopped = true;
                                RemindThreadMsgOutQueue.Enqueue(new ThreadControlMsg{cmd = ThreadControlCmd.Stopped});
                                break;
                        }
                    }
                }

                var curTime = DateTime.Now;

                if(running && (curTime.Year != lastTime.Year || curTime.Month != lastTime.Month || curTime.Day != lastTime.Day || curTime.Hour != lastTime.Hour || curTime.Minute != lastTime.Minute))
                {
                    lastTime = curTime;
                    var eventList = eventInfoService.GetAllList(DateTime.MinValue,DateTime.MaxValue);                    
                    var startDate = DateTime.Now;
                    startDate = new DateTime(startDate.Year,startDate.Month,startDate.Day,0,0,0);
                    var endDate = DateTime.Now;
                    endDate = new DateTime(endDate.Year,endDate.Month,endDate.Day,23,59,59);
                    var ts = endDate - startDate;
                    var diffDays = (int)Math.Ceiling(ts.TotalDays);
                    var curDate = startDate;
                    Event foundEvent = null;

                    try
                    {
                        for(var i = 0;i < diffDays;i++,curDate = curDate.AddDays(1))
                        {
                            foreach(var item in eventList)
                            {
                                if(item.enabled)
                                {
                                    try
                                    {
                                        var fm = new FormulaManager(item.remindFormula);

                                        if(fm.GetStartDate() <= curDate && curDate <= fm.GetEndDate() && fm.CheckDate(curDate))
                                        {
                                            if((Time)item.startTime == (Time)curTime)
                                            {
                                                foundEvent = item;
                                                goto next;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                
                    }

                    next:
                        if(foundEvent != null)
                        {
                            Invoke(new Action(() => {var sp = new SoundPlayer();sp.SoundLocation = @"Resource\remind.wav";sp.PlayLooping();viewEvent(foundEvent.id);sp.Stop();}));
                        }
                }

                Thread.Sleep(100);
            }
        }

        private void checkBox_Schedule_Remind_CheckedChanged(object sender,EventArgs e)
        {
            if(checkBox_Schedule_Remind.Checked)
            {
                RemindThreadMsgInQueue.Enqueue(new ThreadControlMsg{cmd = ThreadControlCmd.Start});
                while(RemindThreadMsgOutQueue.Count == 0);
                ThreadControlMsg msg;

                while(!RemindThreadMsgOutQueue.TryDequeue(out msg))
                {
                    Thread.Sleep(20);
                }

                if(msg.cmd != ThreadControlCmd.Started)
                {
                    Global.Error("提醒线程启动出现重大故障！");
                    Environment.Exit(-1);
                }
            }
            else
            {
                RemindThreadMsgInQueue.Enqueue(new ThreadControlMsg{cmd = ThreadControlCmd.Pause});
                while(RemindThreadMsgOutQueue.Count == 0);
                ThreadControlMsg msg;

                while(!RemindThreadMsgOutQueue.TryDequeue(out msg))
                {
                    Thread.Sleep(20);
                }

                if(msg.cmd != ThreadControlCmd.Paused)
                {
                    Global.Error("提醒线程暂停出现重大故障！");
                    Environment.Exit(-1);
                }
            }
        }
    }
}