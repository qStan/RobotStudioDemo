using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private NetworkScanner scanner = null;
        private ABBRobotClass myRobot = null;
        private Controller controller = null;
        private Task[] tasks = null;
        private NetworkWatcher networkwatcher = null;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.scanner = new NetworkScanner();
            this.scanner.Scan();
            ControllerInfoCollection controllers = scanner.Controllers;
            foreach (ControllerInfo info in controllers)
            {
                comboBox1.Items.Add(info.ControllerName + " / " + info.IPAddress.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ControllerInfoCollection controllers = scanner.Controllers;
            foreach (ControllerInfo info in controllers)
            {
                if (comboBox1.Text.Equals(info.ControllerName + " / " + info.IPAddress.ToString()))
                {
                    if (info.Availability == Availability.Available)
                    {
                        if (myRobot != null)
                        {
                            myRobot.Dispose(); // = LogOff
                            myRobot = null;
                        }
                        myRobot = new ABBRobotClass(ControllerFactory.CreateFrom(info));
                        myRobot.Controller.ConnectionChanged += new EventHandler<ConnectionChangedEventArgs>(ConnectionChanged);
                        ConnectButton.BackColor = Color.Green;
                        MessageBox.Show("You are now connected");
                        break;
                    }
                }
                {
                    MessageBox.Show("Selected controller not available.");
                }
            }
            if (myRobot == null) MessageBox.Show("Selected controller not available. (comboBox String != controller info)");
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        
        public void ConnectionChanged(object sender, EventArgs e)
        {
            if (myRobot.Controller.Connected == true) ConnectButton.BackColor = Color.Green;
            else ConnectButton.BackColor = Color.Red;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myRobot != null)
            {
                myRobot.Dispose(); // = LogOff
                myRobot = null;
            }
        }
    }

}
