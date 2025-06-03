using OfficeOpenXml; // For Excel reading (EPPlus)
using Microsoft.Data.SqlClient; // For SQL Server access

namespace SAIdImporter
{
    public partial class Form1 : Form
    {

        private List<dynamic> allData = new List<dynamic>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbGender.Items.AddRange(new string[] { "All", "Male", "Female" });
            cmbGender.SelectedIndex = 0;

            txtSearch.TextChanged += (s, ev) => FilterAndSort();
            cmbGender.SelectedIndexChanged += (s, ev) => FilterAndSort();

            dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;
        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            // Set the license context for EPPlus
            ExcelPackage.License.SetNonCommercialPersonal("Michael Westman");

            // Open file dialog to select Excel file
            openFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return; // User cancelled

            string filePath = openFileDialog1.FileName;

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var ws = package.Workbook.Worksheets["Sheet1"];
                    if (ws == null)
                    {
                        MessageBox.Show("Sheet1 not found in the Excel file.");
                        return;
                    }

                    int rowCount = ws.Dimension.End.Row;

                    string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=PersonDB;Trusted_Connection=True;";

                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        for (int row = 2; row <= rowCount; row++) // Start at 2 to skip headers
                        {
                            string firstName = ws.Cells[row, 1].Text.Trim();
                            string surname = ws.Cells[row, 2].Text.Trim();
                            string idNumber = ws.Cells[row, 3].Text.Trim().PadLeft(13, '0');

                            // Validate ID number
                            SouthAfricanID saId = new SouthAfricanID(idNumber);

                            // Check if ID already exists in DB
                            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Persons WHERE IDNumber = @ID", conn);
                            checkCmd.Parameters.AddWithValue("@ID", idNumber);
                            int exists = (int)checkCmd.ExecuteScalar();
                            if (exists > 0)
                                continue; // Skip duplicates

                            // Insert new record
                            SqlCommand insertCmd = new SqlCommand(
                                "INSERT INTO Persons (FirstName, Surname, IDNumber) VALUES (@F, @S, @I)", conn);
                            insertCmd.Parameters.AddWithValue("@F", firstName);
                            insertCmd.Parameters.AddWithValue("@S", surname);
                            insertCmd.Parameters.AddWithValue("@I", idNumber);
                            insertCmd.ExecuteNonQuery();

                            // Add to list to show in grid with calculated fields
                            allData.Add(new
                            {
                                FirstName = firstName,
                                Surname = surname,
                                IDNumber = idNumber,
                                DateOfBirth = saId.DateOfBirth?.ToString("yyyy-MM-dd"),
                                Gender = saId.Gender,
                                Validity = saId.IsValid ? "Valid" : "Invalid"
                            });
                        }
                    }
                }

                // Bind to DataGridView to show results
                dataGridView1.DataSource = allData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error importing Excel file:\n" + ex.Message);
            }
        }

        private void FilterAndSort()
        {
            if (allData == null) return;

            string search = txtSearch.Text.Trim().ToLower();
            string genderFilter = cmbGender.SelectedItem.ToString();

            var filtered = allData.Where(x =>
                (string.IsNullOrEmpty(search) ||
                 x.FirstName.ToLower().Contains(search) ||
                 x.Surname.ToLower().Contains(search)) &&
                (genderFilter == "All" || x.Gender == genderFilter)
            ).ToList();

            // Optional: preserve sorting order
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = filtered;
        }

        private bool sortAscending = true;
        private string lastSortedColumn = "";

        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

            if (allData == null) return;

            var sorted = (sortAscending || lastSortedColumn != columnName)
                ? allData.OrderBy(x => x.GetType().GetProperty(columnName).GetValue(x, null)).ToList()
                : allData.OrderByDescending(x => x.GetType().GetProperty(columnName).GetValue(x, null)).ToList();

            sortAscending = lastSortedColumn == columnName ? !sortAscending : true;
            lastSortedColumn = columnName;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = sorted;
        }

        private void FilterResults()
        {
            if (allData == null)
                return;

            var filtered = allData.AsEnumerable();

            string search = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(p =>
                    p.FirstName.ToLower().Contains(search) ||
                    p.Surname.ToLower().Contains(search));
            }

            string gender = cmbGender.SelectedItem.ToString();
            if (gender != "All")
            {
                filtered = filtered.Where(p => p.Gender == gender);
            }

            dataGridView1.DataSource = filtered.ToList();
        }

        private void btnViewDb_Click(object sender, EventArgs e)
        {
            try
            {
                allData = new List<dynamic>();

                string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=PersonDB;Trusted_Connection=True;";

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var command = new SqlCommand("SELECT FirstName, Surname, IDNumber FROM Persons", conn);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string idNumber = reader["IDNumber"].ToString().PadLeft(13, '0');
                        var saId = new SouthAfricanID(idNumber); // Reuse your existing class

                        allData.Add(new
                        {
                            FirstName = reader["FirstName"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            IDNumber = idNumber,
                            DateOfBirth = saId.DateOfBirth?.ToString("yyyy-MM-dd"),
                            Gender = saId.Gender,
                            Validity = saId.IsValid ? "Valid" : "Invalid"
                        });
                    }
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = allData;
                FilterAndSort(); // Apply any active search/gender filters

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading from database:\n" + ex.Message);
            }
        }

    }
}
