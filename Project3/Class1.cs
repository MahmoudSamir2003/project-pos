using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Project3
{
    public partial class SalesForm : Form
    {
        private List<Product> cart = new List<Product>();

        private TextBox productNameTextBox;
        private TextBox priceTextBox; // Change to TextBox for price
        private NumericUpDown quantityNumericUpDown;
        private ListBox cartListBox;
        private Label totalLabel;
        private TextBox discountTextBox;
        private Button addButton;
        private Button updateButton;
        private Button deleteButton;

        private Product selectedProduct; // To store selected product for update

        public SalesForm()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeComponent()
        {
            // Initialize controls and set properties
            this.productNameTextBox = new TextBox();
            this.priceTextBox = new TextBox(); // Change to TextBox for price
            this.quantityNumericUpDown = new NumericUpDown();
            this.cartListBox = new ListBox();
            this.totalLabel = new Label();
            this.discountTextBox = new TextBox();
            this.addButton = new Button();
            this.updateButton = new Button();
            this.deleteButton = new Button();

            // Set control properties
            this.Text = "Sales Form";
            this.Size = new System.Drawing.Size(600, 400);

            this.productNameTextBox.Location = new System.Drawing.Point(10, 10);
            this.productNameTextBox.Size = new System.Drawing.Size(200, 20);
            this.productNameTextBox.Name = "productNameTextBox";
            this.productNameTextBox.Text = "Product Name";

            this.priceTextBox.Location = new System.Drawing.Point(10, 40); // Change to TextBox for price
            this.priceTextBox.Size = new System.Drawing.Size(200, 20); // Change to TextBox for price
            this.priceTextBox.Name = "priceTextBox"; // Change to TextBox for price
            this.priceTextBox.Text = "0"; // Change to TextBox for price

            this.quantityNumericUpDown.Location = new System.Drawing.Point(10, 70);
            this.quantityNumericUpDown.Size = new System.Drawing.Size(200, 20);
            this.quantityNumericUpDown.Name = "quantityNumericUpDown";
            this.quantityNumericUpDown.Value = 1;
            this.quantityNumericUpDown.Minimum = 1;

            this.addButton.Location = new System.Drawing.Point(10, 100);
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.Text = "Add";
            this.addButton.Name = "addButton";

            this.updateButton.Location = new System.Drawing.Point(90, 100);
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.Text = "Update";
            this.updateButton.Name = "updateButton";

            this.deleteButton.Location = new System.Drawing.Point(170, 100);
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.Text = "Delete";
            this.deleteButton.Name = "deleteButton";

            this.cartListBox.Location = new System.Drawing.Point(10, 130);
            this.cartListBox.Size = new System.Drawing.Size(400, 200);
            this.cartListBox.Name = "cartListBox";

            this.totalLabel.Location = new System.Drawing.Point(420, 130);
            this.totalLabel.Size = new System.Drawing.Size(150, 20);
            this.totalLabel.Name = "totalLabel";

            this.discountTextBox.Location = new System.Drawing.Point(420, 160);
            this.discountTextBox.Size = new System.Drawing.Size(150, 20);
            this.discountTextBox.Name = "discountTextBox";
            this.discountTextBox.Text = "0"; // Default discount
            this.discountTextBox.TextChanged += DiscountTextBox_TextChanged;

            // Add controls to the form
            this.Controls.Add(productNameTextBox);
            this.Controls.Add(priceTextBox); // Change to TextBox for price
            this.Controls.Add(quantityNumericUpDown);
            this.Controls.Add(addButton);
            this.Controls.Add(updateButton);
            this.Controls.Add(deleteButton);
            this.Controls.Add(cartListBox);
            this.Controls.Add(totalLabel);
            this.Controls.Add(discountTextBox);
        }

        private void InitializeControls()
        {
            // Add event handlers
            addButton.Click += addButton_Click;
            updateButton.Click += updateButton_Click;
            deleteButton.Click += deleteButton_Click;

            // Double click on ListBox to select and update product
            cartListBox.DoubleClick += (sender, e) =>
            {
                int selectedIndex = cartListBox.SelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < cart.Count)
                {
                    selectedProduct = cart[selectedIndex];
                    productNameTextBox.Text = selectedProduct.Name;
                    priceTextBox.Text = selectedProduct.Price.ToString(); // Populate price
                    quantityNumericUpDown.Value = selectedProduct.Quantity;
                    discountTextBox.Text = selectedProduct.Discount.ToString();
                }
            };
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string productName = productNameTextBox.Text;
            decimal price = decimal.Parse(priceTextBox.Text); // Parse price from TextBox
            int quantity = (int)quantityNumericUpDown.Value;

            if (int.TryParse(discountTextBox.Text, out int discount))
            {
                if (discount >= 0 && discount <= 100)
                {
                    Product product = new Product(productName, price, quantity, discount);
                    cart.Add(product);

                    UpdateCartDisplay();
                }
                else
                {
                    MessageBox.Show("Discount should be between 0 and 100.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (selectedProduct != null)
            {
                selectedProduct.Name = productNameTextBox.Text;
                selectedProduct.Price = decimal.Parse(priceTextBox.Text);
                selectedProduct.Quantity = (int)quantityNumericUpDown.Value;
                selectedProduct.Discount = int.Parse(discountTextBox.Text);

                UpdateCartDisplay();
                selectedProduct = null; // Reset selected product after update
            }
            else
            {
                MessageBox.Show("Please select a product from the cart to update.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = cartListBox.SelectedIndex;
            if (selectedIndex >= 0)
            {
                cart.RemoveAt(selectedIndex);
                UpdateCartDisplay();
            }
        }

        private void UpdateCartDisplay()
        {
            cartListBox.Items.Clear();
            decimal total = 0;

            foreach (var product in cart)
            {
                decimal discountedPrice = product.Price - (product.Price * product.Discount / 100);
                string itemText = $"Product: {product.Name}, Price: {product.Price:C}, Quantity: {product.Quantity}, Discount: {product.Discount}%, Discounted Price: {discountedPrice:C}";
                cartListBox.Items.Add(itemText);
                total += discountedPrice * product.Quantity;
            }

            totalLabel.Text = $"Total Price: {total:C}";
        }

        private void DiscountTextBox_TextChanged(object sender, EventArgs e)
        {
            // Validate discount text
            if (!int.TryParse(discountTextBox.Text, out int discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Discount should be between 0 and 100.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                discountTextBox.Text = "0"; // Reset discount to 0 if invalid
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }

        public Product(string name, decimal price, int quantity, int discount)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            Discount = discount;
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SalesForm());
        }
    }
}
