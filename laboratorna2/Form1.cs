using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.Net.Http;

namespace laboratorna2
{
    public partial class Form1 : Form
    {
        public Hotel _Hotel = new Hotel("#1 Hotel");
        public Form1()
        {
            _Hotel.Rooms = new List<Room>()
            {
                new Room(0, 4, 0, "", 200, Room.Status.FREE),
                new Room(1, 4, 0, "", 200, Room.Status.FREE),
                new Room(2, 4, 0, "", 200, Room.Status.FREE),
                new Room(3, 4, 0, "", 200, Room.Status.FREE),
                new Room(4, 4, 0, "", 200, Room.Status.FREE),
                new Room(5, 4, 0, "", 200, Room.Status.FREE),
                new Room(6, 4, 0, "", 200, Room.Status.FREE),
                new Room(7, 4, 0, "", 200, Room.Status.FREE),
                new Room(8, 4, 0, "", 200, Room.Status.FREE),
                new Room(9, 4, 0, "", 200, Room.Status.FREE)
            };
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime today = DateTime.Today;
                TimeSpan days = dateTimePicker1.Value - today;
                int _days = days.Days;
                var selectedRoom = dataGridView1.SelectedRows[0].DataBoundItem as Room;
                if (_days > 0)
                {
                    selectedRoom.TakenPlaces = Convert.ToInt32(comboBox1.SelectedItem);
                    selectedRoom.Date = dateTimePicker1.Text.ToString();
                    selectedRoom.CurrentStatus = Room.Status.LOCKED;
                    _Hotel.Cash += _days * selectedRoom.Price * selectedRoom.TakenPlaces;
                }
                FillData();
                SaveFile(_Hotel.Rooms);
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = _Hotel.Name;
            try
            {
                var roomsList = DeserializeJson<Room>("Rooms.json", typeof(List<Room>));
                _Hotel.Rooms = roomsList.ToList();
            }
            catch { }
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            comboBox1.SelectedIndex = 0;
            FillData();
            SerializeJson("Rooms.json", _Hotel.Rooms);
        }

        public void FillData()
        {
            dataGridView1.DataSource = _Hotel.Rooms;
            dataGridView1.Refresh();
            label1.Text = "Касса: " + _Hotel.Cash.ToString();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;
                        string jsonString = File.ReadAllText(filePath);
                        using (HttpClient httpClient = new HttpClient())
                        {
                            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                            var response = await httpClient.PostAsync("http://localhost:8080/api/endpoint", content);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public static void SerializeJson<T>(string fileName, IEnumerable<T> data)
        {
            var json = new DataContractJsonSerializer(data.GetType());
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                json.WriteObject(file, data);
            }
        }

        public static IEnumerable<T> DeserializeJson<T>(string fileName, Type result)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }
            var json = new DataContractJsonSerializer(result);
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return json.ReadObject(file) as List<T>;
            }
        }

        public static void SaveFile(List<Room> list)
        {
            File.Delete("Rooms.json");
            SerializeJson("Rooms.json", list);
        }
    }
}
