using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
//created by Christina Yuen

namespace AddressBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Contacts> contact = new List<Contacts>();
        
        class Contacts
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
        }
        

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* This function is for the add contact button. I also wanted the contact to display the first and last name
             * on the listview when contact is added.
             * It will also clear all inputted data when contact is added.*/

            
            Contacts a = new Contacts();
            listView1.Sorting = SortOrder.Ascending;

            //contact.OrderBy(x => x).ToList();
           

            a.FirstName = textBox1.Text;
            a.LastName = textBox2.Text;
            a.Email = textBox3.Text;
            a.Mobile = textBox4.Text;
            contact.Add(a);
            listView1.Items.Add(a.FirstName + " " + a.LastName);

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear(); 
            textBox4.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* This function is written for the delete button. I have written two lines of code because I want to
             delete the name displayed in listview and also the element in the List<> field. I also wanted to clear all the 
             information inputted in the textbox when deleteing the contact.*/

            if (listView1.SelectedItems.Count > 0)
            {
                contact.RemoveAt(listView1.SelectedIndices[0]);
                listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            /* This function is written for the save button. The first four lines is so that the save button will save
             all the information entered. The last line of code is that if there is any editing to the first name, 
             buy pressing save, the name displayed in the listview will also update and save.*/
            if (listView1.SelectedItems.Count > 0)
            {
                contact[listView1.SelectedIndices[0]].FirstName = textBox1.Text;
                contact[listView1.SelectedIndices[0]].LastName = textBox2.Text;
                contact[listView1.SelectedIndices[0]].Mobile = textBox3.Text;
                contact[listView1.SelectedIndices[0]].Email = textBox4.Text;
                listView1.SelectedItems[0].Text = textBox1.Text;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            /* Here I have created a path to an xml file under the folder name app data.
             * I have used an if statement so that if it does not exist, it will need to create one.
             I have also created a root element for the path. The start root will begin with the name 'contacts*/

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(path + "\\Address Book - Christina"))
            {
                Directory.CreateDirectory(path + "\\Address Book - Christina");
            }

            if (!File.Exists(path + "\\Address Book - Christina\\settings.xml"))
            {
                XmlTextWriter xWriter = new XmlTextWriter(path + "\\Address Book - Christina\\settings.xml", Encoding.UTF8);
                xWriter.WriteStartElement("Contacts");
                xWriter.WriteEndElement();
                xWriter.Close();
            }  
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* This function is so that it updates when we select a different person on the listview */

            if(listView1.SelectedItems.Count > 0)
            {
                textBox1.Text = contact[listView1.SelectedIndices[0]].FirstName;
                textBox2.Text = contact[listView1.SelectedIndices[0]].LastName;
                textBox3.Text = contact[listView1.SelectedIndices[0]].Mobile;
                textBox4.Text = contact[listView1.SelectedIndices[0]].Email;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /* This function is to access all the contacts from the contacts list and write it to the xml file */
 
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path + "\\Address Book - Christina\\settings.xml");
            XmlNode xNode = xDoc.SelectSingleNode("Contacts");
            xNode.RemoveAll();

            foreach(Contacts a in contact)
            {
                /*Create a new node for each and store in the xMain node */
                XmlNode xMain = xDoc.CreateElement("Contact");
                XmlNode xFirstName = xDoc.CreateElement("FirstName");
                XmlNode xLastName = xDoc.CreateElement("LastName");
                XmlNode xMobile = xDoc.CreateElement("Mobile");
                XmlNode xEmail = xDoc.CreateElement("Email");

                /* Store the information in the above nodes with the information entered in the form */
                xFirstName.InnerText = a.FirstName;
                xLastName.InnerText = a.LastName;
                xMobile.InnerText = a.Mobile;
                xEmail.InnerText = a.Email;

                /* Store all the sub nodes into the xMain.*/
                xMain.AppendChild(xFirstName);
                xMain.AppendChild(xLastName);
                xMain.AppendChild(xMobile);
                xMain.AppendChild(xEmail);

                /* Add the xMain node to the document of the Contacts node */
                xDoc.DocumentElement.AppendChild(xMain);
            }

            /* Save the document to the provided path */
            xDoc.Save(path + "\\Address Book - Christina\\settings.xml");

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            /* This function is to use shortcuts to buttons, for accessibility */

            if(e.Alt && e.KeyCode == Keys.D)
            {
                btnDelete.PerformClick();
            }

            if(e.Alt && e.KeyCode == Keys.A)
            {
                btnAddContact.PerformClick();
            }

            if(e.Alt && e.KeyCode == Keys.S)
            {
                btnSave.PerformClick();
            }

            if (e.KeyCode == Keys.Escape) 
            {
                Close();
            }
        }
       
    }
}
