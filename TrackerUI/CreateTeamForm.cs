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
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();
        private ITeamRequestor callingForm;
        
        public CreateTeamForm(ITeamRequestor caller)
        {
            InitializeComponent();
            callingForm = caller;
            //CreateSampleData();
            WireUpLists();
        }

     
        //Dummy data to check if it works
        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "Henrique", LastName = "Martins" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Vitor", LastName = "França" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Marie", LastName = "Martins" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Keyt", LastName = "Martins" });

        }

        private void WireUpLists()
        {
            //It makes the bind actually work and refresh the data
            selectTeamMemberDropDown.DataSource = null;

            //DataSOurce is basically the object that will feed the drop down
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            //DisplayMember is for the property we wish to display
            selectTeamMemberDropDown.DisplayMember = "FullName";
            selectTeamMemberDropDown.Refresh();

            //It makes the bind actually work and refresh the data
            teamMembersListBox.DataSource = null;

            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";

            //so that it refreshes when we add and remove
            teamMembersListBox.Refresh();
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                //creating a model
                PersonModel p = new PersonModel();
                //puting values in that model

                p.FirstName = firstNameValue.Text;
                p.LastName = lastNameValue.Text;
                p.EmailAddress = emailValue.Text;
                p.CellphoneNumber = cellphoneValue.Text;
                
                //executing connection for creating a person and then inserting that person into the box

                GlobalConfig.Connection.CreatePerson(p);

                

                selectedTeamMembers.Add(p);

                WireUpLists();
                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellphoneValue.Text = "";
                if (p.Id != 0)
                {
                    MessageBox.Show("Member Sucessfully created!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            else
            {
                MessageBox.Show("You need to fill in all of the fields");
            }
        }

        //can put stronger validation, at the moment we just check to see if fields are blanked
        private bool ValidateForm()
        {
            // TODO - Add validation to the form
            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }

            if (lastNameValue.Text.Length == 0)
            {
                return false;
            }

            if (emailValue.Text.Length == 0)
            {
                return false;
            }

            if (cellphoneValue.Text.Length == 0)
            {
                return false;
            }
            return true;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            //selectedItem is for the chosen person, and based on that we create a personModel
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;

            if (p != null)
            {
                //we remove from available and add to selected
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);
            }
            //then doing a cleanup
            WireUpLists();
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);
            }

            WireUpLists();
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = new TeamModel
            {
                TeamName = teamNameValue.Text,
                TeamMembers = selectedTeamMembers
            };

            GlobalConfig.Connection.CreateTeam(t);

            callingForm.TeamComplete(t);

            MessageBox.Show("Team sucessfully created!", "Team Created", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }
    }
}
