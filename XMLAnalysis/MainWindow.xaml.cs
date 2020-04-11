using System;
using System.Collections.Generic;
using System.Linq;
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
using Jxnflzc.XMLClassDiagram;

namespace XMLAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String fileName = "";
        private List<ClassDiagram> cdList = null;
        private List<Operation> opList = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListInit()
        {
            this.list_ck.Items.Clear();
            this.list_attribute.Items.Clear();
            this.list_operation.Items.Clear();
            this.list_parameter.Items.Clear();
        }

        private ClassDiagram GetClassDiagramByName(String name)
        {
            foreach (ClassDiagram cd in this.cdList)
            {
                if (cd.GetName() == name)
                {
                    return cd;
                }
            }
            return null;
        }

        private Operation GetOperationByName(String name)
        {
            foreach (Operation op in this.opList)
            {
                if (op.GetName() == name)
                {
                    return op;
                }
            }
            return null;
        }

        private void ChooseFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PowerDesigner类图文件|*.xml;*.oom;*.oob"
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                this.fileName = dlg.FileName;
                this.txt_fileName.Text = this.fileName;
            }
        }

        private void Analyze(object sender, RoutedEventArgs e)
        {
            this.list_class.Items.Clear();

            try
            {
                this.cdList = new List<ClassDiagram>();
                this.cdList = XMLUtil.Analyze(txt_fileName.Text);

                foreach (ClassDiagram cd in this.cdList)
                {
                    this.list_class.Items.Add(cd.GetName());
                }
            }
            catch
            {
                MessageBox.Show("XML分析失败，请检查文件是否正确", "错误");
            }
        }

        private void ClassSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListInit();
            opList = new List<Operation>();

            ClassDiagram cd = GetClassDiagramByName((String)this.list_class.SelectedItem);

            if (cd != null)
            {
                this.list_ck.Items.Add("Dit: " + cd.GetDit());
                this.list_ck.Items.Add("Cbo: " + cd.GetCbo());
                this.list_ck.Items.Add("Noc: " + cd.GetNoc());

                foreach (String attribute in cd.GetAttributes())
                {
                    this.list_attribute.Items.Add(attribute);
                }

                foreach (Operation operation in cd.GetOperations())
                {
                    this.list_operation.Items.Add(operation.GetName());
                    opList.Add(operation);
                }
            }
        }

        private void OperationSelectChanged(object sender, SelectionChangedEventArgs e)
        {
            this.list_parameter.Items.Clear();

            Operation op = GetOperationByName((String)this.list_operation.SelectedItem);

            if (op != null)
            {
                foreach (String parameter in op.GetParameters())
                {
                    this.list_parameter.Items.Add(parameter);
                }
            }
        }

        private void FileDrop(object sender, DragEventArgs e)
        {
            String file = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (file.ToUpper().EndsWith(".OOM") || file.ToUpper().EndsWith(".OOB") || file.ToUpper().EndsWith(".XML"))
            {
                this.txt_fileName.Text = file;
            }
            else
            {
                MessageBox.Show("不是正确的文件类型", "错误");
            }
        }
    }
}
