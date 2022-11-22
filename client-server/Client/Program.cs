using System; //EventHandler
using System.Windows.Forms; //Form
using System.Drawing; //Point
using System.Drawing.Text; //PrivateFontCollection
using System.Text; //Encoding
using System.IO; //File
using System.Net; //IPAddress, IPEndpoint
using System.Net.Sockets; //UdpClient
using System.Threading; //Sleep
using System.Net.NetworkInformation; //Ping
using System.Drawing.Drawing2D; //GraphicsPath

//класс CircularButton
class CircularButton : Button
{
    protected override void OnPaint(PaintEventArgs pevent)
    {
        GraphicsPath gp = new GraphicsPath();
        gp.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
        this.Region = new Region(gp);
        base.OnPaint(pevent);
    }
}

//class My1Form
class My1Form : Form
{
    private CircularButton btn1, btn2;
    private Label lb;
    private TextBox tbox1, tbox2;

    //1 constructor
    public My1Form()
    {
        //form
        ClientSize = new Size(280, 240); //size of my form
        Text = "Client";
        StartPosition = FormStartPosition.Manual;
        Left = 1330;
        Top = 20;
        BackColor = Color.FromArgb(51, 190, 255);
        ControlBox = false;
        MinimizeBox = false;
        MaximizeBox = false;

        //button 1
        btn1 = new CircularButton();
        btn1.Enabled = true;
        btn1.Text = "Delete";
        btn1.Font = new Font("Comic Sans MS", 11.9F, FontStyle.Bold, GraphicsUnit.Point);
        btn1.Size = new Size(65, 65);
        btn1.FlatAppearance.BorderSize = 0;
        btn1.FlatStyle = FlatStyle.Flat;
        btn1.BackColor = Color.Red;
        btn1.FlatAppearance.BorderColor = Color.Red; //border color
        btn1.ForeColor = Color.Yellow;
        btn1.Location = new Point(55, 160); //button1 coordinates
        Controls.Add(btn1);
        btn1.Click += new EventHandler(btn1_Click);

        //button 2
        btn2 = new CircularButton();
        btn2.Enabled = true;
        btn2.Text = "Exit";
        btn2.Font = new Font("Comic Sans MS", 11.9F, FontStyle.Bold, GraphicsUnit.Point);
        btn2.Size = new Size(65, 65);
        btn2.FlatAppearance.BorderSize = 0;
        btn2.FlatStyle = FlatStyle.Flat;
        btn2.BackColor = Color.Blue;
        btn2.FlatAppearance.BorderColor = Color.Blue; //border color
        btn2.ForeColor = Color.Yellow;
        btn2.Location = new Point(180, 160); //button2 coordinates
        Controls.Add(btn2);
        btn2.Click += new EventHandler(btn2_Click);

        //label
        lb = new Label();
        lb.AutoSize = true;
        //pfc.Families[0]: AlexBrush-Regular
        lb.Font = new Font("Arial", 20F, FontStyle.Regular);//0,1,7
        lb.Location = new Point(5, 1); //coordinates of the question
        lb.Size = new Size(352, 24);
        lb.Text = "Protocol UDP";
        lb.ForeColor = Color.FromArgb(255, 17, 134);
        Controls.Add(lb);

        //textbox1 
        tbox1 = new TextBox();
        tbox1.Location = new Point(65, 65); //textbox location
        tbox1.Size = new Size(160, 20); //textbox size
        tbox1.BackColor = SystemColors.GradientActiveCaption;
        tbox1.ForeColor = Color.Black; //font color
        Font myfont = new Font("Arial", 16.0f); //font size
        tbox1.Font = myfont;
        tbox1.RightToLeft = RightToLeft.No;
        tbox1.BorderStyle = BorderStyle.FixedSingle;
        //tbox1.Text = "192.168.1.104"; //this is a server IP (or 127.0.0.1 for cumputer without network)
        tbox1.Text = "127.0.0.1";
        Controls.Add(tbox1);

        //textbox2
        tbox2 = new TextBox();
        tbox2.Location = new Point(65, 115); //textbox location
        tbox2.Size = new Size(160, 20); //textbox size
        tbox2.BackColor = SystemColors.GradientActiveCaption;
        tbox2.ForeColor = Color.Black; //font color
        myfont = new Font("Arial", 16.0f); //font size
        tbox2.Font = myfont;
        tbox2.RightToLeft = RightToLeft.No;
        tbox2.BorderStyle = BorderStyle.FixedSingle;
        tbox2.Text = "a.txt"; //name of the file
        Controls.Add(tbox2);
    }

    //2 button1
    private void btn1_Click(object sender, EventArgs e)
    {
        if (IsConnectedToInternet())
        {
            string ipAddress = tbox1.Text; //this is a server ip
            int port = 15000; //this is a server port
            string nameOfFile = tbox2.Text; //this is a name of the file

            //send name of the file to server
            int theNumberOfBytes = udpSend(ipAddress, port, nameOfFile);
            MessageBox.Show("Name of the file " + nameOfFile + " has been send..");
        }
        else
            MessageBox.Show("Check your internet connection..");
    }

    //3 button2 
    private void btn2_Click(object sender, EventArgs e)
    {
        Close();
    }

    //4 udpSend
    private int udpSend(string ipAddress, int port, string myMessage)
    {
        byte[] myMessageBytes = Encoding.UTF8.GetBytes(myMessage);
        UdpClient udp = new UdpClient();
        IPAddress ipad = IPAddress.Parse(ipAddress);
        IPEndPoint ipen = new IPEndPoint(ipad, port);
        int send = udp.Send(myMessageBytes, myMessageBytes.Length, ipen);
        udp.Close();
        return send;
    }

    //5 IsConnectedToInternet
    public bool IsConnectedToInternet()
    {
        try
        {
            Ping myPing = new Ping();
            String host = "google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
            return (reply.Status == IPStatus.Success);
        }
        catch (Exception)
        {
            return false;
        }
    }

    //11 Main 
    [STAThread]
    static void Main()
    {
        My1Form f = new My1Form();
        Application.Run(f);
    }
}