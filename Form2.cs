using FluentValidation.Data;
using FluentValidation.Models;
using FluentValidation.Results;
using FluentValidation.ValidationRules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FluentValidation
{
    public partial class Form2 : Form
    {
        ApplicationDbContext context;
        Category model = new Category();
        CategoryValidator validation = new CategoryValidator();
        public Form2()
        {
            InitializeComponent();
            context = new ApplicationDbContext();
        }

        private void categoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 product = new Form1();
            product.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void Clear()
        {
            txtCategory.Clear();
            checkYes.Checked = false;
            txtSearch.Clear();
        }
        void FormLoad()
        {
            dataGrdCategory.DataSource = context.Categories.Include("Products").Select(i => new
            {
                Id = i.Id,
                CategoryName = i.Name,
                Products = i.Products.Count(),
                IsApproved = i.IsConfirmed,
                CreatedDate = i.CreatedDate,
                UpdatedDate = i.UpdatedDate
            }).OrderByDescending(i => i.CreatedDate).ToList();
        }
        void Search()
        {
            dataGrdCategory.DataSource = context.Categories.Include("Products").Select(i => new
            {
                Id = i.Id,
                CategoryName = i.Name,
                Products = i.Products.Count(),
                IsApproved = i.IsConfirmed,
                CreatedDate = i.CreatedDate,
                UpdatedDate = i.UpdatedDate
            }).Where(i=>i.CategoryName.Contains(txtSearch.Text)).OrderByDescending(i => i.CreatedDate).ToList();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            FormLoad();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ValidationResult result = validation.Validate(model);
            if (!result.IsValid)
            {
                string errorMessage = "";
                foreach (var item in result.Errors)
                {
                    errorMessage += $"{item} \n";
                }
                MessageBox.Show(errorMessage, "Error Messages:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                model.Name = txtCategory.Text;
                model.IsConfirmed = Convert.ToBoolean(checkYes.Checked);
                model.CreatedDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());

                context.Categories.Add(model);
                context.SaveChanges();
                Clear();
                FormLoad();
            }
        }

        private void dataGrdCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGrdCategory.CurrentRow;

            txtCategory.Text = row.Cells["CategoryName"].Value.ToString();
            txtCategory.Tag = row.Cells["Id"].Value;           
            checkYes.Checked = Convert.ToBoolean(row.Cells["IsApproved"].Value.ToString());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ValidationResult result = validation.Validate(model);
            if (!result.IsValid)
            {
                string errorMessage = "";
                foreach (var item in result.Errors)
                {
                    errorMessage += $"{item} \n";
                }
                MessageBox.Show(errorMessage, "Error Messages:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                var categoryId = (int)txtCategory.Tag;
                model = context.Categories.FirstOrDefault(i => i.Id == categoryId);

                model.Name = txtCategory.Text;
                model.IsConfirmed = Convert.ToBoolean(checkYes.Checked);
                model.UpdatedDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());

                context.SaveChanges();
                Clear();
                FormLoad();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var categoryId = (int)dataGrdCategory.CurrentRow.Cells["Id"].Value;
            model = context.Categories.FirstOrDefault(i => i.Id == categoryId);

            context.Categories.Remove(model);
            context.SaveChanges();
            Clear();
            FormLoad();
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }
    }
}
