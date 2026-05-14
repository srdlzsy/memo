namespace TimberCutPlannerMockup;

public partial class Form1 : Form
{
    private static readonly Color PanelLavender = Color.FromArgb(188, 187, 255);
    private static readonly Color HeaderGray = Color.FromArgb(228, 228, 228);
    private static readonly Color AccentCyan = Color.FromArgb(0, 202, 202);
    private static readonly Color AccentPurple = Color.FromArgb(191, 180, 255);

    private readonly Random _plcRandom = new();
    private readonly Queue<string> _plcEvents = new();
    private VirtualPlcWoodState _plcState = new();
    private System.Windows.Forms.Timer? _plcTimer;
    private WoodPlcDashboard? _plcDashboard;
    private ListBox? _plcEventList;
    private Label? _plcModeValue;
    private Label? _plcLogValue;
    private Label? _plcLengthValue;
    private Label? _plcDimensionValue;
    private Label? _plcMoistureValue;
    private Label? _plcStageValue;
    private Label? _plcSpeedValue;
    private Label? _plcAlarmValue;
    private Label? _entrySignalValue;
    private Label? _measureSignalValue;
    private Label? _clampSignalValue;
    private Label? _sawSignalValue;
    private Label? _pusherSignalValue;
    private Label? _exitSignalValue;
    private Label? _rejectSignalValue;
    private Label? _cutProgressValue;
    private Label? _cutTargetValue;
    private Button? _pauseSimulationButton;
    private bool _simulationPaused;

    public Form1()
    {
        InitializeComponent();
        BuildDesign();
        StartVirtualPlc();
    }

    private void BuildDesign()
    {
        Text = "Project - [SUV-SAU]     TIMBER CUT CONTROL DEMO 0.1     INPUT";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1180, 760);
        Size = new Size(1420, 860);
        BackColor = Color.White;
        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

        Controls.Clear();

        Controls.Add(BuildStatusBar());
        Controls.Add(BuildMainTabs());
        Controls.Add(BuildToolBar());
        Controls.Add(BuildMenu());
    }

    private MenuStrip BuildMenu()
    {
        var menu = new MenuStrip
        {
            BackColor = Color.FromArgb(238, 238, 238),
            GripStyle = ToolStripGripStyle.Hidden,
            RenderMode = ToolStripRenderMode.System
        };

        menu.Items.Add(new ToolStripMenuItem("Files"));
        menu.Items.Add(new ToolStripMenuItem("Edit"));
        menu.Items.Add(new ToolStripMenuItem("Tools"));
        menu.Items.Add(new ToolStripMenuItem("Machine Setup"));
        menu.Items.Add(new ToolStripMenuItem("Help"));

        return menu;
    }

    private ToolStrip BuildToolBar()
    {
        var toolbar = new ToolStrip
        {
            AutoSize = false,
            Height = 48,
            BackColor = Color.FromArgb(242, 242, 242),
            GripStyle = ToolStripGripStyle.Hidden,
            RenderMode = ToolStripRenderMode.System,
            Padding = new Padding(8, 5, 8, 5)
        };

        toolbar.Items.Add(MakeToolButton("NEW", Color.White));
        toolbar.Items.Add(MakeToolButton("OPEN", Color.FromArgb(226, 238, 196)));
        toolbar.Items.Add(MakeToolButton("SAVE", Color.White));
        toolbar.Items.Add(MakeToolButton("PRINT", Color.White));
        toolbar.Items.Add(new ToolStripSeparator());
        toolbar.Items.Add(MakeToolButton("WALL", Color.FromArgb(255, 224, 97)));
        toolbar.Items.Add(MakeToolButton("CUT", Color.FromArgb(255, 232, 126)));
        toolbar.Items.Add(MakeToolButton("LOG +", Color.FromArgb(255, 232, 126)));
        toolbar.Items.Add(MakeToolButton("LOG -", Color.FromArgb(255, 232, 126)));
        toolbar.Items.Add(new ToolStripSeparator());
        toolbar.Items.Add(MakeWideToolButton("Optimisation", AccentCyan, 112));
        toolbar.Items.Add(MakeWideToolButton("MULTILOG", AccentPurple, 96));
        toolbar.Items.Add(new ToolStripSeparator { Margin = new Padding(340, 0, 8, 0) });
        toolbar.Items.Add(MakeToolButton("<", Color.FromArgb(111, 111, 255)));
        toolbar.Items.Add(MakeToolButton(">", Color.FromArgb(111, 111, 255)));
        toolbar.Items.Add(MakeWideToolButton("PUSH OUT", Color.White, 86));
        toolbar.Items.Add(MakeWideToolButton("HOME", Color.White, 70));
        toolbar.Items.Add(MakeWideToolButton("Daten SPS", Color.White, 86));
        toolbar.Items.Add(MakeToolButton("!", Color.Yellow));

        return toolbar;
    }

    private static ToolStripButton MakeToolButton(string text, Color backColor)
    {
        return new ToolStripButton(text)
        {
            AutoSize = false,
            Width = 54,
            Height = 34,
            BackColor = backColor,
            DisplayStyle = ToolStripItemDisplayStyle.Text,
            Font = new Font("Segoe UI", 8F, FontStyle.Bold),
            Margin = new Padding(2, 0, 2, 0)
        };
    }

    private static ToolStripButton MakeWideToolButton(string text, Color backColor, int width)
    {
        var button = MakeToolButton(text, backColor);
        button.Width = width;
        return button;
    }

    private TabControl BuildMainTabs()
    {
        var tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            Appearance = TabAppearance.FlatButtons,
            ItemSize = new Size(180, 32),
            SizeMode = TabSizeMode.Fixed
        };

        tabs.TabPages.Add(BuildInputTab());
        tabs.TabPages.Add(BuildProductionTab());
        tabs.TabPages.Add(BuildPlcSimulationTab());
        tabs.TabPages.Add(BuildAddTab());
        return tabs;
    }

    private TabPage BuildInputTab()
    {
        var page = new TabPage("ITTO CONTROL SOFTWARE FOR DATA INPUT")
        {
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3,
            BackColor = Color.White
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 230));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));

        var grid = BuildPartsGrid();
        root.Controls.Add(grid, 0, 0);

        var side = BuildInputSidePanel();
        root.Controls.Add(side, 1, 0);
        root.SetRowSpan(side, 3);

        var canvas = new CutPlanCanvas
        {
            Dock = DockStyle.Fill,
            Mode = CutPlanMode.Input
        };
        root.Controls.Add(canvas, 0, 1);

        root.Controls.Add(BuildLogInputPanel(), 0, 2);

        page.Controls.Add(root);
        return page;
    }

    private TabPage BuildProductionTab()
    {
        var page = new TabPage("PRODUCTION MONITORING AND BOOKING")
        {
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            BackColor = Color.White
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 230));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        root.Controls.Add(BuildPartsGrid(), 0, 0);
        root.Controls.Add(BuildProductionSidePanel(), 1, 0);

        var lower = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.White,
            Margin = new Padding(0, 14, 0, 0)
        };
        lower.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        lower.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        lower.Controls.Add(BuildProductionActionPanel(), 0, 0);
        lower.Controls.Add(new CutPlanCanvas
        {
            Dock = DockStyle.Fill,
            Mode = CutPlanMode.Production
        }, 1, 0);

        root.Controls.Add(lower, 0, 1);
        root.SetColumnSpan(lower, 2);

        page.Controls.Add(root);
        return page;
    }

    private TabPage BuildAddTab()
    {
        var page = new TabPage("EKLE")
        {
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };

        panel.Controls.Add(new Label
        {
            Text = "Yeni kayıt ekleyin:",
            Location = new Point(16, 16),
            AutoSize = true,
            Font = new Font(Font, FontStyle.Bold)
        });

        panel.Controls.Add(new TextBox
        {
            Name = "txtNewItem",
            Location = new Point(16, 46),
            Size = new Size(300, 24)
        });

        panel.Controls.Add(new Button
        {
            Text = "Ekle",
            Location = new Point(330, 44),
            Size = new Size(80, 28)
        });

        page.Controls.Add(panel);
        return page;
    }

    private TabPage BuildPlcSimulationTab()
    {
        var page = new TabPage("PLC ODUN DURUM SIMULASYONU")
        {
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 3,
            BackColor = Color.White
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 132));

        var metricBar = BuildPlcMetricBar();
        root.Controls.Add(metricBar, 0, 0);
        root.SetColumnSpan(metricBar, 3);

        root.Controls.Add(BuildPlcSignalPanel(), 0, 1);

        _plcDashboard = new WoodPlcDashboard
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(10, 0, 10, 0)
        };
        root.Controls.Add(_plcDashboard, 1, 1);

        root.Controls.Add(BuildPlcDetailPanel(), 2, 1);

        var eventPanel = BuildPlcEventPanel();
        root.Controls.Add(eventPanel, 0, 2);
        root.SetColumnSpan(eventPanel, 3);

        page.Controls.Add(root);
        return page;
    }

    private TableLayoutPanel BuildPlcMetricBar()
    {
        var bar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(238, 238, 238),
            ColumnCount = 8,
            RowCount = 1,
            Padding = new Padding(6),
            Margin = new Padding(0, 0, 0, 8)
        };

        for (var i = 0; i < 8; i++)
        {
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
        }

        bar.Controls.Add(BuildMetricCell("KAYNAK", "SIM PLC", AccentCyan, label => _plcModeValue = label), 0, 0);
        bar.Controls.Add(BuildMetricCell("ODUN", "LOG-001", Color.FromArgb(255, 224, 97), label => _plcLogValue = label), 1, 0);
        bar.Controls.Add(BuildMetricCell("BOY", "600 cm", Color.LightSteelBlue, label => _plcLengthValue = label), 2, 0);
        bar.Controls.Add(BuildMetricCell("KESIT", "5.8 x 13.5", AccentPurple, label => _plcDimensionValue = label), 3, 0);
        bar.Controls.Add(BuildMetricCell("NEM", "14.0%", Color.FromArgb(180, 224, 180), label => _plcMoistureValue = label), 4, 0);
        bar.Controls.Add(BuildMetricCell("DURUM", "Besleme", Color.FromArgb(129, 129, 255), label => _plcStageValue = label), 5, 0);
        bar.Controls.Add(BuildMetricCell("HIZ", "42 cm/s", Color.FromArgb(190, 220, 235), label => _plcSpeedValue = label), 6, 0);
        bar.Controls.Add(BuildMetricCell("UYARI", "Yok", Color.FromArgb(255, 190, 190), label => _plcAlarmValue = label), 7, 0);

        return bar;
    }

    private static Panel BuildMetricCell(string title, string value, Color accent, Action<Label> captureValue)
    {
        var cell = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(4)
        };

        cell.Controls.Add(new Panel
        {
            BackColor = accent,
            Dock = DockStyle.Left,
            Width = 5
        });

        cell.Controls.Add(new Label
        {
            Text = title,
            Location = new Point(14, 8),
            Size = new Size(132, 18),
            ForeColor = Color.DimGray,
            Font = new Font("Segoe UI", 7.5F, FontStyle.Bold)
        });

        var valueLabel = new Label
        {
            Text = value,
            Location = new Point(14, 32),
            Size = new Size(132, 28),
            ForeColor = Color.Black,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            AutoEllipsis = true,
            TextAlign = ContentAlignment.MiddleLeft
        };
        cell.Controls.Add(valueLabel);
        captureValue(valueLabel);

        return cell;
    }

    private Panel BuildPlcSignalPanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = PanelLavender,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0)
        };

        AddSectionTitle(panel, "PLC sinyalleri", 12);
        AddSignalRow(panel, "Giris sensoru", 44, label => _entrySignalValue = label);
        AddSignalRow(panel, "Olcum sensoru", 78, label => _measureSignalValue = label);
        AddSignalRow(panel, "Mengene kapali", 112, label => _clampSignalValue = label);
        AddSignalRow(panel, "Testere motoru", 146, label => _sawSignalValue = label);
        AddSignalRow(panel, "Pusher ileri", 180, label => _pusherSignalValue = label);
        AddSignalRow(panel, "Cikis sensoru", 214, label => _exitSignalValue = label);

        AddSectionTitle(panel, "Kalite rotasi", 266);
        AddSignalRow(panel, "Ayirma kapagi", 298, label => _rejectSignalValue = label);

        return panel;
    }

    private static void AddSignalRow(Control parent, string name, int top, Action<Label> captureValue)
    {
        parent.Controls.Add(new Label
        {
            Text = name,
            Location = new Point(18, top + 4),
            Size = new Size(130, 22),
            AutoEllipsis = true,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
        });

        var value = new Label
        {
            Text = "OFF",
            Location = new Point(154, top),
            Size = new Size(56, 26),
            BackColor = Color.FromArgb(218, 218, 218),
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 8F, FontStyle.Bold)
        };

        parent.Controls.Add(value);
        captureValue(value);
    }

    private Panel BuildPlcDetailPanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(238, 238, 238),
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0)
        };

        AddSectionTitle(panel, "Kesim komutu", 12);
        AddPlcInfoRow(panel, "Hedef parca", "359 cm", 48, label => _cutTargetValue = label);
        AddPlcInfoRow(panel, "Kesim ilerleme", "0%", 86, label => _cutProgressValue = label);

        AddSectionTitle(panel, "Sanal kontrol", 146);

        _pauseSimulationButton = new Button
        {
            Text = "SIM PAUSE",
            Location = new Point(18, 180),
            Size = new Size(110, 32),
            BackColor = Color.White,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
        };
        _pauseSimulationButton.Click += (_, _) =>
        {
            _simulationPaused = !_simulationPaused;
            UpdatePlcUi();
        };
        panel.Controls.Add(_pauseSimulationButton);

        var nextLogButton = new Button
        {
            Text = "NEW LOG",
            Location = new Point(144, 180),
            Size = new Size(110, 32),
            BackColor = Color.White,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
        };
        nextLogButton.Click += (_, _) => CreateNextVirtualLog("Manuel yeni odun");
        panel.Controls.Add(nextLogButton);

        var source = new Label
        {
            Text = "PLC kaynagi: sanal register seti",
            Location = new Point(18, 238),
            Size = new Size(236, 24),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
        };
        panel.Controls.Add(source);

        return panel;
    }

    private static void AddPlcInfoRow(Control parent, string labelText, string valueText, int top, Action<Label> captureValue)
    {
        parent.Controls.Add(new Label
        {
            Text = labelText,
            Location = new Point(18, top),
            Size = new Size(110, 24),
            TextAlign = ContentAlignment.MiddleLeft
        });

        var value = new Label
        {
            Text = valueText,
            Location = new Point(136, top),
            Size = new Size(118, 24),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold)
        };
        parent.Controls.Add(value);
        captureValue(value);
    }

    private Control BuildPlcEventPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.FromArgb(238, 238, 238),
            Padding = new Padding(10),
            Margin = new Padding(0, 8, 0, 0)
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        panel.Controls.Add(new Label
        {
            Text = "PLC olaylari",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(135, 0, 135),
            Font = new Font("Segoe UI", 8F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);

        _plcEventList = new ListBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.FixedSingle,
            IntegralHeight = false,
            BackColor = Color.White,
            Font = new Font("Consolas", 9F, FontStyle.Regular)
        };
        panel.Controls.Add(_plcEventList, 0, 1);

        return panel;
    }

    private DataGridView BuildPartsGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            ColumnHeadersHeight = 26,
            EnableHeadersVisualStyles = false,
            GridColor = Color.FromArgb(120, 120, 150),
            MultiSelect = false,
            ReadOnly = true,
            RowHeadersWidth = 30,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };

        grid.ColumnHeadersDefaultCellStyle.BackColor = HeaderGray;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font(Font, FontStyle.Bold);
        grid.DefaultCellStyle.BackColor = Color.FromArgb(189, 190, 255);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(141, 160, 255);
        grid.DefaultCellStyle.SelectionForeColor = Color.Black;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(204, 205, 255);

        AddColumn(grid, "LFNr.", 62);
        AddColumn(grid, "Wall", 72);
        AddColumn(grid, "Component", 150);
        AddColumn(grid, "Width", 74);
        AddColumn(grid, "Height", 74);
        AddColumn(grid, "Length", 84);
        AddColumn(grid, "Quantity", 84);
        AddColumn(grid, "Produced", 84);
        AddColumn(grid, "End left", 220);
        AddColumn(grid, "End right", 220);

        string[,] rows =
        {
            { "1", "1", "Y12.3", "5.8", "13.5", "359", "20", "3", "Corner joint external", "Corner joint external" },
            { "2", "1", "Y2.3", "5.8", "13.5", "359", "2", "2", "Corner joint external", "Corner joint external" },
            { "3", "1", "X1.2.3.4", "5.8", "13.5", "320", "4", "0", "Offcut", "Corner joint external" },
            { "4", "1", "X1.4", "5.8", "13.5", "320", "2", "2", "Corner joint external", "Corner joint external" },
            { "5", "1", "X1.4", "5.8", "13.5", "320", "4", "0", "Corner joint external", "Corner joint external" },
            { "6", "1", "X1.2.4 P", "5.8", "13.5", "320", "3", "0", "Corner joint external", "Corner joint external" },
            { "7", "1", "X1.4", "5.8", "13.5", "290", "2", "0", "Offcut", "Corner joint external" },
            { "8", "1", "X1.4", "5.8", "13.5", "270", "2", "0", "Offcut", "Corner joint external" },
            { "9", "1", "X1.4", "5.8", "13.5", "250", "2", "0", "Offcut", "Corner joint external" },
            { "10", "1", "Y2", "5.8", "13.5", "233", "7", "5", "Headslot", "Corner joint external" },
            { "11", "1", "X1.4", "5.8", "13.5", "225", "4", "0", "Offcut", "Corner joint external" },
            { "12", "1", "X1.2.4", "5.8", "13.5", "225", "5", "0", "Offcut", "Corner joint external" }
        };

        for (int i = 0; i < rows.GetLength(0); i++)
        {
            var values = new object[rows.GetLength(1)];
            for (int j = 0; j < rows.GetLength(1); j++)
            {
                values[j] = rows[i, j];
            }

            grid.Rows.Add(values);
        }

        if (grid.Rows.Count > 0)
        {
            grid.Rows[0].Selected = true;
        }

        return grid;
    }

    private static void AddColumn(DataGridView grid, string header, int minimumWidth)
    {
        var index = grid.Columns.Add(header.Replace(".", string.Empty).Replace(" ", string.Empty), header);
        grid.Columns[index].MinimumWidth = minimumWidth;
        grid.Columns[index].FillWeight = minimumWidth;
    }

    private Panel BuildInputSidePanel()
    {
        var panel = BuildLavenderPanel();
        AddSectionTitle(panel, "Wall selection", 12);

        panel.Controls.Add(new Label
        {
            Text = "Wall:",
            Location = new Point(16, 46),
            AutoSize = true
        });

        panel.Controls.Add(new ComboBox
        {
            Location = new Point(58, 42),
            Size = new Size(112, 24),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Items = { "WN: 1", "WN: 2", "WN: 3" },
            SelectedIndex = 0
        });

        panel.Controls.Add(new RadioButton
        {
            Text = "Sorted by number",
            Location = new Point(18, 82),
            Size = new Size(154, 24),
            Checked = true,
            BackColor = AccentCyan
        });

        panel.Controls.Add(new RadioButton
        {
            Text = "Sorted by length",
            Location = new Point(18, 112),
            Size = new Size(154, 24)
        });

        panel.Controls.Add(new Label
        {
            Text = "dimension",
            Location = new Point(18, 166),
            AutoSize = true
        });

        panel.Controls.Add(new ComboBox
        {
            Location = new Point(18, 190),
            Size = new Size(154, 24),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Items = { "5.8 / 13.5", "7.0 / 13.5", "8.0 / 16.0" },
            SelectedIndex = 0
        });

        panel.Controls.Add(new Button
        {
            Text = "Proj",
            Location = new Point(18, 240),
            Size = new Size(62, 28)
        });

        panel.Controls.Add(new TextBox
        {
            Text = "1",
            Location = new Point(90, 242),
            Size = new Size(44, 24),
            TextAlign = HorizontalAlignment.Center
        });

        panel.Controls.Add(new Label
        {
            Text = "X",
            Location = new Point(148, 247),
            AutoSize = true
        });

        return panel;
    }

    private Panel BuildProductionSidePanel()
    {
        var panel = BuildLavenderPanel();
        AddSectionTitle(panel, "Options", 12);

        panel.Controls.Add(new Button
        {
            Text = "Preview graphic",
            Location = new Point(18, 48),
            Size = new Size(150, 48),
            BackColor = Color.FromArgb(129, 129, 255),
            FlatStyle = FlatStyle.Standard
        });

        panel.Controls.Add(new RadioButton
        {
            Text = "Opti List",
            Location = new Point(18, 132),
            Size = new Size(140, 24)
        });

        panel.Controls.Add(new RadioButton
        {
            Text = "Input List",
            Location = new Point(18, 164),
            Size = new Size(140, 24),
            Checked = true
        });

        panel.Controls.Add(new Label
        {
            Text = "Current log",
            Location = new Point(18, 226),
            AutoSize = true
        });

        panel.Controls.Add(new Label
        {
            Text = "600 cm / 5.8 x 13.5",
            Location = new Point(18, 250),
            Size = new Size(150, 24),
            BorderStyle = BorderStyle.Fixed3D,
            BackColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        });

        return panel;
    }

    private static Panel BuildLavenderPanel()
    {
        return new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = PanelLavender,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(8, 0, 0, 0)
        };
    }

    private static void AddSectionTitle(Control parent, string text, int top)
    {
        parent.Controls.Add(new Label
        {
            Text = text,
            Location = new Point(14, top),
            AutoSize = true,
            ForeColor = Color.FromArgb(135, 0, 135),
            Font = new Font("Segoe UI", 8F, FontStyle.Bold)
        });
    }

    private Panel BuildLogInputPanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(238, 238, 238),
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 8, 0, 0)
        };

        panel.Controls.Add(new Label
        {
            Text = "Log input - all dimensions in cm",
            Location = new Point(12, 8),
            AutoSize = true,
            ForeColor = Color.FromArgb(184, 88, 184),
            Font = new Font("Segoe UI", 8F, FontStyle.Bold)
        });

        AddLabeledInput(panel, "Wall", "1", 24, 36, 62);
        AddLabeledInput(panel, "Component", "Y12.3", 108, 36, 100);
        AddLabeledInput(panel, "Width", "5.8", 232, 36, 70);
        AddLabeledInput(panel, "Length", "359", 326, 36, 78);
        AddLabeledInput(panel, "Quantity", "20", 428, 36, 72);

        panel.Controls.Add(new Label
        {
            Text = "End left",
            Location = new Point(524, 21),
            AutoSize = true
        });

        panel.Controls.Add(new ComboBox
        {
            Location = new Point(524, 45),
            Size = new Size(170, 24),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Items = { "Corner joint external", "Offcut", "Headslot" },
            SelectedIndex = 0
        });

        panel.Controls.Add(new Label
        {
            Text = "End right",
            Location = new Point(710, 21),
            AutoSize = true
        });

        panel.Controls.Add(new ComboBox
        {
            Location = new Point(710, 45),
            Size = new Size(170, 24),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Items = { "Corner joint external", "Offcut", "Headslot" },
            SelectedIndex = 0
        });

        panel.Controls.Add(MakeCommandButton("Add log [F2]", 900, 42));
        panel.Controls.Add(MakeCommandButton("Dovetail below [F4]", 1018, 42));
        panel.Controls.Add(MakeCommandButton("Groove below [F6]", 1174, 42));
        panel.Controls.Add(MakeCommandButton("Grooves front [F8]", 1326, 42));

        return panel;
    }

    private static void AddLabeledInput(Control parent, string label, string value, int left, int top, int width)
    {
        parent.Controls.Add(new Label
        {
            Text = label,
            Location = new Point(left, top - 24),
            AutoSize = true
        });

        parent.Controls.Add(new TextBox
        {
            Text = value,
            Location = new Point(left, top),
            Size = new Size(width, 24)
        });
    }

    private static Button MakeCommandButton(string text, int left, int top)
    {
        return new Button
        {
            Text = text,
            Location = new Point(left, top),
            Size = new Size(136, 30),
            BackColor = Color.FromArgb(234, 234, 234)
        };
    }

    private Panel BuildProductionActionPanel()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };

        panel.Controls.Add(MakeProductionButton("DONE", Color.LimeGreen, 20, 56));
        panel.Controls.Add(MakeProductionButton("WORK", Color.Firebrick, 20, 168));
        panel.Controls.Add(new Label
        {
            Text = "0 / 5",
            Location = new Point(20, 204),
            Size = new Size(90, 28),
            BorderStyle = BorderStyle.FixedSingle,
            ForeColor = Color.Firebrick,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        });
        panel.Controls.Add(MakeProductionButton("NEXT", Color.MediumOrchid, 20, 314));

        return panel;
    }

    private static Button MakeProductionButton(string text, Color color, int left, int top)
    {
        return new Button
        {
            Text = text,
            Location = new Point(left, top),
            Size = new Size(90, 30),
            BackColor = Color.White,
            ForeColor = color,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            FlatStyle = FlatStyle.Standard
        };
    }

    private void StartVirtualPlc()
    {
        components ??= new System.ComponentModel.Container();
        _plcState = VirtualPlcWoodState.Create(1, _plcRandom);
        AddPlcEvent("SIM PLC basladi");
        AddPlcEvent($"LOG-{_plcState.LogNumber:000} hatta alindi");

        _plcTimer = new System.Windows.Forms.Timer(components)
        {
            Interval = 520
        };
        _plcTimer.Tick += (_, _) => AdvanceVirtualPlc();
        _plcTimer.Start();

        UpdatePlcUi();
    }

    private void AdvanceVirtualPlc()
    {
        if (_simulationPaused)
        {
            return;
        }

        _plcState.Tick++;
        _plcState.StageTick++;

        if (_plcState.WarningTicks > 0)
        {
            _plcState.WarningTicks--;
        }

        _plcState.MoisturePercent = Math.Clamp(
            _plcState.MoisturePercent + (float)((_plcRandom.NextDouble() - 0.5D) * 0.18D),
            9.5F,
            22.0F);

        if (_plcState.WarningTicks == 0 &&
            (_plcState.Stage == WoodProcessStage.Measuring || _plcState.Stage == WoodProcessStage.Centering) &&
            _plcRandom.NextDouble() < 0.025D)
        {
            _plcState.WarningTicks = 5;
            _plcState.AlarmText = _plcRandom.Next(2) == 0 ? "Olcum sensor gecikmesi" : "Besleme hizi dalgali";
            AddPlcEvent($"LOG-{_plcState.LogNumber:000} uyari: {_plcState.AlarmText}");
        }

        switch (_plcState.Stage)
        {
            case WoodProcessStage.Loading:
                _plcState.ConveyorSpeedCmSec = Jitter(48);
                _plcState.PositionPercent = Math.Min(18F, _plcState.PositionPercent + 3.6F);
                if (_plcState.StageTick >= 6)
                {
                    SwitchPlcStage(WoodProcessStage.Measuring);
                }
                break;

            case WoodProcessStage.Measuring:
                _plcState.ConveyorSpeedCmSec = Jitter(30);
                _plcState.PositionPercent = Math.Min(36F, _plcState.PositionPercent + 2.1F);
                if (_plcState.StageTick == 2)
                {
                    AddPlcEvent($"LOG-{_plcState.LogNumber:000} boy/nem okundu");
                }

                if (_plcState.StageTick >= 8)
                {
                    SwitchPlcStage(WoodProcessStage.Centering);
                }
                break;

            case WoodProcessStage.Centering:
                _plcState.ConveyorSpeedCmSec = Jitter(22);
                _plcState.PositionPercent = Math.Min(55F, _plcState.PositionPercent + 2.7F);
                if (_plcState.StageTick >= 7)
                {
                    SwitchPlcStage(WoodProcessStage.Cutting);
                }
                break;

            case WoodProcessStage.Cutting:
                _plcState.ConveyorSpeedCmSec = 0;
                if (_plcState.StageTick == 1)
                {
                    AddPlcEvent($"LOG-{_plcState.LogNumber:000} mengene kapandi");
                }

                _plcState.CutProgress = Math.Min(100, _plcState.CutProgress + _plcRandom.Next(13, 22));
                if (_plcState.CutProgress >= 100)
                {
                    AddPlcEvent($"LOG-{_plcState.LogNumber:000} kesim tamam");
                    SwitchPlcStage(WoodProcessStage.Pushing);
                }
                break;

            case WoodProcessStage.Pushing:
                _plcState.ConveyorSpeedCmSec = Jitter(38);
                _plcState.PositionPercent = Math.Min(80F, _plcState.PositionPercent + 3.2F);
                if (_plcState.StageTick >= 9)
                {
                    SwitchPlcStage(WoodProcessStage.Exiting);
                }
                break;

            case WoodProcessStage.Exiting:
                _plcState.ConveyorSpeedCmSec = Jitter(44);
                _plcState.PositionPercent = Math.Min(100F, _plcState.PositionPercent + 4.2F);
                if (_plcState.PositionPercent >= 100F)
                {
                    AddPlcEvent(_plcState.Reject
                        ? $"LOG-{_plcState.LogNumber:000} kalite ayirmaya gitti"
                        : $"LOG-{_plcState.LogNumber:000} cikisa ulasti");
                    CreateNextVirtualLog("Otomatik yeni odun");
                }
                break;
        }

        UpdatePlcUi();
    }

    private int Jitter(int baseValue)
    {
        return Math.Max(0, baseValue + _plcRandom.Next(-3, 4));
    }

    private void SwitchPlcStage(WoodProcessStage stage)
    {
        if (_plcState.Stage == stage)
        {
            return;
        }

        _plcState.Stage = stage;
        _plcState.StageTick = 0;
        AddPlcEvent($"LOG-{_plcState.LogNumber:000} durum: {_plcState.StageText}");
    }

    private void CreateNextVirtualLog(string reason)
    {
        var nextNumber = Math.Max(1, _plcState.LogNumber + 1);
        _plcState = VirtualPlcWoodState.Create(nextNumber, _plcRandom);
        AddPlcEvent($"{reason}: LOG-{_plcState.LogNumber:000} hatta alindi");
        UpdatePlcUi();
    }

    private void UpdatePlcUi()
    {
        SetText(_plcModeValue, _simulationPaused ? "PAUSE" : "SIM PLC");
        SetText(_plcLogValue, $"LOG-{_plcState.LogNumber:000}");
        SetText(_plcLengthValue, $"{_plcState.LengthCm} cm");
        SetText(_plcDimensionValue, $"{_plcState.WidthCm:0.0} x {_plcState.HeightCm:0.0}");
        SetText(_plcMoistureValue, $"{_plcState.MoisturePercent:0.0}%");
        SetText(_plcStageValue, _plcState.StageText);
        SetText(_plcSpeedValue, $"{_plcState.ConveyorSpeedCmSec} cm/s");
        SetText(_plcAlarmValue, _plcState.WarningTicks > 0 ? _plcState.AlarmText : _plcState.Reject ? "Nem risk" : "Yok");
        SetText(_cutProgressValue, $"{_plcState.CutProgress}%");
        SetText(_cutTargetValue, $"{_plcState.CutTargetCm} cm");

        if (_plcAlarmValue != null)
        {
            _plcAlarmValue.ForeColor = _plcState.WarningTicks > 0 || _plcState.Reject ? Color.Firebrick : Color.Black;
        }

        SetSignal(_entrySignalValue, _plcState.EntrySensor);
        SetSignal(_measureSignalValue, _plcState.MeasureSensor);
        SetSignal(_clampSignalValue, _plcState.ClampClosed);
        SetSignal(_sawSignalValue, _plcState.SawMotor);
        SetSignal(_pusherSignalValue, _plcState.PusherOut);
        SetSignal(_exitSignalValue, _plcState.ExitSensor);
        SetSignal(_rejectSignalValue, _plcState.RejectGate);

        if (_pauseSimulationButton != null)
        {
            _pauseSimulationButton.Text = _simulationPaused ? "SIM START" : "SIM PAUSE";
            _pauseSimulationButton.BackColor = _simulationPaused ? Color.FromArgb(255, 244, 178) : Color.White;
        }

        if (_plcDashboard != null)
        {
            _plcDashboard.Snapshot = _plcState;
            _plcDashboard.Invalidate();
        }

        RefreshPlcEventList();
    }

    private static void SetText(Label? label, string value)
    {
        if (label != null)
        {
            label.Text = value;
        }
    }

    private static void SetSignal(Label? label, bool active)
    {
        if (label == null)
        {
            return;
        }

        label.Text = active ? "ON" : "OFF";
        label.BackColor = active ? Color.FromArgb(95, 210, 125) : Color.FromArgb(218, 218, 218);
        label.ForeColor = active ? Color.Black : Color.DimGray;
    }

    private void AddPlcEvent(string message)
    {
        _plcEvents.Enqueue($"{DateTime.Now:HH:mm:ss}  {message}");
        while (_plcEvents.Count > 8)
        {
            _plcEvents.Dequeue();
        }

        RefreshPlcEventList();
    }

    private void RefreshPlcEventList()
    {
        if (_plcEventList == null)
        {
            return;
        }

        _plcEventList.BeginUpdate();
        _plcEventList.Items.Clear();
        foreach (var plcEvent in _plcEvents.Reverse())
        {
            _plcEventList.Items.Add(plcEvent);
        }

        _plcEventList.EndUpdate();
    }

    private StatusStrip BuildStatusBar()
    {
        var status = new StatusStrip
        {
            BackColor = Color.FromArgb(232, 232, 232),
            SizingGrip = false
        };

        status.Items.Add(new ToolStripStatusLabel("Status: New"));
        status.Items.Add(new ToolStripStatusLabel { Spring = true });
        status.Items.Add(new ToolStripStatusLabel("Job: 36037"));
        status.Items.Add(new ToolStripStatusLabel("Machine: BL100A"));
        status.Items.Add(new ToolStripStatusLabel("Labelprinter: On"));
        status.Items.Add(new ToolStripStatusLabel(DateTime.Now.ToString("dd.MM.yyyy HH:mm")));

        return status;
    }
}

internal enum CutPlanMode
{
    Input,
    Production
}

internal sealed class CutPlanCanvas : Control
{
    public CutPlanMode Mode { get; set; }

    public CutPlanCanvas()
    {
        BackColor = Color.White;
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        e.Graphics.Clear(Color.White);

        if (Mode == CutPlanMode.Input)
        {
            DrawInputPlan(e.Graphics, ClientRectangle);
            return;
        }

        DrawProductionPlan(e.Graphics, ClientRectangle);
    }

    private void DrawInputPlan(Graphics graphics, Rectangle bounds)
    {
        var working = Rectangle.Inflate(bounds, -36, -26);
        if (working.Width < 300 || working.Height < 180)
        {
            return;
        }

        var log = new RectangleF(working.Left + 90, working.Top + working.Height * 0.35f, working.Width - 180, 30);
        DrawDimension(graphics, log.Left, log.Right, log.Top - 58, "15.8            139                         194.2                         10");
        DrawLog(graphics, log, Color.DimGray, false);

        using var tealBrush = new SolidBrush(Color.FromArgb(45, 145, 129));
        using var smallFont = new Font(Font.FontFamily, 9F, FontStyle.Bold);
        graphics.DrawString("5.8", smallFont, tealBrush, log.Left + 18, log.Top - 24);
        graphics.DrawString("5.8", smallFont, tealBrush, log.Left + log.Width * 0.48f, log.Top - 24);
        graphics.DrawString("5.8", smallFont, tealBrush, log.Right - 70, log.Top - 24);

        DrawJoint(graphics, new RectangleF(log.Left + 12, log.Top - 2, 18, log.Height + 4));
        DrawJoint(graphics, new RectangleF(log.Left + log.Width * 0.48f, log.Top - 2, 18, log.Height + 4));
        DrawJoint(graphics, new RectangleF(log.Right - 50, log.Top - 2, 18, log.Height + 4));

        graphics.DrawString("HV", Font, Brushes.DimGray, log.Left + 12, log.Bottom + 2);
        graphics.DrawString("HV", Font, Brushes.DimGray, log.Left + log.Width * 0.48f, log.Bottom + 2);
        graphics.DrawString("HV", Font, Brushes.DimGray, log.Right - 50, log.Bottom + 2);

        var bottom = log.Top + 120;
        DrawDimension(graphics, log.Left, log.Right, bottom, "359");
        DrawCenteredText(graphics, "Y12.3  >  5.8 / 13.5  <", Font, Brushes.DimGray, new RectangleF(log.Left, bottom + 12, log.Width, 22));

        DrawScreenTitle(graphics, "ITTO CONTROL SOFTWARE FOR DATA INPUT", bounds);
    }

    private void DrawProductionPlan(Graphics graphics, Rectangle bounds)
    {
        var working = Rectangle.Inflate(bounds, -30, -20);
        if (working.Width < 420 || working.Height < 260)
        {
            return;
        }

        var logWidth = working.Width - 70;
        var left = working.Left + 30;
        var firstY = working.Top + 38;
        var gap = Math.Max(78, (working.Height - 160) / 3);

        DrawProductionRow(graphics, new RectangleF(left, firstY, logWidth, 28), Color.DimGray, false, "2", "10");
        DrawProductionRow(graphics, new RectangleF(left, firstY + gap, logWidth, 28), Color.Red, true, "1", "10");
        DrawProductionRow(graphics, new RectangleF(left, firstY + gap * 2, logWidth, 28), Color.DimGray, false, "1", "12");

        DrawScreenTitle(graphics, "PRODUCTION MONITORING AND BOOKING", bounds);
    }

    private void DrawProductionRow(Graphics graphics, RectangleF log, Color lineColor, bool selected, string leftNumber, string rightNumber)
    {
        using var labelBrush = new SolidBrush(Color.FromArgb(33, 150, 136));
        using var labelFont = new Font(Font.FontFamily, 10F, FontStyle.Bold);
        using var redBrush = new SolidBrush(Color.Red);

        DrawCenteredText(graphics, leftNumber, labelFont, labelBrush, new RectangleF(log.Left, log.Top - 36, log.Width * 0.52f, 22));
        DrawCenteredText(graphics, rightNumber, labelFont, labelBrush, new RectangleF(log.Left + log.Width * 0.56f, log.Top - 36, log.Width * 0.42f, 22));

        DrawLog(graphics, log, lineColor, selected);

        var separatorOne = log.Left + log.Width * 0.34f;
        var separatorTwo = log.Left + log.Width * 0.69f;
        DrawJoint(graphics, new RectangleF(log.Left + 8, log.Top - 2, 18, log.Height + 4));
        DrawJoint(graphics, new RectangleF(separatorOne - 8, log.Top - 2, 16, log.Height + 4));
        DrawJoint(graphics, new RectangleF(separatorTwo - 8, log.Top - 2, 16, log.Height + 4));
        DrawJoint(graphics, new RectangleF(log.Right - 28, log.Top - 2, 18, log.Height + 4));

        var pocket = new RectangleF(separatorTwo + 22, log.Top + 1, 44, log.Height - 2);
        using (var pen = new Pen(lineColor, selected ? 3F : 1.4F))
        {
            graphics.DrawRectangle(pen, pocket.X, pocket.Y, pocket.Width, pocket.Height);
            graphics.DrawLine(pen, pocket.Left + pocket.Width * 0.45f, pocket.Top + 4, pocket.Left + pocket.Width * 0.45f, pocket.Bottom - 4);
        }

        if (selected)
        {
            var arrowX = pocket.Left + pocket.Width * 0.5f;
            PointF[] points =
            {
                new(arrowX, log.Top - 34),
                new(arrowX - 10, log.Top - 18),
                new(arrowX + 10, log.Top - 18)
            };
            graphics.FillPolygon(redBrush, points);
            using var shaft = new Pen(Color.Red, 4F);
            graphics.DrawLine(shaft, arrowX, log.Top - 50, arrowX, log.Top - 28);
        }

        DrawDimension(graphics, log.Left, separatorTwo + 30, log.Bottom + 34, "359");
        DrawDimension(graphics, log.Left, log.Right, log.Bottom + 58, "600");
        DrawDimension(graphics, separatorTwo + 60, log.Right, log.Bottom + 34, "233");
        DrawCenteredText(graphics, "5.8 / 13.5", Font, Brushes.DimGray, new RectangleF(log.Left, log.Top - 14, log.Width, 20));
    }

    private static void DrawLog(Graphics graphics, RectangleF log, Color lineColor, bool selected)
    {
        using var pen = new Pen(lineColor, selected ? 3F : 1.5F);
        graphics.DrawRectangle(pen, log.X, log.Y, log.Width, log.Height);

        var segmentOne = log.Left + log.Width * 0.35f;
        var segmentTwo = log.Left + log.Width * 0.68f;
        graphics.DrawLine(pen, segmentOne, log.Top, segmentOne, log.Bottom);
        graphics.DrawLine(pen, segmentTwo, log.Top, segmentTwo, log.Bottom);
    }

    private static void DrawJoint(Graphics graphics, RectangleF rect)
    {
        using var brush = new System.Drawing.Drawing2D.HatchBrush(
            System.Drawing.Drawing2D.HatchStyle.Percent50,
            Color.LightGray,
            Color.White);
        using var pen = new Pen(Color.Gray, 1F);
        graphics.FillRectangle(brush, rect);
        graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
    }

    private static void DrawDimension(Graphics graphics, float left, float right, float y, string label)
    {
        using var pen = new Pen(Color.Silver, 1F);
        using var brush = new SolidBrush(Color.DimGray);
        using var font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);

        graphics.DrawLine(pen, left, y, right, y);
        graphics.DrawLine(pen, left, y - 9, left, y + 9);
        graphics.DrawLine(pen, right, y - 9, right, y + 9);
        graphics.DrawLine(pen, left - 4, y - 4, left + 4, y + 4);
        graphics.DrawLine(pen, right - 4, y - 4, right + 4, y + 4);

        var labelSize = graphics.MeasureString(label, font);
        graphics.DrawString(label, font, brush, left + ((right - left - labelSize.Width) / 2F), y - 22);
    }

    private static void DrawCenteredText(Graphics graphics, string text, Font font, Brush brush, RectangleF bounds)
    {
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter
        };
        graphics.DrawString(text, font, brush, bounds, format);
    }

    private static void DrawScreenTitle(Graphics graphics, string title, Rectangle bounds)
    {
        var bandHeight = Math.Min(76, Math.Max(48, bounds.Height / 8));
        var band = new RectangleF(bounds.Left, bounds.Bottom - bandHeight, bounds.Width, bandHeight);
        using var bandBrush = new SolidBrush(Color.FromArgb(224, 224, 224));
        using var linePen = new Pen(Color.DeepPink, 3F);
        using var textBrush = new SolidBrush(Color.Black);
        using var titleFont = new Font("Segoe UI", Math.Max(18F, Math.Min(34F, bounds.Width / 34F)), FontStyle.Regular, GraphicsUnit.Point);

        graphics.FillRectangle(bandBrush, band);
        graphics.DrawLine(linePen, band.Left + 12, band.Bottom - 10, band.Right - 12, band.Bottom - 10);
        DrawCenteredText(graphics, title, titleFont, textBrush, band);
    }
}

internal enum WoodProcessStage
{
    Loading,
    Measuring,
    Centering,
    Cutting,
    Pushing,
    Exiting
}

internal sealed class VirtualPlcWoodState
{
    public int Tick { get; set; }
    public int LogNumber { get; set; } = 1;
    public float PositionPercent { get; set; }
    public WoodProcessStage Stage { get; set; } = WoodProcessStage.Loading;
    public int StageTick { get; set; }
    public int LengthCm { get; set; } = 600;
    public float WidthCm { get; set; } = 5.8F;
    public float HeightCm { get; set; } = 13.5F;
    public float MoisturePercent { get; set; } = 14.0F;
    public int ConveyorSpeedCmSec { get; set; } = 42;
    public int CutTargetCm { get; set; } = 359;
    public int CutProgress { get; set; }
    public int WarningTicks { get; set; }
    public string AlarmText { get; set; } = "Yok";
    public bool Reject { get; set; }

    public static VirtualPlcWoodState Create(int logNumber, Random random)
    {
        var width = random.Next(3) == 0 ? 7.0F : 5.8F;
        var height = width > 6F ? 16.0F : 13.5F;
        var length = random.Next(570, 641);
        var moisture = (float)(11.4D + random.NextDouble() * 8.8D);
        int[] cutTargets = { 233, 270, 290, 320, 359 };

        return new VirtualPlcWoodState
        {
            LogNumber = logNumber,
            LengthCm = length,
            WidthCm = width,
            HeightCm = height,
            MoisturePercent = moisture,
            CutTargetCm = Math.Min(length - 150, cutTargets[random.Next(cutTargets.Length)]),
            Reject = moisture > 18.4F || random.NextDouble() < 0.08D,
            AlarmText = moisture > 18.4F ? "Nem yuksek" : "Yok"
        };
    }

    public string StageText => Stage switch
    {
        WoodProcessStage.Loading => "Besleme",
        WoodProcessStage.Measuring => "Olcum",
        WoodProcessStage.Centering => "Pozisyon",
        WoodProcessStage.Cutting => "Kesim",
        WoodProcessStage.Pushing => "Pusher",
        WoodProcessStage.Exiting => "Cikis",
        _ => "Bekle"
    };

    public int StageProgressPercent => Stage switch
    {
        WoodProcessStage.Loading => Math.Min(100, StageTick * 17),
        WoodProcessStage.Measuring => Math.Min(100, StageTick * 13),
        WoodProcessStage.Centering => Math.Min(100, StageTick * 15),
        WoodProcessStage.Cutting => CutProgress,
        WoodProcessStage.Pushing => Math.Min(100, StageTick * 12),
        WoodProcessStage.Exiting => Math.Clamp((int)((PositionPercent - 80F) * 5F), 0, 100),
        _ => 0
    };

    public bool EntrySensor => Stage == WoodProcessStage.Loading || PositionPercent < 24F;
    public bool MeasureSensor => Stage == WoodProcessStage.Measuring || Stage == WoodProcessStage.Centering;
    public bool ClampClosed => Stage == WoodProcessStage.Cutting;
    public bool SawMotor => Stage == WoodProcessStage.Cutting;
    public bool PusherOut => Stage == WoodProcessStage.Pushing;
    public bool ExitSensor => Stage == WoodProcessStage.Exiting || PositionPercent > 88F;
    public bool RejectGate => Reject && (Stage == WoodProcessStage.Pushing || Stage == WoodProcessStage.Exiting);
}

internal sealed class WoodPlcDashboard : Control
{
    public VirtualPlcWoodState? Snapshot { get; set; }

    public WoodPlcDashboard()
    {
        BackColor = Color.White;
        DoubleBuffered = true;
        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        e.Graphics.Clear(Color.White);

        var state = Snapshot;
        if (state == null)
        {
            DrawCenteredText(e.Graphics, "PLC SIM WAIT", Font, Brushes.DimGray, ClientRectangle);
            return;
        }

        var working = Rectangle.Inflate(ClientRectangle, -24, -22);
        if (working.Width < 420 || working.Height < 260)
        {
            return;
        }

        DrawHeader(e.Graphics, working, state);
        DrawConveyor(e.Graphics, working, state);
        DrawProgress(e.Graphics, working, state);
        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
    }

    private void DrawHeader(Graphics graphics, Rectangle bounds, VirtualPlcWoodState state)
    {
        var header = new RectangleF(bounds.Left, bounds.Top, bounds.Width, 54);
        using var headerBrush = new SolidBrush(Color.FromArgb(238, 238, 238));
        using var linePen = new Pen(Color.FromArgb(0, 202, 202), 3F);
        using var titleFont = new Font(Font.FontFamily, 15F, FontStyle.Bold);
        using var smallFont = new Font(Font.FontFamily, 8.5F, FontStyle.Bold);
        using var textBrush = new SolidBrush(Color.Black);

        graphics.FillRectangle(headerBrush, header);
        graphics.DrawLine(linePen, header.Left, header.Bottom - 2, header.Right, header.Bottom - 2);
        graphics.DrawString("SANAL PLC AKIS HATTI", titleFont, textBrush, header.Left + 14, header.Top + 10);

        var statusText = $"LOG-{state.LogNumber:000}  |  {state.StageText}  |  hedef {state.CutTargetCm} cm";
        DrawRightAlignedText(graphics, statusText, smallFont, Brushes.DimGray, new RectangleF(header.Left + 260, header.Top + 14, header.Width - 276, 24));
    }

    private void DrawConveyor(Graphics graphics, Rectangle bounds, VirtualPlcWoodState state)
    {
        var track = new RectangleF(
            bounds.Left + 42,
            bounds.Top + bounds.Height * 0.52F,
            bounds.Width - 84,
            34);

        using var trackBrush = new SolidBrush(Color.FromArgb(214, 220, 224));
        using var trackEdgePen = new Pen(Color.FromArgb(135, 145, 150), 1.5F);
        graphics.FillRectangle(trackBrush, track);
        graphics.DrawRectangle(trackEdgePen, track.X, track.Y, track.Width, track.Height);

        using var rollerPen = new Pen(Color.FromArgb(145, 155, 160), 1F);
        for (var x = track.Left + 18F; x < track.Right - 8F; x += 34F)
        {
            graphics.DrawEllipse(rollerPen, x, track.Top + 8F, 16F, 16F);
        }

        DrawStation(graphics, track, 0.07F, "Giris", state.EntrySensor);
        DrawStation(graphics, track, 0.28F, "Olcum", state.MeasureSensor);
        DrawStation(graphics, track, 0.50F, "Mengene", state.ClampClosed);
        DrawStation(graphics, track, 0.65F, "Testere", state.SawMotor);
        DrawStation(graphics, track, 0.91F, "Cikis", state.ExitSensor);

        DrawPusher(graphics, track, state);
        DrawSaw(graphics, track.Left + track.Width * 0.65F, track.Top - 58F, state);
        DrawRejectGate(graphics, track, state);
        DrawWood(graphics, track, state);
    }

    private void DrawStation(Graphics graphics, RectangleF track, float percent, string label, bool active)
    {
        var x = track.Left + track.Width * percent;
        using var linePen = new Pen(active ? Color.FromArgb(0, 140, 130) : Color.Silver, active ? 2.2F : 1.2F);
        using var font = new Font(Font.FontFamily, 8F, FontStyle.Bold);

        graphics.DrawLine(linePen, x, track.Top - 58F, x, track.Bottom + 38F);
        DrawIndicator(graphics, new RectangleF(x - 8F, track.Top - 78F, 16F, 16F), active);
        DrawCenteredText(graphics, label, font, Brushes.DimGray, new RectangleF(x - 42F, track.Bottom + 42F, 84F, 20F));
    }

    private void DrawWood(Graphics graphics, RectangleF track, VirtualPlcWoodState state)
    {
        var logWidth = Math.Clamp(track.Width * 0.19F + ((state.LengthCm - 600) * 0.5F), 120F, Math.Min(230F, track.Width * 0.36F));
        var logHeight = 48F;
        var logX = track.Left + (track.Width - logWidth) * (state.PositionPercent / 100F);
        var log = new RectangleF(logX, track.Top - logHeight - 6F, logWidth, logHeight);

        using var woodBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
            log,
            Color.FromArgb(176, 112, 54),
            Color.FromArgb(108, 66, 32),
            90F);
        using var edgePen = new Pen(Color.FromArgb(82, 48, 24), 2F);
        using var grainPen = new Pen(Color.FromArgb(120, 72, 36), 1F);
        using var labelFont = new Font(Font.FontFamily, 8.5F, FontStyle.Bold);

        graphics.FillRectangle(woodBrush, log);
        graphics.DrawRectangle(edgePen, log.X, log.Y, log.Width, log.Height);

        for (var y = log.Top + 9F; y < log.Bottom - 4F; y += 9F)
        {
            graphics.DrawLine(grainPen, log.Left + 8F, y, log.Right - 8F, y + (state.Tick % 3));
        }

        var cutX = log.Left + log.Width * Math.Clamp(state.CutTargetCm / (float)state.LengthCm, 0.12F, 0.88F);
        using var cutPen = new Pen(Color.White, 2F);
        using var cutShadowPen = new Pen(Color.Firebrick, 1F);
        graphics.DrawLine(cutPen, cutX, log.Top - 4F, cutX, log.Bottom + 4F);
        graphics.DrawLine(cutShadowPen, cutX + 3F, log.Top - 4F, cutX + 3F, log.Bottom + 4F);

        DrawCenteredText(graphics, $"LOG-{state.LogNumber:000}", labelFont, Brushes.White, log);
    }

    private void DrawSaw(Graphics graphics, float centerX, float centerY, VirtualPlcWoodState state)
    {
        var active = state.SawMotor;
        var radius = active ? 24F : 21F;
        using var sawBrush = new SolidBrush(active ? Color.FromArgb(230, 230, 235) : Color.FromArgb(210, 210, 214));
        using var pen = new Pen(active ? Color.Firebrick : Color.DimGray, active ? 2F : 1.2F);

        graphics.FillEllipse(sawBrush, centerX - radius, centerY - radius, radius * 2F, radius * 2F);
        graphics.DrawEllipse(pen, centerX - radius, centerY - radius, radius * 2F, radius * 2F);

        var phase = active ? (state.Tick % 12) * Math.PI / 12D : 0D;
        for (var i = 0; i < 10; i++)
        {
            var angle = phase + (Math.PI * 2D * i / 10D);
            var inner = radius * 0.35F;
            var outer = radius * 0.92F;
            var x1 = centerX + (float)Math.Cos(angle) * inner;
            var y1 = centerY + (float)Math.Sin(angle) * inner;
            var x2 = centerX + (float)Math.Cos(angle) * outer;
            var y2 = centerY + (float)Math.Sin(angle) * outer;
            graphics.DrawLine(pen, x1, y1, x2, y2);
        }
    }

    private static void DrawPusher(Graphics graphics, RectangleF track, VirtualPlcWoodState state)
    {
        var extension = state.PusherOut ? 1F : 0.35F;
        var arm = new RectangleF(track.Left + track.Width * 0.37F, track.Bottom + 14F, track.Width * 0.20F * extension, 16F);
        using var brush = new SolidBrush(state.PusherOut ? Color.FromArgb(129, 129, 255) : Color.FromArgb(190, 190, 210));
        using var pen = new Pen(Color.FromArgb(90, 90, 145), 1.5F);

        graphics.FillRectangle(brush, arm);
        graphics.DrawRectangle(pen, arm.X, arm.Y, arm.Width, arm.Height);
        graphics.FillRectangle(brush, arm.Left - 18F, arm.Top - 7F, 18F, 30F);
    }

    private static void DrawRejectGate(Graphics graphics, RectangleF track, VirtualPlcWoodState state)
    {
        var gate = new RectangleF(track.Left + track.Width * 0.76F, track.Bottom + 12F, 74F, 36F);
        using var pen = new Pen(state.RejectGate ? Color.Firebrick : Color.Gray, state.RejectGate ? 3F : 1.2F);
        using var font = new Font("Segoe UI", 8F, FontStyle.Bold);

        graphics.DrawRectangle(pen, gate.X, gate.Y, gate.Width, gate.Height);
        DrawCenteredText(graphics, "AYIRMA", font, state.RejectGate ? Brushes.Firebrick : Brushes.DimGray, gate);
    }

    private void DrawProgress(Graphics graphics, Rectangle bounds, VirtualPlcWoodState state)
    {
        var progressFrame = new RectangleF(bounds.Left + 30F, bounds.Bottom - 58F, bounds.Width - 60F, 18F);
        using var frameBrush = new SolidBrush(Color.FromArgb(224, 224, 224));
        using var fillBrush = new SolidBrush(state.WarningTicks > 0 || state.Reject ? Color.FromArgb(255, 190, 86) : Color.FromArgb(0, 202, 202));
        using var pen = new Pen(Color.Silver, 1F);
        using var font = new Font(Font.FontFamily, 9F, FontStyle.Bold);

        graphics.FillRectangle(frameBrush, progressFrame);
        var fill = progressFrame;
        fill.Width *= state.StageProgressPercent / 100F;
        graphics.FillRectangle(fillBrush, fill);
        graphics.DrawRectangle(pen, progressFrame.X, progressFrame.Y, progressFrame.Width, progressFrame.Height);

        var text = state.WarningTicks > 0
            ? state.AlarmText
            : state.Reject
                ? "Kalite kontrol: ayirma rotasi hazir"
                : $"{state.StageText} ilerleme {state.StageProgressPercent}%";
        DrawCenteredText(graphics, text, font, Brushes.Black, new RectangleF(progressFrame.Left, progressFrame.Top - 28F, progressFrame.Width, 22F));

        using var smallFont = new Font(Font.FontFamily, 8F, FontStyle.Bold);
        var detail = $"Boy {state.LengthCm} cm     Kesit {state.WidthCm:0.0} x {state.HeightCm:0.0}     Nem {state.MoisturePercent:0.0}%     Hiz {state.ConveyorSpeedCmSec} cm/s";
        DrawCenteredText(graphics, detail, smallFont, Brushes.DimGray, new RectangleF(bounds.Left, bounds.Bottom - 30F, bounds.Width, 24F));
    }

    private static void DrawIndicator(Graphics graphics, RectangleF bounds, bool active)
    {
        using var brush = new SolidBrush(active ? Color.FromArgb(95, 210, 125) : Color.FromArgb(210, 210, 210));
        using var pen = new Pen(active ? Color.FromArgb(34, 125, 64) : Color.Gray, 1.2F);

        graphics.FillEllipse(brush, bounds);
        graphics.DrawEllipse(pen, bounds);
    }

    private static void DrawCenteredText(Graphics graphics, string text, Font font, Brush brush, RectangleF bounds)
    {
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter
        };
        graphics.DrawString(text, font, brush, bounds, format);
    }

    private static void DrawRightAlignedText(Graphics graphics, string text, Font font, Brush brush, RectangleF bounds)
    {
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter
        };
        graphics.DrawString(text, font, brush, bounds, format);
    }
}
