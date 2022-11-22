using System; //Exception, Environment, EventHandler
using System.Windows.Forms; //Label, MessageBox
using System.Drawing; //Color, Font Size, Point
using System.IO; //File
using System.Text; //Encoding
using System.Diagnostics; //Process
using System.Threading; //Thread
using System.Net.Sockets; //UdpClient
using System.Net; //IPEndPoint, udp
using System.Drawing.Text; //PrivateFontCollection

//класс MyForm
class MyForm : Form
{
    //кнопки и лейбл
    private Button btn1, btn2;
    private Label lb;

    //сокет
    private Thread rec;
    private UdpClient udp;
    private bool stopReceive;

    //1. конструктор
    public MyForm()
    {
        //сокет
        rec = null;
        udp = new UdpClient(15000); //15000 - порт сервера
        stopReceive = false;

        //форма
        ClientSize = new System.Drawing.Size(290, 200); //size of my form
        Text = "Server";
        ControlBox = true; //false - remove all the Control Buttons (e.g. Minimize, Maximize, Exit) and also the icon
        StartPosition = FormStartPosition.Manual;
        BackColor = Color.FromArgb(178, 255, 51);
        ShowInTaskbar = false;
        Left = 1305; //координаты формы
        Top = 30; //координаты формы
        ControlBox = false;
        MinimizeBox = false;
        MaximizeBox = false;

        //кнопка 1
        btn1 = new Button();
        btn1.Enabled = true;
        btn1.Text = "Start";
        btn1.Font = new Font("Arial", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
        btn1.Size = new Size(50, 30);
        btn1.FlatAppearance.BorderSize = 0;
        btn1.FlatStyle = FlatStyle.Flat;
        btn1.BackColor = Color.Red;
        btn1.FlatAppearance.BorderColor = Color.Red; //border color
        btn1.ForeColor = Color.White;
        btn1.Location = new Point(70, 110); //coordinates of my button
        Controls.Add(btn1);
        btn1.Click += new EventHandler(btn1_Click);

        //кнопка 2
        btn2 = new Button();
        btn2.Enabled = true;
        btn2.Text = "Stop";
        btn2.Font = new Font("Arial", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
        btn2.Size = new Size(50, 30);
        btn2.FlatAppearance.BorderSize = 0;
        btn2.FlatStyle = FlatStyle.Flat;
        btn2.BackColor = Color.SeaGreen;
        btn2.FlatAppearance.BorderColor = Color.SeaGreen; //border color
        btn2.ForeColor = Color.White;
        btn2.Location = new Point(180, 110); //coordinates of my button
        Controls.Add(btn2);
        btn2.Click += new EventHandler(btn2_Click);

        //лейбл 
        lb = new Label();
        lb.AutoSize = true;
        lb.Font = new System.Drawing.Font("Arial", 20.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        lb.Location = new System.Drawing.Point(1, 15); //coordinates of the question
        lb.Size = new System.Drawing.Size(352, 24);
        lb.Text = "Server Delete File";
        lb.ForeColor = Color.FromArgb(144, 12, 63);
        Controls.Add(lb);
    }

    //2. обработчик первой кнопки Start
    private void btn1_Click(object sender, EventArgs e)
    {
        stopReceive = false;
        rec = new Thread(new ThreadStart(ReceiveMessage));
        rec.Start();
        lb.Text = "Started listening";
        lb.Location = new System.Drawing.Point(20, 15); //coordinates of the question
    }

    //3. обработчик второй кнопки Stop
    private void btn2_Click(object sender, EventArgs e)
    {
        lb.Text = "Stop";
        lb.Location = new System.Drawing.Point(110, 15);
        stopReceive = true;
        if (udp != null) udp.Close();
        if (rec != null) rec.Join();
        Close();
    }

    //4. функция ReceiveMessage для передачи процессу
    void ReceiveMessage()
    {
        try
        {
            while (true)
            {
                //получаем данные от клиента
                IPEndPoint ipendpoint = null;
                byte[] message = udp.Receive(ref ipendpoint);//ipendpoint.Address.ToString() = 192.168.1.199 - ip клиента
                //ipendpoint.Port.ToString() = 64696 - порта клиента
                MessageBox.Show("N=" + message.Length);

                //for example, mymessage = "a.txt"
                string mymessage = "";
                for (int i = 0; i < message.Length; i++)
                    mymessage += (char)message[i];

                //delete file a.txt
                File.Delete("D:\\NahrinM\\" + mymessage);
                Thread.Sleep(1500);
                MessageBox.Show("File " + mymessage + " has been deleted..");

                if (stopReceive == true) break;
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    //5. Главный модуль 
    [STAThread]
    static void Main()
    {
        MyForm f = new MyForm();
        Application.Run(f);
    }
}