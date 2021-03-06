﻿using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraCharts;
using DevExpress.XtraTreeList;

namespace PostRig2_0
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

        public bool NewCarBuilt { get; set; }
        public bool NewSimSetup { get; set; }
        public bool ViewResults { get; set; }

        public bool SingleStepIP { get; set; }
        public bool HarmonicIP { get; set; }
        public bool CustomIP { get; set; }

        private bool RunSuccess = false;

        public string OpenFilePath { get; set; } = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        public string SaveFilePath { get; set; } = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        public string CSVFilePath { get; set; } = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        public string CarFilePath { get; set; } = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        public Document Doc { get; set; }

        public CarData Car { get; set; }

        public SimData Sim { get; set; }

        public InputData Input { get; set; }

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
                            case 3:
                                CarFilePath = path;
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

            if (System.IO.Directory.Exists(CarFilePath))
            {
                AllPaths.Add(CarFilePath);
            }

            System.IO.File.WriteAllLines(OptionsFile, AllPaths.ToArray());
        }


        public void DesignValueUpdates()
        {
            UpdateDocumentFromUI();

            UpdateUIFromDocument();

            CornerWeightTextBox.Text = Doc.Input.VehicleMass.ToString();
            SpringStiffnessTextBox.Text = Doc.Input.SpringStiffness.ToString();
            DampingCoefficientTextBox.Text = Doc.Input.DampingCoefficient.ToString();
        }




        public void DesignTreeList_FocusChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            DesignValueUpdates();

        }

        public void DesignTreeList_OnCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            DesignValueUpdates();
        }


        public void SimSetupTreeList_OnCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            UpdateDocumentFromUI();

            InitialiseSimSetupBarButton_ItemClick(null, null);
        }

        public void SimSetupTreeList_FocusChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            UpdateDocumentFromUI();

            InitialiseSimSetupBarButton_ItemClick(null, null);
        }


        public void UpdateUIFromDocument()
        {
            if (Doc != null)
            {
                // Display Values in UI from Initialised Value in Input Constructor

                DesignValuesTreeListColumn.TreeList.Nodes[0].Nodes[0].SetValue(DesignValuesTreeListColumn, Doc.Input.VehicleMass);

                DesignValuesTreeListColumn.TreeList.Nodes[1].Nodes[0].Nodes[0].SetValue(DesignValuesTreeListColumn, Doc.Input.SpringStiffness);

                DesignValuesTreeListColumn.TreeList.Nodes[1].Nodes[1].Nodes[0].SetValue(DesignValuesTreeListColumn, Doc.Input.SpringFreeLength);
                DesignValuesTreeListColumn.TreeList.Nodes[1].Nodes[1].Nodes[1].SetValue(DesignValuesTreeListColumn, Doc.Input.SpringCompressedLength);
                DesignValuesTreeListColumn.TreeList.Nodes[1].Nodes[1].Nodes[2].SetValue(DesignValuesTreeListColumn, Doc.Input.SpringExtendedLength);

                DesignValuesTreeListColumn.TreeList.Nodes[2].Nodes[0].SetValue(DesignValuesTreeListColumn, Doc.Input.DampingCoefficient);
                DesignValuesTreeListColumn.TreeList.Nodes[2].Nodes[1].SetValue(DesignValuesTreeListColumn, Doc.Input.DampingRatio);

                SimSetupValuesColumn.TreeList.Nodes[0].Nodes[0].SetValue(SimSetupValuesColumn, Doc.Input.StartTime);
                SimSetupValuesColumn.TreeList.Nodes[0].Nodes[1].SetValue(SimSetupValuesColumn, Doc.Input.TimeStep);
                SimSetupValuesColumn.TreeList.Nodes[0].Nodes[2].SetValue(SimSetupValuesColumn, Doc.Input.EndTime);

                SimSetupValuesColumn.TreeList.Nodes[1].Nodes[0].SetValue(SimSetupValuesColumn, Doc.Input.ExcitationFrequencyHz);

                SimSetupValuesColumn.TreeList.Nodes[2].Nodes[0].SetValue(SimSetupValuesColumn, Doc.Input.ForceAmplitude);

                SimSetupValuesColumn.TreeList.Nodes[3].Nodes[0].SetValue(SimSetupValuesColumn, Doc.Input.InitialDisplacement);
                SimSetupValuesColumn.TreeList.Nodes[3].Nodes[1].SetValue(SimSetupValuesColumn, Doc.Input.InitialVelocity);
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


                strval = DesignValuesTreeListColumn.TreeList.Nodes[0].Nodes[0].GetValue(DesignValuesTreeListColumn).ToString();

                if(double.TryParse(strval, out dblVal))
                {
                    Doc.Input.VehicleMass = dblVal;
                }

                else
                {
                    Error = true;
                }

                strval = DesignValuesTreeListColumn.TreeList.Nodes[1].Nodes[0].Nodes[0].GetValue(DesignValuesTreeListColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.SpringStiffness = dblVal;
                }

                else
                {
                    Error = true;
                }

                strval = DesignValuesTreeListColumn.TreeList.Nodes[1].Nodes[1].Nodes[0].GetValue(DesignValuesTreeListColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.SpringFreeLength = dblVal;
                }

                else
                {
                    Error = true;
                }

                strval = DesignValuesTreeListColumn.TreeList.Nodes[1].Nodes[1].Nodes[1].GetValue(DesignValuesTreeListColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.SpringCompressedLength = dblVal;
                }

                else
                {
                    Error = true;
                }

                strval = DesignValuesTreeListColumn.TreeList.Nodes[1].Nodes[1].Nodes[2].GetValue(DesignValuesTreeListColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.SpringExtendedLength = dblVal;
                }

                else
                {
                    Error = true;
                }

                strval = DesignValuesTreeListColumn.TreeList.Nodes[2].Nodes[0].GetValue(DesignValuesTreeListColumn).ToString();

                if (double.TryParse(strval, out dblVal))
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
                    Doc.Input.ExcitationFrequencyHz = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[2].Nodes[0].GetValue(SimSetupValuesColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.ForceAmplitude = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[3].Nodes[0].GetValue(SimSetupValuesColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.InitialDisplacement = dblVal;
                }
                else
                {
                    Error = true;
                }

                strval = SimSetupValuesColumn.TreeList.Nodes[3].Nodes[1].GetValue(SimSetupValuesColumn).ToString();

                if (double.TryParse(strval, out dblVal))
                {
                    Doc.Input.InitialVelocity = dblVal;
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

                //ValidateVehicleTemplateCheckedButton();
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

            NewCarDesignRibbonBarButton_ItemClick(null, null);
            DesignTreeList_FocusChanged(null, null);

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

                NewCarDesignRibbonBarButton_ItemClick(null, null);
                DesignTreeList_FocusChanged(null, null);

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

        private void NewCarDesignRibbonBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Car = new CarData(this);

            UpdateUIFromDocument();

            DesignValueUpdates();

            DesignTreeList.ExpandAll();
            DesignTreeList.Visible = true;

            NewCarBuilt = true;
        }


        private void SaveCar_ItemClick(object sender, ItemClickEventArgs e)
        {

            using (SaveFileDialog dialog = new SaveFileDialog() { InitialDirectory = CarFilePath, Filter = "Car Files (*.car)| *.car", ValidateNames = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Doc.Input.SaveCar(dialog.FileName);

                    CarFilePath = System.IO.Path.GetDirectoryName(dialog.FileName);
                }
            }
        }

        private void LoadCarBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog() { InitialDirectory = CarFilePath, Filter = "Car Files (*.car)| *.car", ValidateNames = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Doc.Input.LoadCar(dialog.FileName);

                    CarFilePath = System.IO.Path.GetDirectoryName(dialog.FileName);
                }
            }

            UpdateUIFromDocument();


            DesignTreeList_OnCellValueChanged(null, null);
        }


        private void SpringDesignBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            DesignTreeList.Visible = true;

            DesignParametersTreeListColumn.TreeList.Nodes[0].Expand();
            DesignParametersTreeListColumn.TreeList.Nodes[0].Visible = true;

            DesignParametersTreeListColumn.TreeList.Nodes[1].Expand();
            DesignParametersTreeListColumn.TreeList.Nodes[1].Visible = true;

            DesignParametersTreeListColumn.TreeList.Nodes[2].Collapse();
            DesignParametersTreeListColumn.TreeList.Nodes[2].Visible = false;

            DesignParametersTreeListColumn.TreeList.Nodes[3].Collapse();
            DesignParametersTreeListColumn.TreeList.Nodes[3].Visible = false;

            

        }


        private void BuildDamperDesignBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            DesignTreeList.Visible = true;

            DesignParametersTreeListColumn.TreeList.Nodes[0].Collapse();
            DesignParametersTreeListColumn.TreeList.Nodes[0].Visible = false;

            DesignParametersTreeListColumn.TreeList.Nodes[1].Collapse();
            DesignParametersTreeListColumn.TreeList.Nodes[1].Visible = false;

            DesignParametersTreeListColumn.TreeList.Nodes[2].Collapse();
            DesignParametersTreeListColumn.TreeList.Nodes[2].Visible = false;

            DesignParametersTreeListColumn.TreeList.Nodes[3].Expand();
            DesignParametersTreeListColumn.TreeList.Nodes[3].Visible = true;

            

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

            if (HarmonicIP)
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

            SimSetupParametersPanel.Visible = true;

            SimSetupParametersColumn.TreeList.Nodes[1].Collapse();
            SimSetupParametersColumn.TreeList.Nodes[1].Visible = false;

            SimSetupParametersColumn.TreeList.Nodes[2].Collapse();
            SimSetupParametersColumn.TreeList.Nodes[2].Visible = false;

            SimSetupParametersColumn.TreeList.Nodes[3].Collapse();
            SimSetupParametersColumn.TreeList.Nodes[3].Visible = false;

            NewSimSetup = true;
        }


        private void SaveSimSetupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog() { InitialDirectory = CarFilePath, Filter = "Sim Files (*.sim)| *.sim", ValidateNames = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Doc.Input.SaveSimData(dialog.FileName);

                    CarFilePath = System.IO.Path.GetDirectoryName(dialog.FileName);
                }
            }
        }


        private void LoadSimSetupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog() { InitialDirectory = CarFilePath, Filter = "Sim Files (*.sim)| *.sim", ValidateNames = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Doc.Input.LoadSimData(dialog.FileName);

                    CarFilePath = System.IO.Path.GetDirectoryName(dialog.FileName);
                }
            }
        }

        private void SingleStepIPCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (SingleStepIPCheckItem.Checked)
            {
                SingleStepIP = true;
                HarmonicIP = false;
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

                    SimSetupValuesColumn.TreeList.Nodes[3].Collapse();
                    SimSetupValuesColumn.TreeList.Nodes[3].Visible = false;

                    //SimSetupValuesColumn.TreeList.Nodes[3].Collapse();
                    //SimSetupValuesColumn.TreeList.Nodes[3].Visible = false;
                }


            }

        }

        private void HarmonicIPCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (HarmonicIPCheckItem.Checked)
            {
                SingleStepIP = false;
                HarmonicIP = true;
                CustomIP = false;

                CustomIPChartControl.Visible = false;

                if (HarmonicIP)
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

                    SimSetupValuesColumn.TreeList.Nodes[3].Collapse();
                    SimSetupValuesColumn.TreeList.Nodes[3].Visible = false;
                }
            }
        }

        private void CustomIPCheckItem_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (CustomIPCheckItem.Checked)
            {
                SingleStepIP = false;
                HarmonicIP = false;
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
            HarmonicIPCheckItem.Checked = false;
            CustomIPCheckItem.Checked = false;

            SimSetupTreeList_FocusChanged(null, null);
            
        }

        private void HarmonicIPCheckItem_OnClick(object sender, ItemClickEventArgs e)
        {
            SingleStepIPCheckItem.Checked = false;
            HarmonicIPCheckItem.Checked = true;
            CustomIPCheckItem.Checked = false;

            SimSetupTreeList_FocusChanged(null, null);
        }

        private void CustomIPCheckItem_OnClick(object sender, ItemClickEventArgs e)
        {
            SingleStepIPCheckItem.Checked = false;
            HarmonicIPCheckItem.Checked = false;
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
                Doc.Input.StepIPNeedsToRecalculate = true;
                Doc.Input.HarmonicIPNeedsToRecalculate = false;
            }

            else if (HarmonicIP)
            {
                Doc.Input.TimeNeedsToRecalculate = true;
                Doc.Input.ResponseNeedsToRecalculate = true;
                Doc.Input.CustomIPCalculate = false;
                Doc.Input.StepIPNeedsToRecalculate = false;
                Doc.Input.HarmonicIPNeedsToRecalculate = true;
            }

            else if (CustomIP)
            {
                Doc.Input.TimeNeedsToRecalculate = false;
                Doc.Input.ResponseNeedsToRecalculate = true;
                Doc.Input.CustomIPCalculate = true;
                Doc.Input.StepIPNeedsToRecalculate = false;
                Doc.Input.HarmonicIPNeedsToRecalculate = false;
            }

            if (!CustomIP)
            {
                Doc.Input.InputDataCalculate();
            }

            UpdateSimSetupPlotBarButton_ItemClick(sender, e);

        }

        private void RunSimSetupBarButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            InitialiseSimSetupBarButton_ItemClick(sender, e);

            Doc.Input.OutputDataCalculate();

            if (Doc.Input.ResponseCalculationComplete)
            {
                RunSuccess = true;
            }

            //else if (!Doc.Input.ResponseCalculationComplete)
            //{
            //    RunSuccess = false;
            //}



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
                
                SimSetupChartControl.Dock = DockStyle.Fill;
                SimSetupChartControl.Visible = true;
                SimSetupChartControl.Titles.Clear();


                //BodyResponseChartControl.Visible = false;

                ChartTitle StepTitle = new ChartTitle();

                StepTitle.Text = "Step Input";
                StepTitle.WordWrap = true;
                StepTitle.MaxLineCount = 2;

                StepTitle.Alignment = StringAlignment.Center;
                StepTitle.Dock = ChartTitleDockStyle.Top;
                StepTitle.Font = new Font("Tahoma", 18);
                StepTitle.TextColor = Color.Black;

                SimSetupChartControl.Titles.Add(StepTitle);
                
                Series StepFunction = new Series("Displacement", ViewType.Line);

                SimSetupChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.TimeIntervals.Count; i++)
                {
                    StepFunction.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.ForceVector[i]));
                }

                SimSetupChartControl.Series.Add(StepFunction);

                XYDiagram diagram = (XYDiagram)SimSetupChartControl.Diagram;

                //diagram.AxisX.WholeRange.MinValue = Doc.Input.StartTime;
                diagram.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisX.Alignment = AxisAlignment.Near;
                diagram.AxisX.Title.Text = "Time (s)";
                diagram.AxisX.Title.TextColor = Color.Black;
                diagram.AxisX.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisX.Title.Font = new Font("Tahoma", 14);

                //diagram.AxisY.WholeRange.MinValue = Doc.Input.StartTime;
                diagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Alignment = AxisAlignment.Near;
                diagram.AxisY.Title.Text = "Displacement (m)";
                diagram.AxisY.Title.TextColor = Color.Black;
                diagram.AxisY.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Title.Font = new Font("Tahoma", 14, FontStyle.Bold);

                SimSetupChartControl.Update();
            }

            else if (HarmonicIP)
            {

                SimSetupChartControl.Dock = DockStyle.Fill;
                SimSetupChartControl.Visible = true;
                SimSetupChartControl.Titles.Clear();

                ChartTitle HarmonicTitle = new ChartTitle();

                HarmonicTitle.Text = "Harmonic Input";
                HarmonicTitle.WordWrap = true;
                HarmonicTitle.MaxLineCount = 2;

                HarmonicTitle.Alignment = StringAlignment.Center;
                HarmonicTitle.Dock = ChartTitleDockStyle.Top;
                HarmonicTitle.Font = new Font("Tahoma", 18);
                HarmonicTitle.TextColor = Color.Black;

                SimSetupChartControl.Titles.Add(HarmonicTitle);

                //BodyResponseChartControl.Visible = false;

                Series MultipleStepSeries = new Series("Displacement", ViewType.Spline);

                SimSetupChartControl.Series.Clear();

                for (int i = 0; i < Doc.Input.TimeIntervals.Count; i++)
                {
                    MultipleStepSeries.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.HarmonicInput[i]));
                }

                SimSetupChartControl.Series.Add(MultipleStepSeries);

                XYDiagram diagram = (XYDiagram)SimSetupChartControl.Diagram;

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

                SimSetupChartControl.Update();
            }

            else if (CustomIP)
            {
                SimSetupChartControl.Visible = false;
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
                    BodyDisplacement.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.TotalResponse[i]));
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
                    VerticalAcceln.Points.Add(new SeriesPoint(Doc.Input.TimeIntervals[i], Doc.Input.BodyAcceln[i]));
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
