using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.ButtonPanel;
using DevExpress.XtraCharts;

namespace PostRig2._0
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        static readonly double roadCarCornerWeight = 400.0;
        static readonly double roadCarSpringStiffness = 80000.0;
        static readonly double roadCarDampingCoefficient = 4000.0;

        static readonly double raceCarCornerWeight = 250.0;
        static readonly double raceCarSpringStiffness = 120000.0;
        static readonly double raceCarDampingCoefficient = 11000.0;

        static readonly double rallyCarCornerWeight = 175.0;
        static readonly double rallyCarSpringStiffness = 150000.0;
        static readonly double rallyCarDampingCoefficient = 16000.0;

        private bool NewCarBuilt = false;

        private bool NewSimSetup = false;
        private bool SingleStepIP = false;
        private bool MultipleStepIP = false;


        public Document Doc { get; set; }

        private void CheckButtonById(string caption)
        {
            foreach (IBaseButton Button in VehicleTemplateButtonPanel.Buttons)
            {
                if (Button.Properties.Caption == caption)
                {
                    Button.Properties.Checked = true;
                }
                else
                {
                    Button.Properties.Checked = false;
                }
            }
        }

        public void ValidateVehicleTemplateCheckedButton()
        {
            double cornerWeight = Doc.Input.VehicleMass;
            double springStiffness = Doc.Input.SpringStiffness;
            double dampingCoefficient = Doc.Input.DampingCoefficient;

            if (cornerWeight == roadCarCornerWeight && springStiffness == roadCarSpringStiffness && dampingCoefficient == roadCarDampingCoefficient)
            {
                CheckButtonById("Road Car");
            }

            else if (cornerWeight == raceCarCornerWeight && springStiffness == raceCarSpringStiffness && dampingCoefficient == raceCarDampingCoefficient)
            {
                CheckButtonById("Race Car");
            }

            else if (cornerWeight == rallyCarCornerWeight && springStiffness == rallyCarSpringStiffness && dampingCoefficient == rallyCarDampingCoefficient)
            {
                CheckButtonById("Rally Car");
            }
            else
            {
                CheckButtonById("Custom Car");
            }
        }

        public void UpdateUIFromDocument()
        {
            if (Doc != null)
            {
                // Display Values in UI from Initialised Value in Input Constructor


                CornerWeightTextBox.Text = Doc.Input.VehicleMass.ToString();

                SpringStiffnessTextBox.Text = Doc.Input.SpringStiffness.ToString();

                DampingCoeffTextBox.Text = Doc.Input.DampingCoefficient.ToString();

                ValidateVehicleTemplateCheckedButton();

                SimSetupValuesColumn.TreeList.Nodes[0].Nodes[0].SetValue(SimSetupValuesColumn, Doc.Input.StartTime);
                SimSetupValuesColumn.TreeList.Nodes[0].Nodes[1].SetValue(SimSetupValuesColumn, Doc.Input.TimeStep);
                SimSetupValuesColumn.TreeList.Nodes[0].Nodes[2].SetValue(SimSetupValuesColumn, Doc.Input.EndTime);

                SimSetupValuesColumn.TreeList.Nodes[1].Nodes[0].SetValue(SimSetupValuesColumn, Doc.Input.StepStartTime);
                SimSetupValuesColumn.TreeList.Nodes[1].Nodes[1].SetValue(SimSetupValuesColumn, Doc.Input.StepLength);
                SimSetupValuesColumn.TreeList.Nodes[1].Nodes[2].SetValue(SimSetupValuesColumn, Doc.Input.IntervalBetweenSteps);
                SimSetupValuesColumn.TreeList.Nodes[1].Nodes[3].SetValue(SimSetupValuesColumn, Doc.Input.StepAmplitude);



            }
        }

        public void UpdateDocumentFromUI()
        {
            if (Doc != null)
            {
                // Set Properties from Data input by the user

                string strval;

                double dblVal = -1.0;


                bool Error = false;

                if (double.TryParse(CornerWeightTextBox.Text, out dblVal))
                {
                    Doc.Input.VehicleMass = dblVal;
                }
                else
                {
                    Error = true;
                }

                if (double.TryParse(SpringStiffnessTextBox.Text, out dblVal))
                {
                    Doc.Input.SpringStiffness = dblVal;
                }
                else
                {
                    Error = true;
                }

                if (double.TryParse(DampingCoeffTextBox.Text, out dblVal))
                {
                    Doc.Input.DampingCoefficient = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[0].Nodes[0].GetValue(SimSetupValuesColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.StartTime = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[0].Nodes[1].GetValue(SimSetupValuesColumn).ToString();

                if(double.TryParse(strval, out dblVal))
                {
                    Doc.Input.TimeStep = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[0].Nodes[2].GetValue(SimSetupValuesColumn).ToString();

                if(double.TryParse(strval, out dblVal))
                {
                    Doc.Input.EndTime = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[1].Nodes[0].GetValue(SimSetupValuesColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.StepStartTime = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[1].Nodes[1].GetValue(SimSetupValuesColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.StepLength = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[1].Nodes[2].GetValue(SimSetupValuesColumn).ToString();

                if(double.TryParse(strval, out dblVal))
                {
                    Doc.Input.IntervalBetweenSteps = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[1].Nodes[3].GetValue(SimSetupValuesColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.StepAmplitude = dblVal;
                }
                else
                {
                    Error = true;
                }




                if (Error)
                {
                    UpdateUIFromDocument();
                    MessageBox.Show("One of the input values is incorrect. Resetting to previous values");
                }

                ValidateVehicleTemplateCheckedButton();
            }
        }

        public void UpdateResultsFromDocument()
        {
            if (Doc != null)
            {
                // Display Results of Derived Parameters in Input Class
            }
        }

        // Initialize Software

        public MainForm()
        {
            InitializeComponent();

            VersionLabel.Text = ProductVersion.ToString();
            SimSetupTreeList.ExpandAll();

        }


        // New File Creation

        private void NewFileRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Doc = new Document();

            UpdateUIFromDocument();

            FileRibbonBasePanel.Visible = false;

            DesignRibbonPage.Visible = true;


            DesignRibbonBasePanel.Visible = true;
            SimSetupRibbonPage.Visible = true;



            MainFormRibbonControl.SelectedPage = DesignRibbonPage;


        }


        // Open Existing File

        private void OpenFileRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = "C:\\";

            dlg.Filter = "PostRig Files (*.postrig)|*.postrig";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Doc = new Document(dlg.FileName);
            }

            if (Doc != null)
            {
                UpdateUIFromDocument();

                FileRibbonBasePanel.Visible = false;

                DesignRibbonPage.Visible = true;

                DesignRibbonBasePanel.Visible = true;
                SimSetupRibbonPage.Visible = true;

                MainFormRibbonControl.SelectedPage = DesignRibbonPage;
            }
        }


        // Save File

        private void SaveFileRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Doc != null && !string.IsNullOrWhiteSpace(Doc.FileName))
            {
                Doc.Save();
            }
            else if (Doc != null)
            {
                SaveAsFileRibbonBarButton_ItemClick(sender, e);
            }
        }


        // Save As

        private void SaveAsFileRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Doc != null)
            {
                SaveFileDialog dlg = new SaveFileDialog();

                dlg.InitialDirectory = "C:\\";

                dlg.Filter = "PostRig Files|*.postrig";

                dlg.AddExtension = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Doc.SaveAs(dlg.FileName);
                }
            }
        }


        // Close Program

        private void CloseFileRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do You Really Want To Close The Program?", "Exit", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void AboutFileRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FileRibbonBasePanel.Visible = true;

            FileRibbonBasePanel.BringToFront();
        }

        private void OnRibbonPageChanged(object sender, EventArgs e)
        {
            if (NewCarBuilt)
            {
                if (MainFormRibbonControl.SelectedPage == DesignRibbonPage)
                {
                    ShowDesignPanelBarButton_ItemClick(sender, null);
                }

                else if (NewSimSetup)
                {
                    if (MainFormRibbonControl.SelectedPage == SimSetupRibbonPage)
                    {
                        SimulationSetupBasePanel.Visible = true;

                        SimulationSetupBasePanel.BringToFront();
                    }
                }
            }

        }


        // Design Ribbon Panel Actions

        private void BuildCarHomeRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            VehicleDataHomeRibbonPanel.Visible = true;

            VehicleDataModelPanel.Visible = true;

            NewCarBuilt = true;
        }


        private void OnTextInputChanged(object sender, EventArgs e)
        {
            UpdateDocumentFromUI();
        }


        private void VehicleTemplateButtonPanel_ButtonChecked(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            foreach (IBaseButton button in VehicleTemplateButtonPanel.Buttons)
            {
                if (button == e.Button)
                {
                    button.Properties.Checked = true;
                }
                else
                {
                    button.Properties.Checked = false;
                }
            }


            if (e.Button.Properties.Caption == "Road Car")
            {
                Doc.Input.VehicleMass = roadCarCornerWeight;
                Doc.Input.SpringStiffness = roadCarSpringStiffness;
                Doc.Input.DampingCoefficient = roadCarDampingCoefficient;

            }
            else if (e.Button.Properties.Caption == "Race Car")
            {
                Doc.Input.VehicleMass = raceCarCornerWeight;
                Doc.Input.SpringStiffness = raceCarSpringStiffness;
                Doc.Input.DampingCoefficient = raceCarDampingCoefficient;
            }
            else if (e.Button.Properties.Caption == "Rally Car")
            {
                Doc.Input.VehicleMass = rallyCarCornerWeight;
                Doc.Input.SpringStiffness = rallyCarSpringStiffness;
                Doc.Input.DampingCoefficient = rallyCarDampingCoefficient;
            }

            UpdateUIFromDocument();
        }

        private void ShowDesignPanelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DesignRibbonBasePanel.Visible = true;

            DesignRibbonBasePanel.BringToFront();
        }

        // Simulation Setup Panel Actions


        private void NewSimSetupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SimulationSetupBasePanel.Visible = true;

            SimulationSetupBasePanel.BringToFront();

            SimSetupParametersColumn.TreeList.Nodes[1].Collapse();
            SimSetupParametersColumn.TreeList.Nodes[1].Visible = false;

            NewSimSetup = true;
        }

        

        private void SingleStepIPCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (SingleStepIPCheckItem.Checked)
            {
                SingleStepIP = true;
                MultipleStepIP = false;

                if (SingleStepIP)
                {
                    SimSetupParametersColumn.TreeList.Nodes[1].ExpandAll();
                    SimSetupParametersColumn.TreeList.Nodes[1].Visible = true;

                    SimSetupValuesColumn.TreeList.Nodes[1].Nodes[0].Visible = true;
                    SimSetupValuesColumn.TreeList.Nodes[1].Nodes[1].Visible = false;
                    SimSetupValuesColumn.TreeList.Nodes[1].Nodes[2].Visible = false;
                    SimSetupValuesColumn.TreeList.Nodes[1].Nodes[3].Visible = true;
                }

                
            }
            
        }

        private void MultipleStepIPCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (MultipleStepIPCheckItem.Checked)
            {
                SingleStepIP = false;
                MultipleStepIP = true;

                if (MultipleStepIP)
                {
                    SimSetupParametersColumn.TreeList.Nodes[1].ExpandAll();
                    SimSetupParametersColumn.TreeList.Nodes[1].Visible = true;

                    SimSetupValuesColumn.TreeList.Nodes[1].Nodes[0].Visible = true;
                    SimSetupValuesColumn.TreeList.Nodes[1].Nodes[1].Visible = true;
                    SimSetupValuesColumn.TreeList.Nodes[1].Nodes[2].Visible = true;
                    SimSetupValuesColumn.TreeList.Nodes[1].Nodes[3].Visible = true;
                }

                
            }
            
        }

        private void SingleStepIPCheckItem_OnClick(object sender, ItemClickEventArgs e)
        {
            SingleStepIPCheckItem.Checked = true;
            MultipleStepIPCheckItem.Checked = false;


        }

        private void MultipleStepIPCheckItem_OnClick(object sender,ItemClickEventArgs e)
        {
            SingleStepIPCheckItem.Checked = false;
            MultipleStepIPCheckItem.Checked = true; ;
        }

        private void InitialiseSimSetupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateDocumentFromUI();

            if (SingleStepIP)
            {
                Doc.Input.SingleStepIPNeedsToRecalculate = true;
                Doc.Input.MultipleStepIPNeedsToRecalculate = false;
            }
            else if (MultipleStepIP)
            {
                Doc.Input.SingleStepIPNeedsToRecalculate = false;
                Doc.Input.MultipleStepIPNeedsToRecalculate = true;
            }



            Doc.Input.InputDataCalculate();
        }

        private void InputSignalSimSetupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(SingleStepIP || MultipleStepIP)
            {
                StepSignalChartControl.Dock = DockStyle.Fill;
                StepSignalChartControl.Visible = true;

                Series StepFunction = new Series("Displacement", ViewType.Line);

                StepSignalChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.TimeIntervals.Count; i++)
                {
                    StepFunction.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.StepInput[i]));
                }

                StepSignalChartControl.Series.Add(StepFunction);

                XYDiagram diagram = (XYDiagram)StepSignalChartControl.Diagram;

                //diagram.AxisX.WholeRange.MinValue = Doc.Input.StartTime;
                diagram.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisX.Alignment = AxisAlignment.Near;
                diagram.AxisX.Title.Text = "Time (s)";
                diagram.AxisX.Title.TextColor = Color.Black;
                diagram.AxisX.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisX.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);

                //diagram.AxisY.WholeRange.MinValue = Doc.Input.StartTime;
                diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Alignment = AxisAlignment.Near;
                diagram.AxisY.Title.Text = "Displacement (m)";
                diagram.AxisY.Title.TextColor = Color.Black;
                diagram.AxisY.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);

                StepSignalChartControl.Update();
            }
            
        }

        private void UpdateSimSetupPlotBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            InputSignalSimSetupBarButton_ItemClick(sender, e);
        }
    }
}
