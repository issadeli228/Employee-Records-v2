using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace EmployeeRecords
{
    public partial class mainForm : Form
    {
        List<Employee> employeeDB = new List<Employee>();

        public mainForm()
        {
            InitializeComponent();
            loadDB();
        }

        private void addButton_Click(object sender, EventArgs e)
        {

            //grab info from user
            string newID = idInput.Text;
            string newFN = fnInput.Text;
            string newLN = lnInput.Text;
            string newDate = dateInput.Text;
            string newSalary = salaryInput.Text;

            //create object from info
            Employee newEmployee = new Employee(newID, newFN, newLN, newDate, newSalary);

            //sotre object in list
            employeeDB.Add(newEmployee);
            

            ClearLabels();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            foreach (Employee emp in employeeDB)
            {
                if(emp.id == idInput.Text)
                {
                    employeeDB.Remove(emp);
                    ClearLabels();
                    outputLabel.Text = $"Employee {emp.firstName} has been removed";
                    return;
                }
            }
            outputLabel.Text = $"Employee not found";
        }

        private void listButton_Click(object sender, EventArgs e)
        {
            outputLabel.Text = "";

            foreach(Employee emp in employeeDB)
            {
                outputLabel.Text += $"{emp.id} {emp.firstName} {emp.lastName} {emp.date} {emp.salary} \n";
            }
        }

        private void ClearLabels()
        {
            idInput.Text = "";
            fnInput.Text = "";
            lnInput.Text = "";
            dateInput.Text = "";
            salaryInput.Text = "";
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            XmlWriter writer = XmlWriter.Create("bin/debug/employeeData.xml", null);

            writer.WriteStartElement("Employees");

            foreach(Employee emp in employeeDB)
            {
                writer.WriteStartElement("Employee");

                writer.WriteElementString("id", emp.id);
                writer.WriteElementString("firstName", emp.firstName);
                writer.WriteElementString("lastName", emp.lastName);
                writer.WriteElementString("date", emp.date);
                writer.WriteElementString("salary", emp.salary);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.Close();
        }

        public void loadDB()
        {
            string id, fn, ln, date, salary;

            XmlReader reader = XmlReader.Create("bin/debug/employeeData.xml");

            while(reader.Read())
            {
                if(reader.NodeType == XmlNodeType.Text)
                {
                    id = reader.ReadString();

                    reader.ReadToNextSibling("firstName");
                    fn = reader.ReadString();

                    reader.ReadToNextSibling("lastName");
                    ln = reader.ReadString();

                    reader.ReadToNextSibling("date");
                    date = reader.ReadString();

                    reader.ReadToNextSibling("salary");
                    salary = reader.ReadString();

                    Employee newemp = new Employee(id, fn, ln, date, salary);
                    employeeDB.Add(newemp);
                }
            }

            reader.Close();
        }

        public void saveDB()
        {

        }
    }
}
