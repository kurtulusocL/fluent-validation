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
    public partial class Form1 : Form
    {
        ApplicationDbContext context;
        Product model = new Product();
        ProductValidation validation = new ProductValidation();

        public Form1()
        {
            InitializeComponent();
            context = new ApplicationDbContext();
        }
        void Clear()
        {
            txtPrice.Clear();
            txtSearch.Clear();
            txtProduct.Clear();
            numUpStock.Value = 0;
            cmbCategory.SelectedItem = "";
            checkYes.Checked = false;
        }
        void FormLoad()
        {
            dataGrdProduct.DataSource = context.Products.Include("Category").Select(i => new
            {
                Id = i.Id,
                ProductName = i.Name,
                CategoryName = i.Category.Name,
                ProductStock = i.Stock,
                ProductPrice = i.Price,
                IsAppoved = i.IsConfirmed,
                CreatedDate = i.CreatedDate,
                UpdatedDate = i.UpdatedDate
            }).OrderByDescending(i => i.CreatedDate).ToList();
        }
        void OrderByStock()
        {
            dataGrdProduct.DataSource = context.Products.Include("Category").Select(i => new
            {
                Id = i.Id,
                ProductName = i.Name,
                CategoryName = i.Category.Name,
                ProductStock = i.Stock,
                ProductPrice = i.Price,
                IsAppoved = i.IsConfirmed,
                CreatedDate = i.CreatedDate,
                UpdatedDate = i.UpdatedDate
            }).OrderBy(i => i.ProductStock).ToList();
        }
        void OrderByPrice()
        {
            dataGrdProduct.DataSource = context.Products.Include("Category").Select(i => new
            {
                Id = i.Id,
                ProductName = i.Name,
                CategoryName = i.Category.Name,
                ProductStock = i.Stock,
                ProductPrice = i.Price,
                IsAppoved = i.IsConfirmed,
                CreatedDate = i.CreatedDate,
                UpdatedDate = i.UpdatedDate
            }).OrderByDescending(i => i.ProductPrice).ToList();
        }
        void OrderByUpdatedDate()
        {
            dataGrdProduct.DataSource = context.Products.Include("Category").Select(i => new
            {
                Id = i.Id,
                ProductName = i.Name,
                CategoryName = i.Category.Name,
                ProductStock = i.Stock,
                ProductPrice = i.Price,
                IsAppoved = i.IsConfirmed,
                CreatedDate = i.CreatedDate,
                UpdatedDate = i.UpdatedDate
            }).OrderByDescending(i => i.UpdatedDate.Value.ToString()).ToList();
        }
        void Search()
        {
            dataGrdProduct.DataSource = context.Products.Include("Category").Select(i => new
            {
                Id = i.Id,
                ProductName = i.Name,
                CategoryName = i.Category.Name,
                ProductStock = i.Stock,
                ProductPrice = i.Price,
                IsAppoved = i.IsConfirmed,
                CreatedDate = i.CreatedDate,
                UpdatedDate = i.UpdatedDate
            }).Where(i => i.ProductName.Contains(txtSearch.Text)).OrderByDescending(i => i.CreatedDate).ToList();
        }

        private void categoryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 category = new Form2();
            category.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var categoryList = context.Categories.Include("Products").Where(i => i.IsConfirmed == true).OrderByDescending(i => i.Products.Count()).ToList();
            cmbCategory.DataSource = categoryList;
            cmbCategory.ValueMember = "Id";
            cmbCategory.DisplayMember = "Name";

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
                model.Name = txtProduct.Text;
                model.Stock = Convert.ToInt32(numUpStock.Value);
                model.Price = Convert.ToDecimal(txtPrice.Text);
                model.CreatedDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                model.CategoryId = (int)cmbCategory.SelectedValue;

                context.Products.Add(model);
                context.SaveChanges();
                Clear();
                FormLoad();
            }
        }

        private void dataGrdProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGrdProduct.CurrentRow;

            txtProduct.Text = row.Cells["ProductName"].Value.ToString();
            txtProduct.Tag = row.Cells["Id"].Value;
            txtPrice.Text = Convert.ToDecimal(row.Cells["ProductPrice"].Value).ToString();
            numUpStock.Value = Convert.ToInt32(row.Cells["ProductStock"].Value.ToString());
            checkYes.Checked = Convert.ToBoolean(row.Cells["IsAppoved"].Value.ToString());
            cmbCategory.SelectedValue = row.Cells["CategoryName"].Value.ToString();
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
                var productId = (int)txtProduct.Tag;
                model = context.Products.FirstOrDefault(i => i.Id == productId);

                model.Name = txtProduct.Text;
                model.Stock = Convert.ToInt32(numUpStock.Value);
                model.Price = Convert.ToDecimal(txtPrice.Text);
                model.IsConfirmed = Convert.ToBoolean(checkYes.Checked);
                model.UpdatedDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                model.CategoryId = (int)cmbCategory.SelectedValue;

                context.SaveChanges();
                Clear();
                FormLoad();
                OrderByUpdatedDate();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var productId = (int)dataGrdProduct.CurrentRow.Cells["Id"].Value;
            model = context.Products.FirstOrDefault(i => i.Id == productId);

            context.Products.Remove(model);
            context.SaveChanges();
            Clear();
            FormLoad();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void radioCreatedDate_CheckedChanged(object sender, EventArgs e)
        {
            if (radioCreatedDate.Checked)
            {
                FormLoad();
            }
        }

        private void radioUpdatedDate_CheckedChanged(object sender, EventArgs e)
        {
            if (radioUpdatedDate.Checked)
            {
                OrderByUpdatedDate();
            }
        }

        private void radioStock_CheckedChanged(object sender, EventArgs e)
        {
            if (radioStock.Checked)
            {
                OrderByStock();
            }
        }

        private void radioPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (radioPrice.Checked)
            {
                OrderByPrice();
            }
        }
    }
}
