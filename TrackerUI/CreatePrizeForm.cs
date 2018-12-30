using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        IPrizeRequester callingForm;

        public CreatePrizeForm(IPrizeRequester caller)
        {
            InitializeComponent();
            callingForm = caller;
        }

        private void addPrizeButton_Click(object sender, EventArgs e)
        {

            if (ValidateForm())
            {
                //creating a prize model and putting the information from there
                PrizeModel model = new PrizeModel(
                    placeNameValue.Text,
                    placeNumberValue.Text,
                    prizeAmountValue.Text,
                    prizePercentageValue.Text);

                //because Connection has a contract with interface IData which contain CreatePrize
                //allows us to have this.
                GlobalConfig.Connection.CreatePrize(model);

                //because we make an instance of IPrizeRequester calling form, which has a method PrizeComplete
                //that accepts prizeModel, we can pass it through
                callingForm.PrizeComplete(model);

                this.Close();
                //Clear form of previous inputs
                placeNameValue.Text = "";
                placeNumberValue.Text = "";
                prizeAmountValue.Text = "0";
                prizePercentageValue.Text = "0";
            }
            else
            {
                MessageBox.Show("This form has invalid information. Please check it and try again.");
            }

        }

        private bool ValidateForm()
        {
            //initialy output (what user entered is ok, then it passess a number of checks)
            bool output = true;
            int placeNumber = 0;
            //checking if there is a valid number
            bool placeNumberValidNumber = int.TryParse(placeNumberValue.Text, out placeNumber);

            //if anything fails then output is false and we return output at the end

            if (placeNumberValidNumber == false)
            {
                MessageBox.Show("Not a valid place number value.");
                output = false;
            }

            if (placeNumber < 1)
            {
                output = false;
            }

            if (placeNameValue.Text.Length == 0)
            {
                output = false;
            }

            decimal prizeAmount = 0;
            double prizePercentage = 0;

            bool prizeAmountValid = decimal.TryParse(prizeAmountValue.Text, out prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageValue.Text, out prizePercentage);

            if (prizeAmountValid == false || prizePercentageValid == false)
            {
                output = false;
            }

            if (prizeAmount < 0 && prizePercentage <= 0)
            {
                output = false;
            }

            if (prizePercentage < 0 || prizePercentage > 100)
            {
                output = false;
            }

            return output;
        }
    }
}

