using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 个人日程管理.Service;

namespace 个人日程管理
{
    public partial class Form_TaskLinker : Form
    {
        private TaskInfoService taskInfoService = new TaskInfoService();
        
        public DialogResult result = DialogResult.Cancel;
        public int taskId = -1;

        public Form_TaskLinker(int id)
        {
            InitializeComponent();
            UpdateTaskList();

            if(id >= 0)
            {
                taskId = id;
                
                if(!SelectTask(new Model.Task{id = id}))
                {
                    taskId = -1;
                }
            }
            else
            {
                taskId = -1;
            }
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

        private bool SelectTask(Model.Task task,bool expandDirectChild = false)
        {
            if(task != null)
            {
                return SelectTask_DFS(task,treeView_Task_Dir.Nodes,null,expandDirectChild);
            }

            return false;
        }

        private void button_Link_Click(object sender,EventArgs e)
        {
            if(treeView_Task_Dir.SelectedNode != null && treeView_Task_Dir.SelectedNode.Tag != null)
            {
                taskId = (treeView_Task_Dir.SelectedNode.Tag as Model.Task).id;
                result = DialogResult.Yes;
                Close();
            }
        }

        private void button_Unlink_Click(object sender,EventArgs e)
        {
            result = DialogResult.No;
            Close();
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
    }
}
