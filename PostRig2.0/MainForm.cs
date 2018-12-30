using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.ButtonPanel;
using DevExpress.XtraCharts;

namespace PostRig2._0
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        // Initialize Software
        public MainForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(ClosingEvent);
            VersionLabel.Text = ProductVersion.ToString();
            SimSetupTreeList.ExpandAll();

            LoadFilePaths();

        }

        static readonly double roadCarCornerWeight = 400.0;
        static readonly double roadCarSpringStiffness = 80000.0;
        static readonly double roadCarDampingCoefficient = 4000.0;

        static readonly double raceCarCornerWeight = 400.0;
        static readonly double raceCarSpringStiffness = 120000.0;
        static readonly double raceCarDampingCoefficient = 13850.0;

        static readonly double rallyCarCornerWeight = 400.0;
        static readonly double rallyCarSpringStiffness = 150000.0;
        static readonly double rallyCarDampingCoefficient = 17050.0;

        public bool NewCarBuilt { get; set; }
        public bool NewSimSetup { get; set; }
        public bool ViewResults { get; set; }

        public bool SingleStepIP { get; set; }
        public bool MultipleStepIP { get; set; }
        public bool CustomIP { get; set; }

        private bool RunSuccess = false;

        public string OpenFilePath { get; set; } = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        public string SaveFilePath { get; set; } = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        public string CSVFilePath { get; set; } = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

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

        public void LoadFilePaths()
        {
            string OptionsFile = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\options.txt";

            if (System.IO.File.Exists(OptionsFile))
            {
                string[] AllPaths = System.IO.File.ReadAllLines(OptionsFile);

                for (int i = 0; i < AllPaths.Length; i++)
                {
                    string path = AllPaths[i];
                    if (System.IO.Directory.Exists(path))
                    {
                        switch (i)
                        {
                            case 0:
                                OpenFilePath = path;
                                break;
                            case 1:
                                SaveFilePath = path;
                                break;
                            case 2:
                                CSVFilePath = path;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public void SaveFilePaths()
        {
            string OptionsFile = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\options.txt";

            if (System.IO.File.Exists(OptionsFile))
            {
                System.IO.File.Delete(OptionsFile);
            }

            List<string> AllPaths = new List<string>();

            if (System.IO.Directory.Exists(OpenFilePath))
            {
                AllPaths.Add(OpenFilePath);
            }

            if (System.IO.Directory.Exists(SaveFilePath))
            {
                AllPaths.Add(SaveFilePath);
            }

            if (System.IO.Directory.Exists(CSVFilePath))
            {
                AllPaths.Add(CSVFilePath);
            }

            System.IO.File.WriteAllLines(OptionsFile, AllPaths.ToArray());
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

                SimSetupValuesColumn.TreeList.Nodes[2].Nodes[0].SetValue(SimSetupValuesColumn, Doc.Input.StepAmplitude);
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

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.TimeStep = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[0].Nodes[2].GetValue(SimSetupValuesColumn).ToString();

                if (double.TryParse(strval, out dblVal))
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

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.IntervalBetweenSteps = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[2].Nodes[0].GetValue(SimSetupValuesColumn).ToString();

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
                ValuesTreeListColumn.TreeList.Nodes[0].SetValue(ValuesTreeListColumn, Doc.Input.NaturalFrequencyHz);
                ValuesTreeListColumn.TreeList.Nodes[1].SetValue(ValuesTreeListColumn, Doc.Input.CriticalDamping);
                ValuesTreeListColumn.TreeList.Nodes[2].SetValue(ValuesTreeListColumn, Doc.Input.DampingRatio);
            }
        }

        // New File Creation
        private void NewFileRibbonBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Doc = new Document(this);

            UpdateUIFromDocument();

            NewCarBuilt = false;
            NewSimSetup = false;
            ViewResults = false;

            SingleStepIP = false;
            CustomIP = false;

            BuildCarHomeRibbonBarButton_ItemClick(null, null);

            HomeRibbonBasePanel.Visible = false;

            DesignRibbonPage.Visible = true;
            SimSetupRibbonPage.Visible = true;
            ResultsRibbonPage.Visible = true;

            DesignRibbonBasePanel.Visible = true;
            MainFormRibbonControl.SelectedPage = DesignRibbonPage;
        }


        // Open Existing File

        private void OpenFileRibbonBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = OpenFilePath;

            dlg.Filter = "PostRig Files (*.postrig)|*.postrig";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Doc = new Document(this, dlg.FileName);
                OpenFilePath = System.IO.Path.GetDirectoryName(dlg.FileName);
            }

            if (Doc != null)
            {
                UpdateUIFromDocument();
                UpdateResultsFromDocument();

                BuildCarHomeRibbonBarButton_ItemClick(null, null);

                NewSimSetup = true;
                ViewResults = true;

                HomeRibbonBasePanel.Visible = false;

                DesignRibbonPage.Visible = true;
                SimSetupRibbonPage.Visible = true;
                ResultsRibbonPage.Visible = true;

                DesignRibbonBasePanel.Visible = true;

                MainFormRibbonControl.SelectedPage = DesignRibbonPage;

                InitialiseSimSetupBarButton_ItemClick(null, null);

                if (!CustomIP)
                {
                    Doc.Input.InputDataCalculate();
                }

                Doc.Input.OutputDataCalculate();

                if (Doc.Input.ResponseCalculationComplete)
                {
                    RunSuccess = true;
                }


            }
        }


        // Save File

        private void SaveFileRibbonBarButton_ItemClick(object sender, ItemClickEventArgs e)
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

        private void SaveAsFileRibbonBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Doc != null)
            {
                SaveFileDialog dlg = new SaveFileDialog();

                dlg.InitialDirectory = SaveFilePath;

                dlg.Filter = "PostRig Files|*.postrig";

                dlg.AddExtension = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Doc.SaveAs(dlg.FileName);
                    SaveFilePath = System.IO.Path.GetDirectoryName(dlg.FileName);
                }
            }
        }

        private void HelpFileRibbonBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            HomeRibbonBasePanel.Visible = true;

            HomeRibbonBasePanel.BringToFront();

            try
            {
                HelpFilePDFViewer.DocumentFilePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Help File.pdf";
            }
            catch { }

            HelpFilePDFViewer.Dock = DockStyle.Fill;

            HelpFilePDFViewer.Visible = true;
            HelpFilePDFViewer.BringToFront();

        }


        private void CaseStudyFileRibbonGroup_ItemClick(object sender, ItemClickEventArgs e)
        {
            HomeRibbonBasePanel.Visible = true;

            HomeRibbonBasePanel.BringToFront();

            try
            {
                CaseStudyPDFViewer.DocumentFilePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Case Study.pdf";
            }
            catch { }
            CaseStudyPDFViewer.Dock = DockStyle.Fill;

            CaseStudyPDFViewer.Visible = true;
            CaseStudyPDFViewer.BringToFront();
        }


        // Close Program

        private void CloseFileRibbonBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do You Really Want To Close The Program?", "Exit", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Application.Exit();
            }
        }


        private void ClosingEvent(object sender, FormClosingEventArgs e)
        {
            SaveFilePaths();
        }

        private void AboutFileRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HelpFilePDFViewer.Visible = false;
            CaseStudyPDFViewer.Visible = false;

            HomeRibbonBasePanel.Visible = true;

            HomeRibbonBasePanel.BringToFront();
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
                        CheckInputForTreeList();

                        SimulationSetupBasePanel.Visible = true;

                        SimulationSetupBasePanel.BringToFront();
                    }

                    else if (ViewResults)
                    {
                        if (MainFormRibbonControl.SelectedPage == ResultsRibbonPage)
                        {
                            ResultsBasePanel.Visible = true;

                            ResultsBasePanel.BringToFront();
                        }
                    }
                }
            }
        }


        // Design Ribbon Panel Actions

        private void BuildCarHomeRibbonBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            VehicleTemplateHomeRibbonPanel.Visible = true;
            VehicleTemplateHomeRibbonPanel.BringToFront();

            VehicleDataModelPanel.Visible = true;

            CarModelImagePanel.Visible = true;

            SpringDesignDataPanel.Visible = false;
            SpringDesignPanel.Visible = false;

            NewCarBuilt = true;
        }


        private void SpringDesignBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SpringDesignDataPanel.Visible = true;
            SpringDesignPanel.BringToFront();

            SpringDesignPanel.Visible = true;
            SpringDesignPanel.Dock = DockStyle.Left;

        }

        private void OnTextInputChanged(object sender, EventArgs e)
        {
            UpdateDocumentFromUI();
        }


        private void EnterClicked(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateDocumentFromUI();
            }
        }

        

        private void CheckInputForTreeList()
        {
            if (SingleStepIP)
            {
                SimSetupParametersPanel.Visible = true;

                SimSetupValuesColumn.TreeList.Nodes[1].Collapse();
                SimSetupValuesColumn.TreeList.Nodes[1].Visible = false;

                SimSetupValuesColumn.TreeList.Nodes[2].Expand();
                SimSetupValuesColumn.TreeList.Nodes[2].Visible = true;
            }

            if (MultipleStepIP)
            {
                SimSetupParametersPanel.Visible = true;

                SimSetupValuesColumn.TreeList.Nodes[1].Expand();
                SimSetupValuesColumn.TreeList.Nodes[1].Visible = true;

                SimSetupValuesColumn.TreeList.Nodes[2].Expand();
                SimSetupValuesColumn.TreeList.Nodes[2].Visible = true;
            }

            if (CustomIP)
            {
                SimSetupParametersPanel.Visible = false;
            }
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

        

        private void ShowDesignPanelBarButton_ItemClick(object sender, ItemClickEventArgs e)
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

            SimSetupParametersColumn.TreeList.Nodes[2].Collapse();
            SimSetupParametersColumn.TreeList.Nodes[2].Visible = false;

            NewSimSetup = true;
        }



        private void SingleStepIPCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (SingleStepIPCheckItem.Checked)
            {
                SingleStepIP = true;
                MultipleStepIP = false;
                CustomIP = false;

                CustomIPChartControl.Visible = false;

                if (SingleStepIP)
                {
                    //Doc.Input.TimeNeedsToRecalculate = true;
                    //Doc.Input.ResponseNeedsToRecalculate = true;
                    //Doc.Input.CustomIPCalculate = false;
                    //Doc.Input.SingleStepIPNeedsToRecalculate = true;
                    //Doc.Input.MultipleStepIPNeedsToRecalculate = false;

                    SimSetupParametersPanel.Visible = true;

                    SimSetupValuesColumn.TreeList.Nodes[1].Collapse();
                    SimSetupValuesColumn.TreeList.Nodes[1].Visible = false;

                    SimSetupValuesColumn.TreeList.Nodes[2].Expand();
                    SimSetupValuesColumn.TreeList.Nodes[2].Visible = true;
                }


            }

        }

        private void MultipleStepIPCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (MultipleStepIPCheckItem.Checked)
            {
                SingleStepIP = false;
                MultipleStepIP = true;
                CustomIP = false;

                CustomIPChartControl.Visible = false;

                if (MultipleStepIP)
                {
                    //Doc.Input.TimeNeedsToRecalculate = true;
                    //Doc.Input.ResponseNeedsToRecalculate = true;
                    //Doc.Input.CustomIPCalculate = false;
                    //Doc.Input.SingleStepIPNeedsToRecalculate = false;
                    //Doc.Input.MultipleStepIPNeedsToRecalculate = true;

                    SimSetupParametersPanel.Visible = true;

                    SimSetupValuesColumn.TreeList.Nodes[1].Expand();
                    SimSetupValuesColumn.TreeList.Nodes[1].Visible = true;

                    SimSetupValuesColumn.TreeList.Nodes[2].Expand();
                    SimSetupValuesColumn.TreeList.Nodes[2].Visible = true;
                }


            }

        }

        private void CustomIPCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (CustomIPCheckItem.Checked)
            {
                SingleStepIP = false;
                MultipleStepIP = false;
                CustomIP = true;

                if (CustomIP)
                {
                    //Doc.Input.TimeNeedsToRecalculate = false;
                    //Doc.Input.ResponseNeedsToRecalculate = true;
                    //Doc.Input.CustomIPCalculate = true;
                    //Doc.Input.SingleStepIPNeedsToRecalculate = false;
                    //Doc.Input.MultipleStepIPNeedsToRecalculate = false;

                    SimSetupParametersPanel.Visible = false;

                    using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV Files (*.csv)| *.csv", ValidateNames = true, InitialDirectory = CSVFilePath })
                    {
                        if(ofd.ShowDialog()== DialogResult.OK)
                        {
                            Doc.CustomInputExcelRead(ofd.FileName);

                            CSVFilePath = System.IO.Path.GetDirectoryName(ofd.FileName);
                        }

                        else
                        {
                            CustomIP = false;
                        }
                    }

                    

                    InitialiseSimSetupBarButton_ItemClick(sender, e);
                }
            }
        }

        private void SingleStepIPCheckItem_OnClick(object sender, ItemClickEventArgs e)
        {
            SingleStepIPCheckItem.Checked = true;
            MultipleStepIPCheckItem.Checked = false;
            CustomIPCheckItem.Checked = false;
            
        }

        private void MultipleStepIPCheckItem_OnClick(object sender, ItemClickEventArgs e)
        {
            SingleStepIPCheckItem.Checked = false;
            MultipleStepIPCheckItem.Checked = true;
            CustomIPCheckItem.Checked = false;
        }

        private void CustomIPCheckItem_OnClick(object sender, ItemClickEventArgs e)
        {
            SingleStepIPCheckItem.Checked = false;
            MultipleStepIPCheckItem.Checked = false;
            CustomIPCheckItem.Checked = true;
        }

        private void InitialiseSimSetupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateDocumentFromUI();


            if (SingleStepIP)
            {
                Doc.Input.TimeNeedsToRecalculate = true;
                Doc.Input.ResponseNeedsToRecalculate = true;
                Doc.Input.CustomIPCalculate = false;
                Doc.Input.SingleStepIPNeedsToRecalculate = true;
                Doc.Input.MultipleStepIPNeedsToRecalculate = false;
            }

            else if (MultipleStepIP)
            {
                Doc.Input.TimeNeedsToRecalculate = true;
                Doc.Input.ResponseNeedsToRecalculate = true;
                Doc.Input.CustomIPCalculate = false;
                Doc.Input.SingleStepIPNeedsToRecalculate = false;
                Doc.Input.MultipleStepIPNeedsToRecalculate = true;
            }

            else if (CustomIP)
            {
                Doc.Input.TimeNeedsToRecalculate = false;
                Doc.Input.ResponseNeedsToRecalculate = true;
                Doc.Input.CustomIPCalculate = true;
                Doc.Input.SingleStepIPNeedsToRecalculate = false;
                Doc.Input.MultipleStepIPNeedsToRecalculate = false;
            }

            if (!CustomIP)
            {
                Doc.Input.InputDataCalculate();
            }



            UpdateSimSetupPlotBarButton_ItemClick(sender, e);

            //MessageBox.Show("Initialisation Complete.\nReady To Run!");

        }

        private void RunSimSetupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            InitialiseSimSetupBarButton_ItemClick(sender, e);

            Doc.Input.OutputDataCalculate();

            if (Doc.Input.ResponseCalculationComplete)
            {
                RunSuccess = true;
            }

            else if (!Doc.Input.ResponseCalculationComplete)
            {
                RunSuccess = false;
            }



            if (RunSuccess)
            {
                MessageBox.Show("Run Successful");

                UpdateResultsFromDocument();

                ViewResults = true;
            }

            else if (!RunSuccess)
            {
                MessageBox.Show("Run Failed!", "Error", MessageBoxButtons.OK);
            }
        }

        private void InputSignalSimSetupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            

            if (SingleStepIP)
            {
                
                StepSignalChartControl.Dock = DockStyle.Fill;
                StepSignalChartControl.Visible = true;

                //BodyResponseChartControl.Visible = false;

                Series StepFunction = new Series("Displacement", ViewType.Line);

                StepSignalChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.TimeIntervals.Count; i++)
                {
                    StepFunction.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.RoadDisplacement[i]));
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

            else if (MultipleStepIP)
            {
                StepSignalChartControl.Dock = DockStyle.Fill;
                StepSignalChartControl.Visible = true;

                //BodyResponseChartControl.Visible = false;

                Series MultipleStepSeries = new Series("Displacement", ViewType.Line);

                StepSignalChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.TimeIntervals.Count; i++)
                {
                    MultipleStepSeries.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.RoadDisplacement[i]));
                }

                StepSignalChartControl.Series.Add(MultipleStepSeries);

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

            else if (CustomIP)
            {
                StepSignalChartControl.Visible = false;
                CustomIPChartControl.Visible = true;

                CustomIPChartControl.Dock = DockStyle.Fill;

                //BodyResponseChartControl.Visible = false;

                Series CustomIPSeries = new Series("Displacement", ViewType.Spline);

                CustomIPChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.TimeIntervals.Count; i++)
                {
                    CustomIPSeries.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.RoadDisplacement[i]));
                }

                CustomIPChartControl.Series.Add(CustomIPSeries);

                XYDiagram diagram = (XYDiagram)CustomIPChartControl.Diagram;

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

                CustomIPChartControl.Update();
            }

        }

        private void UpdateSimSetupPlotBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            InputSignalSimSetupBarButton_ItemClick(sender, e);
        }

        

        private void SysCharResultsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (RunSuccess)
            {
                UpdateResultsFromDocument();

                ResultsLeftSidePanel.Visible = true;
            }
        }

        private void BodyDisplacementResultsBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ResultsBasePanel.Visible = true;
            ResultsBasePanel.BringToFront();



            if (ViewResults)
            {
                BodyDisplacementChartControl.Visible = true;
                SpringForceChartControl.Visible = false;
                DamperForceChartControl.Visible = false;
                BodyForceChartControl.Visible = false;
                VerticalAccelnChartControl.Visible = false;


                BodyDisplacementChartControl.Dock = DockStyle.Fill;

                //BodyResponseChartControl.Visible = false;

                Series BodyDisplacement = new Series("Body Displacement", ViewType.Spline);

                BodyDisplacementChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.TimeIntervals.Count; i++)
                {
                    BodyDisplacement.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.BodyDisplacement[i]));
                }

                BodyDisplacementChartControl.Series.Add(BodyDisplacement);

                XYDiagram diagram = (XYDiagram)BodyDisplacementChartControl.Diagram;

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

                BodyDisplacementChartControl.Update();
            }
        }

        private void SpringForcePlotBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ResultsBasePanel.Visible = true;
            ResultsBasePanel.BringToFront();

            if (ViewResults)
            {
                BodyDisplacementChartControl.Visible = false;
                SpringForceChartControl.Visible = true;
                DamperForceChartControl.Visible = false;
                BodyForceChartControl.Visible = false;
                VerticalAccelnChartControl.Visible = false;


                SpringForceChartControl.Dock = DockStyle.Fill;

                //BodyResponseChartControl.Visible = false;

                Series SpringForce = new Series("Spring Force", ViewType.Spline);

                SpringForceChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.TimeIntervals.Count; i++)
                {
                    SpringForce.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.SpringForce[i]));
                }

                SpringForceChartControl.Series.Add(SpringForce);

                XYDiagram diagram = (XYDiagram)SpringForceChartControl.Diagram;

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
                diagram.AxisY.Title.Text = "Force (N)";
                diagram.AxisY.Title.TextColor = Color.Black;
                diagram.AxisY.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);

                SpringForceChartControl.Update();
            }
        }

        private void DamperForcePlotBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ResultsBasePanel.Visible = true;
            ResultsBasePanel.BringToFront();

            if (ViewResults)
            {
                BodyDisplacementChartControl.Visible = false;
                SpringForceChartControl.Visible = false;
                DamperForceChartControl.Visible = true;
                BodyForceChartControl.Visible = false;
                VerticalAccelnChartControl.Visible = false;


                DamperForceChartControl.Dock = DockStyle.Fill;

                //BodyResponseChartControl.Visible = false;

                Series DamperForce = new Series("Damper Force", ViewType.Spline);

                DamperForceChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.DamperForce.Count; i++)
                {
                    DamperForce.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.DamperForce[i]));
                }

                DamperForceChartControl.Series.Add(DamperForce);

                XYDiagram diagram = (XYDiagram)DamperForceChartControl.Diagram;

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
                diagram.AxisY.Title.Text = "Force (N)";
                diagram.AxisY.Title.TextColor = Color.Black;
                diagram.AxisY.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);

                DamperForceChartControl.Update();
            }
        }

        private void BodyForcePlotBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ResultsBasePanel.Visible = true;
            ResultsBasePanel.BringToFront();

            if (ViewResults)
            {
                BodyDisplacementChartControl.Visible = false;
                SpringForceChartControl.Visible = false;
                DamperForceChartControl.Visible = false;
                BodyForceChartControl.Visible = true;
                VerticalAccelnChartControl.Visible = false;


                BodyForceChartControl.Dock = DockStyle.Fill;


                Series BodyForce = new Series("Body Force", ViewType.Spline);

                BodyForceChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.TimeIntervals.Count; i++)
                {
                    BodyForce.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.BodyForce[i]));
                }

                BodyForceChartControl.Series.Add(BodyForce);

                XYDiagram diagram = (XYDiagram)BodyForceChartControl.Diagram;

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
                diagram.AxisY.Title.Text = "Force (N)";
                diagram.AxisY.Title.TextColor = Color.Black;
                diagram.AxisY.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);

                BodyForceChartControl.Update();
            }
        }

        private void VerticalAccelnPlotBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            ResultsBasePanel.Visible = true;
            ResultsBasePanel.BringToFront();

            if (ViewResults)
            {
                BodyDisplacementChartControl.Visible = false;
                SpringForceChartControl.Visible = false;
                DamperForceChartControl.Visible = false;
                BodyForceChartControl.Visible = false;
                VerticalAccelnChartControl.Visible = true;


                VerticalAccelnChartControl.Dock = DockStyle.Fill;

                //BodyResponseChartControl.Visible = false;

                Series VerticalAcceln = new Series("Vertical Acceleration", ViewType.Spline);

                VerticalAccelnChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.BodyAcceln.Count; i++)
                {
                    VerticalAcceln.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.BodyAccelnG[i]));
                }

                VerticalAccelnChartControl.Series.Add(VerticalAcceln);

                //XYDiagram diagram = (XYDiagram)VerticalAccelnChartControl.Diagram;

                ////diagram.AxisX.WholeRange.MinValue = Doc.Input.StartTime;
                //diagram.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
                //diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                //diagram.AxisX.Alignment = AxisAlignment.Near;
                //diagram.AxisX.Title.Text = "Time (s)";
                //diagram.AxisX.Title.TextColor = Color.Black;
                //diagram.AxisX.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                //diagram.AxisX.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);

                ////diagram.AxisY.WholeRange.MinValue = Doc.Input.StartTime;
                //diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
                //diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                //diagram.AxisY.Alignment = AxisAlignment.Near;
                //diagram.AxisY.Title.Text = "Vertical Acceleration (G)";
                //diagram.AxisY.Title.TextColor = Color.Black;
                //diagram.AxisY.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                //diagram.AxisY.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);

                VerticalAccelnChartControl.Update();
            }
        }

        
    }
}
