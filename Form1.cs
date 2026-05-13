namespace TimberCutPlannerMockup;

public partial class Form1 : Form
{
    private static readonly Color PanelLavender = Color.FromArgb(188, 187, 255);
    private static readonly Color HeaderGray = Color.FromArgb(228, 228, 228);
    private static readonly Color AccentCyan = Color.FromArgb(0, 202, 202);
    private static readonly Color AccentPurple = Color.FromArgb(191, 180, 255);

    public Form1()
    {
        InitializeComponent();
        BuildDesign();
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
