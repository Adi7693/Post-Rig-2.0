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

namespace PostRig2._0
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        static readonly double roadCarCornerWeight = 400.0 ;
        static readonly double touringCarCornerWeight= 250.0;
        static readonly double singleSeaterCornerWeight = 175.0;
        static readonly double roadCarSpringStiffness= 80000.0;
        static readonly double touringCarSpringStiffness = 120000.0;
        static readonly double singleSeaterSpringStiffness = 150000.0;
        static readonly double roadCarDampingCoefficient= 4000.0;
        static readonly double touringCarDampingCoefficient = 8000.0;
        static readonly double singleSeaterDampingCoefficient = 8500.0;


        public Document Doc { get; set; }

        private void CheckButtonById (string caption)
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

            else if (cornerWeight == touringCarCornerWeight && springStiffness == touringCarSpringStiffness && dampingCoefficient == touringCarDampingCoefficient)
            {
                CheckButtonById("Touring Car");
            }

            else if (cornerWeight == singleSeaterCornerWeight && springStiffness == singleSeaterSpringStiffness && dampingCoefficient == singleSeaterDampingCoefficient)
            {
                CheckButtonById("Single Seater");
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
            }
        }

        public void UpdateDocumentFromUI()
        {
            if (Doc != null)
            {
                // Set Properties from Data input by the user

                double dblVal = -1.0;
                

                bool Error = false;

                if(double.TryParse(CornerWeightTextBox.Text, out dblVal))
                {
                    Doc.Input.VehicleMass = dblVal;
                }
                else
                {
                    Error = true;
                }

                if(double.TryParse(SpringStiffnessTextBox.Text, out dblVal))
                {
                    Doc.Input.SpringStiffness = dblVal;
                }
                else
                {
                    Error = true;
                }

                if(double.TryParse(DampingCoeffTextBox.Text, out dblVal))
                {
                    Doc.Input.DampingCoefficient = dblVal;
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

            
        }

        
        // New File Creation

        private void NewFileRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Doc = new Document();

            UpdateUIFromDocument();

            FileRibbonBasePanel.Visible = false;

            DesignRibbonPage.Visible = true;
            DesignRibbonBasePanel.Visible = true;



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

        private void BuildCarHomeRibbonBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            VehicleDataHomeRibbonPanel.Visible = true;

            VehicleDataModelPanel.Visible = true;
        }

        private void ShowDesignPanelBarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DesignRibbonBasePanel.Visible = true;

            DesignRibbonBasePanel.BringToFront();
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
            else if (e.Button.Properties.Caption == "Touring Car")
            {
                Doc.Input.VehicleMass = touringCarCornerWeight;
                Doc.Input.SpringStiffness = touringCarSpringStiffness;
                Doc.Input.DampingCoefficient = touringCarDampingCoefficient;
            }
            else if (e.Button.Properties.Caption == "Single Seater")
            {
                Doc.Input.VehicleMass = singleSeaterCornerWeight;
                Doc.Input.SpringStiffness = singleSeaterSpringStiffness;
                Doc.Input.DampingCoefficient = singleSeaterDampingCoefficient;
            }

            UpdateUIFromDocument();
        }

        private void OnRibbonPageChanged(object sender, EventArgs e)
        {
            if(MainFormRibbonControl.SelectedPage == DesignRibbonPage)
            {
                ShowDesignPanelBarButton_ItemClick(sender, null);
            }
        }

        private void OnTextInputChanged(object sender, EventArgs e)
        {
            UpdateDocumentFromUI();
        }
    }
}
